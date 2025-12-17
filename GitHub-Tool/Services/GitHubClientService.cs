using Octokit;
using System;

namespace GitHubSearch.Services
{
    public class GitHubClientService
    {
        private GitHubClient _client;
        private const string ApplicationName = "GitHub-Tool";

        public const int MaximumNumberOfPages = 10;
        public const int ResultsPerPage = 100;
        public const int MaxTotalResults = 1000;

        public bool IsAuthenticated => _client?.Credentials?.AuthenticationType == AuthenticationType.Oauth;

        public GitHubClient Client
        {
            get
            {
                if (_client == null)
                {
                    throw new InvalidOperationException(
                        "GitHub client has not been initialized. Call Authenticate() first.");
                }
                return _client;
            }
        }

        public void Authenticate(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentException("Access token cannot be empty.", nameof(accessToken));
            }

            _client = new GitHubClient(new ProductHeaderValue(ApplicationName))
            {
                Credentials = new Credentials(accessToken)
            };
        }

        public void CreateAnonymousClient()
        {
            _client = new GitHubClient(new ProductHeaderValue(ApplicationName));
        }

        public int CalculatePageCount(int totalCount, int perPage = ResultsPerPage)
        {
            if (totalCount == 0)
            {
                return 0;
            }

            // GitHub limits search results to 1000
            int effectiveTotal = Math.Min(totalCount, MaxTotalResults);

            if (effectiveTotal > MaxTotalResults)
            {
                return MaximumNumberOfPages;
            }

            return (int)Math.Ceiling((double)effectiveTotal / perPage);
        }
    }
}