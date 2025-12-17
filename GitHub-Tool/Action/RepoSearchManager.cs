using System;
using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitHubSearch.Model;
using GitHubSearch.Services;
using System.Linq;

namespace GitHubSearch.Action
{
    class RepoSearchManager
    {
        private readonly GitHubClientService _clientService;

        public RepoSearchManager(GitHubClientService clientService)
        {
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
        }

        public async Task<List<Model.Repository>> SearchRepositoriesAsync(
            SearchRepositoriesRequestParameters requestParameters,
            int firstPage,
            int lastPage)
        {
            List<Model.Repository> repos = new List<Model.Repository>();
            SearchRepositoriesRequest repoRequest = BuildSearchRequest(requestParameters);

            for (int i = firstPage; i <= lastPage; i++)
            {
                repoRequest.Page = i;
                SearchRepositoryResult result = await _clientService.Client.Search
                    .SearchRepo(repoRequest)
                    .ConfigureAwait(false);

                repos.AddRange(result.Items.Select(repo => MapToRepository(repo)).ToList());
            }

            return repos;
        }


        public async Task<int> GetPageCountAsync(Model.SearchRepositoriesRequestParameters requestParameters)
        {
            SearchRepositoriesRequest repoRequest = BuildSearchRequest(requestParameters);
            SearchRepositoryResult result = await _clientService.Client.Search
                .SearchRepo(repoRequest)
                .ConfigureAwait(false);

            return _clientService.CalculatePageCount(result.TotalCount, repoRequest.PerPage);
        }


        private SearchRepositoriesRequest BuildSearchRequest(SearchRepositoriesRequestParameters parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters.Term))
            {
                throw new ArgumentException("A search term is required for GitHub repository search.");
            }

            SearchRepositoriesRequest repoRequest = new SearchRepositoriesRequest(parameters.Term);

            if (!string.IsNullOrWhiteSpace(parameters.Language))
            {
                if (Enum.TryParse<Language>(parameters.Language, true, out var language))
                {
                    repoRequest.Language = language;
                }
            }

            if (!string.IsNullOrWhiteSpace(parameters.Owner))
            {
                repoRequest.User = parameters.Owner;
            }

            repoRequest.SortField = GetSortBy(parameters.SortBy);
            repoRequest.Order = GetSortDirection(parameters.Order);

            if (!string.IsNullOrWhiteSpace(parameters.ForksChoice))
            {
                repoRequest.Forks = PickRange(parameters.ForksChoice, parameters.Forks);
            }

            if (!string.IsNullOrWhiteSpace(parameters.StarsChoice))
            {
                repoRequest.Stars = PickRange(parameters.StarsChoice, parameters.Stars);
            }

            if (!string.IsNullOrWhiteSpace(parameters.SizeChoice))
            {
                repoRequest.Size = PickRange(parameters.SizeChoice, parameters.Size);
            }

            repoRequest.In = GetInParameters(parameters.ReadmeIncluded, parameters.Term);

            var createdRange = GetCreatedAtParameter(parameters.DateChoice, parameters.Date, parameters.EndDate);
            if (createdRange != null)
            {
                repoRequest.Created = createdRange;
            }

            var updatedRange = GetUpdatedAtParameter(parameters.UpdatedAt);
            if (updatedRange != null)
            {
                repoRequest.Updated = updatedRange;
            }

            return repoRequest;
        }


        private static Model.Repository MapToRepository(Octokit.Repository repo)
        {
            return new Model.Repository
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
            };
        }


        private static Range PickRange(string choice, int value)
        {
            if (!string.IsNullOrWhiteSpace(choice) && choice.Equals("More than", StringComparison.OrdinalIgnoreCase))
            {
                return Range.GreaterThanOrEquals(value);
            }

            return Range.LessThanOrEquals(value);
        }


        private static InQualifier[] GetInParameters(bool? readmeIncluded, string term)
        {
            if (readmeIncluded == true && !string.IsNullOrWhiteSpace(term))
            {
                return new[] { InQualifier.Readme, InQualifier.Description, InQualifier.Name };
            }
            return new[] { InQualifier.Description, InQualifier.Name };
        }


        private static SortDirection GetSortDirection(string choice)
        {
            if (!string.IsNullOrWhiteSpace(choice) && choice.Equals("Ascending", StringComparison.OrdinalIgnoreCase))
            {
                return SortDirection.Ascending;
            }
            return SortDirection.Descending;
        }


        private static DateRange GetUpdatedAtParameter(DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }

            return DateRange.GreaterThanOrEquals(date.Value);
        }


        private static DateRange GetCreatedAtParameter(string dateChoice, DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(dateChoice))
            {
                return DateRange.GreaterThanOrEquals(startDate.Value);
            }

            if (dateChoice.Equals("Created after", StringComparison.OrdinalIgnoreCase))
            {
                return DateRange.GreaterThanOrEquals(startDate.Value);
            }
            else if (dateChoice.Equals("Created before", StringComparison.OrdinalIgnoreCase))
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


        private static RepoSearchSort GetSortBy(string sortBy)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return new RepoSearchSort();
            }

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