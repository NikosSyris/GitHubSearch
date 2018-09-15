using System;
using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitHub_Tool.Model;
using Model = GitHub_Tool.Model;
using System.Diagnostics;

namespace GitHub_Tool.Action
{
    class CodeSearch
    {

        public async Task<List<File>> searchCode(Model.SearchCodeRequestParameters requestParameters) 
        {

            List<File> files = new List<File>();       
            var codeRequest = getSearchCodeRequest(requestParameters);         
            var numberOfPages = 3;

            for (int i = 1; i <= numberOfPages; i++)
            {
                codeRequest.Page = i;
                var result = await GlobalVariables.client.Search.SearchCode(codeRequest).ConfigureAwait(false);

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

            codeRequest.FileName = parameters.FileName;
            codeRequest.Path = parameters.Path;
            codeRequest.User = parameters.Owner;
            codeRequest.Extension = parameters.Extension;
            codeRequest.Language = language;
            codeRequest.Size = pickRange(parameters.SizeChoice, parameters.Size);
            codeRequest.Forks = getBoolParameter(parameters.ForksIncluded);
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
