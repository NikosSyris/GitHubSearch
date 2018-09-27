using System;
using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitHubSearch.Model;
using Model = GitHubSearch.Model;

namespace GitHubSearch.Action
{
    class CodeSearchManager
    {

        public async Task<List<File>> searchCode(Model.SearchCodeRequestParameters requestParameters, int firstPage, int lastPage) 
        {

            List<File> files = new List<File>();       
            SearchCodeRequest codeRequest = getSearchCodeRequest(requestParameters);     
            
            for (int i = firstPage; i <= lastPage; i++)
            {
                codeRequest.Page = i;
                SearchCodeResult result = await GlobalVariables.client.Search.SearchCode(codeRequest).ConfigureAwait(false);
                
                files.AddRange(result.Items.Select(file => new Model.File
                {
                    Name = file.Name,
                    Path = file.Path,
                    RepoName = file.Repository.Name,
                    Owner = file.Repository.Owner.Login,
                    HtmlUrl = file.HtmlUrl 
                    
                }).ToList());
            }

            return files;
        }


        public async Task<int> getNumberOfPages(Model.SearchCodeRequestParameters requestParameters)
        {
            SearchCodeRequest codeRequest = getSearchCodeRequest(requestParameters);
            SearchCodeResult result = await GlobalVariables.client.Search.SearchCode(codeRequest);

            if (result.TotalCount == 0)
            {
                return 0;
            }

            if (result.TotalCount > 1000)
            {
                return GlobalVariables.MaximumNumberOfPages;
            }

            if (result.TotalCount % codeRequest.PerPage == 0)
            {
                return result.TotalCount / codeRequest.PerPage;
            }

            return result.TotalCount / codeRequest.PerPage + 1;
        }


        private SearchCodeRequest getSearchCodeRequest(Model.SearchCodeRequestParameters parameters)
        {

            SearchCodeRequest codeRequest;

            try
            {
                codeRequest = new SearchCodeRequest(parameters.Term);
            }
            catch (Exception)
            {
                codeRequest = new SearchCodeRequest();
            }

            Language language = (Language)Enum.Parse(typeof(Language), parameters.Language);

            codeRequest.Language = language;
            codeRequest.Path = parameters.Path;
            codeRequest.User = parameters.Owner;
            codeRequest.FileName = parameters.FileName;
            codeRequest.Extension = parameters.Extension;
            codeRequest.Forks = getBoolParameter(parameters.ForksIncluded);
            codeRequest.Size = pickRange(parameters.SizeChoice, parameters.Size);
            codeRequest.In = getPathIncludedParameter(parameters.PathIncluded, parameters.Term);

            return codeRequest;
        }


        private bool getBoolParameter(string parameter)
        {
            if (parameter.Equals("Yes"))
            {
                return true;
            }

            return false;
        }

        private IEnumerable<CodeInQualifier> getPathIncludedParameter(bool? pathIncluded, string parameter)
        {
            if (pathIncluded == true && parameter != null)
            {
                return new[] { CodeInQualifier.File, CodeInQualifier.Path };
            }

            return new[] { CodeInQualifier.File };
        }


        private Range pickRange(string choice, int size)
        {

            if (choice.Equals("More than"))
            {
                return Range.GreaterThanOrEquals(size);
            }

            return Range.LessThanOrEquals(size);
        }


        public async Task<List<Model.Commit>> getCommitsForFIle(string owner, string repoName, string path)
        {

            List<Model.Commit> commits = new List<Model.Commit>();
            var commitRequest = new CommitRequest { Path = path };
            var commitsForFile = await GlobalVariables.client.Repository.Commit.GetAll(owner, repoName, commitRequest).ConfigureAwait(false);
            var numberOfCommits = commitsForFile.Count;

            foreach (var commit in commitsForFile)
            {
                commits.Add(new Model.Commit(owner, repoName, path, commit.Sha, commit.Commit.Committer.Date, numberOfCommits));
                numberOfCommits--;
            }

            return commits;
        }


        public async Task<RepositoryContent> getSpecificVersion(string owner, string repoName, string path, string sha)
        {
            var file = await GlobalVariables.client
                    .Repository
                    .Content
                    .GetAllContentsByRef(owner, repoName, path, sha)
                    .ConfigureAwait(false);

            return file[0];
        }
    }
}
