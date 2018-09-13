using System;
using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitHub_Tool.Model;
using Model = GitHub_Tool.Model;
using System.Diagnostics;
using System.Linq;

namespace GitHub_Tool.Action
{
    class RepoSearch
    {
        public async Task<List<Model.Repository>> searchRepos( Model.SearchRepositoriesRequestParameters requestParameters)
        {

            List<Model.Repository> repos = new List<Model.Repository>();
            var repoRequest = getSearchRepoRequest(requestParameters);
            var numberOfPages = 3;
            
            for (int i = 0; i < numberOfPages; i++)
            {
                repoRequest.Page += 1;
                var result = await GlobalVariables.client.Search.SearchRepo(repoRequest);

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

            return repos;
        }


        private SearchRepositoriesRequest getSearchRepoRequest(Model.SearchRepositoriesRequestParameters parameters)
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

            repoRequest.Page = 0;
            repoRequest.Forks = Range.GreaterThanOrEquals(parameters.Forks);
            repoRequest.Stars = Range.GreaterThanOrEquals(parameters.Stars);
            repoRequest.Size = Range.GreaterThan(parameters.Size);
            repoRequest.SortField = getSortBy(parameters.SortBy);

            Language language = (Language)Enum.Parse(typeof(Language), parameters.Language);
            repoRequest.Language = language;

            if (parameters.ReadmeIncluded == true && parameters.Term != null)
            {
                repoRequest.In = new[] { InQualifier.Readme, InQualifier.Description, InQualifier.Name };
            }

            if (parameters.Owner != null)
            {
                repoRequest.User = parameters.Owner;
            }
 
            if (parameters.Order == "Ascending")
            {
                repoRequest.Order = SortDirection.Ascending;
            }

            if (parameters.Date.HasValue )
            {                
                repoRequest.Created =  checkDateChoice(parameters);
            }

            if (parameters.UpdatedAt.HasValue)
            {
                repoRequest.Updated = DateRange.GreaterThanOrEquals(parameters.UpdatedAt.Value);
            }

            return repoRequest;
        }


        private DateRange checkDateChoice(SearchRepositoriesRequestParameters parameters)
        {
            DateRange dateRange;

            if (parameters.DateChoice.Equals("Created after"))
            {
                dateRange = DateRange.GreaterThanOrEquals(parameters.Date.Value);
            }
            else if (parameters.DateChoice.Equals("Created before"))
            {
                dateRange = DateRange.LessThanOrEquals(parameters.Date.Value);
            }
            else
            {
                if (parameters.EndDate.HasValue)
                {
                    dateRange = DateRange.Between(parameters.Date.Value, parameters.EndDate.Value);
                }
                else
                {
                    dateRange = DateRange.GreaterThanOrEquals(parameters.Date.Value);
                }

            }

            return dateRange;
        }


        private RepoSearchSort getSortBy(string sortBy)
        {
            RepoSearchSort repoSearchSort;

            switch (sortBy)
            {
                case "Stars":
                    repoSearchSort = RepoSearchSort.Stars;
                    break;
                case "Forks":
                    repoSearchSort = RepoSearchSort.Forks;
                    break;
                case "Updated":
                    repoSearchSort = RepoSearchSort.Updated;
                    break;
                default:
                    repoSearchSort = new RepoSearchSort();
                    break;
            }

            return repoSearchSort;
        }


    }
}
