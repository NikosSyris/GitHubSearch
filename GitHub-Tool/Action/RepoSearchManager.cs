using System;
using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitHubSearch.Model;
using Model = GitHubSearch.Model;
using System.Linq;
using System.Diagnostics;

namespace GitHubSearch.Action
{
    class RepoSearchManager
    {
        public async Task<List<Model.Repository>> searchRepos(SearchRepositoriesRequestParameters requestParameters, int firstPage, int lastPage)
        {

            List<Model.Repository> repos = new List<Model.Repository>();
            SearchRepositoriesRequest repoRequest = getSearchRepoRequest(requestParameters);
            
            for (int i = firstPage; i <= lastPage; i++)
            {
                repoRequest.Page = i;
                SearchRepositoryResult result = await GlobalVariables.client.Search.SearchRepo(repoRequest);

                repos.AddRange(result.Items.Select(repo => new Model.Repository
                {
                    Name = repo.Name,
                    Owner = repo.Owner.Login,
                    Size = repo.Size,
                    CreatedAt = repo.CreatedAt,
                    UpdatedAt = repo.UpdatedAt,
                    StargazersCount = repo.StargazersCount,
                    HtmlUrl = repo.HtmlUrl,
                    Description = repo.Description,
                    ForksCount = repo.ForksCount,
                    Language = repo.Language

                }).ToList());
            }
            Debug.WriteLine(repos.Count);
            return repos;
        }


        private SearchRepositoriesRequest getSearchRepoRequest(SearchRepositoriesRequestParameters parameters)
        {
            SearchRepositoriesRequest repoRequest;

            try
            {
                repoRequest = new SearchRepositoriesRequest(parameters.Term);
            }
            catch (Exception)
            {
                repoRequest = new SearchRepositoriesRequest();      
            }

            Language language = (Language)Enum.Parse(typeof(Language), parameters.Language);

            repoRequest.User = parameters.Owner;
            repoRequest.Language = language;
            repoRequest.SortField = getSortBy(parameters.SortBy);
            repoRequest.Order = getSortDirection(parameters.Order);
            repoRequest.Forks = pickRange(parameters.ForksChoice, parameters.Forks);
            repoRequest.Stars = pickRange(parameters.StarsChoice, parameters.Stars); 
            repoRequest.Size =  pickRange(parameters.SizeChoice, parameters.Size);
            repoRequest.In = getInParameters(parameters.ReadmeIncluded, parameters.Term);
            repoRequest.Created = getCreatedAtParameter(parameters.DateChoice, parameters.Date, parameters.EndDate);
            repoRequest.Updated = getUpdatedAtParameter(parameters.UpdatedAt);
            
            return repoRequest;
        }


        public async Task<int> getNumberOfPages(Model.SearchRepositoriesRequestParameters requestParameters)
        {
            SearchRepositoriesRequest repoRequest = getSearchRepoRequest(requestParameters);
            SearchRepositoryResult result = await GlobalVariables.client.Search.SearchRepo(repoRequest);

            if (result.TotalCount == 0)
            {
                return 0;
            }

            if (result.TotalCount > 1000)
            {
                return GlobalVariables.MaximumNumberOfPages;
            }

            if (result.TotalCount % repoRequest.PerPage == 0)
            {
                return result.TotalCount / repoRequest.PerPage;
            }

            return result.TotalCount / repoRequest.PerPage + 1;
        }


        private Range pickRange(string choice, int value)
        {

            if (choice.Equals("More than"))
            {
                return Range.GreaterThanOrEquals(value);
            }

            return Range.LessThanOrEquals(value);
        }


        private InQualifier[] getInParameters(bool? readmeIncluded, string term)
        {
            if (readmeIncluded == true && term != null)
            {
                return new[] { InQualifier.Readme, InQualifier.Description, InQualifier.Name };
            }
            return new[] { InQualifier.Description, InQualifier.Name };
        }


        private SortDirection getSortDirection(string choice)
        {
            if ( choice.Equals("Ascending") )
            {
                return SortDirection.Ascending;
            }
            return SortDirection.Descending;
        }

        private DateRange getUpdatedAtParameter(DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }

            return DateRange.GreaterThanOrEquals(date.Value);
        }


        private DateRange getCreatedAtParameter(string dateChoice, DateTime? startDate, DateTime? endDate)
        {
            
            if ( !startDate.HasValue )
            {
                return null;
            }

            if (dateChoice.Equals("Created after"))
            {
                return DateRange.GreaterThanOrEquals(startDate.Value);
            }
            else if (dateChoice.Equals("Created before"))
            {
                return DateRange.LessThanOrEquals(startDate.Value);
            }
            else
            {
                if (endDate.HasValue)
                {
                    return DateRange.Between(startDate.Value, endDate.Value);
                }

                return DateRange.GreaterThanOrEquals(startDate.Value);
            }               
        }


        private RepoSearchSort getSortBy(string sortBy)
        {
            switch (sortBy)
            {
                case "Stars":
                    return RepoSearchSort.Stars;
                case "Forks":
                    return RepoSearchSort.Forks;
                case "Updated":
                    return RepoSearchSort.Updated;
                default:
                    return new RepoSearchSort();
            }
        }
    }
}
