using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;
using GitHubSearch.Model;
using GitHubSearch.Services;

namespace GitHubSearch.Action
{
    public class CodeSearchManager
    {
        private readonly GitHubClientService _clientService;
        private readonly SearchRequestBuilder _requestBuilder;

        public CodeSearchManager(GitHubClientService clientService, SearchRequestBuilder requestBuilder)
        {
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
            _requestBuilder = requestBuilder ?? throw new ArgumentNullException(nameof(requestBuilder));
        }

        public CodeSearchManager(GitHubClientService clientService)
            : this(clientService, new SearchRequestBuilder())
        {
        }

        public async Task<List<File>> SearchCodeAsync(
            SearchCodeRequestParameters requestParameters,
            int firstPage,
            int lastPage)
        {
            ValidatePageRange(firstPage, lastPage);

            var codeRequest = _requestBuilder.BuildCodeSearchRequest(requestParameters);
            var files = new List<File>();

            for (int page = firstPage; page <= lastPage; page++)
            {
                var pageResults = await ExecuteSearchAsync(codeRequest, page).ConfigureAwait(false);
                files.AddRange(pageResults);
            }

            return files;
        }

        public async Task<List<File>> SearchCodeAsync(
            SearchCodeRequestParameters requestParameters,
            int page = 1)
        {
            return await SearchCodeAsync(requestParameters, page, page).ConfigureAwait(false);
        }

        public async Task<int> GetPageCountAsync(SearchCodeRequestParameters requestParameters)
        {
            var codeRequest = _requestBuilder.BuildCodeSearchRequest(requestParameters);
            var result = await _clientService.Client.Search
                .SearchCode(codeRequest)
                .ConfigureAwait(false);

            return _clientService.CalculatePageCount(result.TotalCount, codeRequest.PerPage);
        }

        public async Task<int> GetTotalCountAsync(SearchCodeRequestParameters requestParameters)
        {
            var codeRequest = _requestBuilder.BuildCodeSearchRequest(requestParameters);
            var result = await _clientService.Client.Search
                .SearchCode(codeRequest)
                .ConfigureAwait(false);

            return result.TotalCount;
        }

        private async Task<List<File>> ExecuteSearchAsync(SearchCodeRequest codeRequest, int page)
        {
            codeRequest.Page = page;

            var result = await _clientService.Client.Search
                .SearchCode(codeRequest)
                .ConfigureAwait(false);

            return result.Items.Select(MapToFile).ToList();
        }

        private static File MapToFile(SearchCode searchResult)
        {
            return new File
            {
                Name = searchResult.Name,
                Path = searchResult.Path,
                RepoName = searchResult.Repository.Name,
                Owner = searchResult.Repository.Owner.Login,
                HtmlUrl = searchResult.HtmlUrl
            };
        }

        private static void ValidatePageRange(int firstPage, int lastPage)
        {
            if (firstPage < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(firstPage), "Page number must be at least 1.");
            }

            if (lastPage < firstPage)
            {
                throw new ArgumentOutOfRangeException(nameof(lastPage), "Last page must be >= first page.");
            }
        }

        public async Task<List<Model.Commit>> GetCommitsForFileAsync(string owner, string repoName, string path)
        {
            ValidateRepositoryParameters(owner, repoName, path);

            var commitRequest = new CommitRequest { Path = path };
            var commitsForFile = await _clientService.Client.Repository.Commit
                .GetAll(owner, repoName, commitRequest)
                .ConfigureAwait(false);

            return commitsForFile
                .Select((commit, index) => MapToCommit(commit, owner, repoName, path, commitsForFile.Count - index))
                .ToList();
        }

        private static Model.Commit MapToCommit(
            GitHubCommit gitHubCommit,
            string owner,
            string repoName,
            string path,
            int order)
        {
            return new Model.Commit(
                owner,
                repoName,
                path,
                gitHubCommit.Sha,
                gitHubCommit.Commit.Committer.Date,
                order
            );
        }

        public async Task<RepositoryContent> GetFileAtCommitAsync(
            string owner,
            string repoName,
            string path,
            string sha)
        {
            ValidateRepositoryParameters(owner, repoName, path);

            if (string.IsNullOrWhiteSpace(sha))
            {
                throw new ArgumentException("Commit SHA is required.", nameof(sha));
            }

            var contents = await _clientService.Client.Repository.Content
                .GetAllContentsByRef(owner, repoName, path, sha)
                .ConfigureAwait(false);

            return contents.FirstOrDefault();
        }

        private static void ValidateRepositoryParameters(string owner, string repoName, string path)
        {
            if (string.IsNullOrWhiteSpace(owner))
            {
                throw new ArgumentException("Owner is required.", nameof(owner));
            }

            if (string.IsNullOrWhiteSpace(repoName))
            {
                throw new ArgumentException("Repository name is required.", nameof(repoName));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("File path is required.", nameof(path));
            }
        }

    }
}