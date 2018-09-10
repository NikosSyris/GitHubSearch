using System;
using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitHub_Tool.Model;
using Model = GitHub_Tool.Model;


namespace GitHub_Tool.Action
{
    class CodeSearch
    {

        public async Task<List<File>> searchCode(String term, int minNumberOfCommits, String extension, String name, int size) 
        {

            List<File> files = new List<File>();

            var codeRequest = getSearchCodeRequest(term, extension, name,size);  
            var result = await GlobalVariables.client.Search.SearchCode(codeRequest).ConfigureAwait(false);
            var numberOfResults = result.TotalCount;
            var parsedSoFar = 0;

            for (var i = 0; i < codeRequest.PerPage; i++)       
            {
                var tempFile = result.Items.ElementAt(i);
                var owner = tempFile.Repository.Owner.Login;
                var repoName = tempFile.Repository.Name;

                List<Model.Commit> commitList = await getCommitsForFIle(owner, repoName, tempFile.Path).ConfigureAwait(false);

                if (commitList.Count >= minNumberOfCommits)    
                {
                    files.Add(new File(tempFile.Name, owner, repoName, tempFile.Path, tempFile.HtmlUrl, commitList));
                }
                parsedSoFar++;

                if (parsedSoFar == numberOfResults)
                {
                    break;
                }
            }
            

            return files;
        }


        private SearchCodeRequest getSearchCodeRequest(String term, String extension = null, String name = null, int size = 0)
        {
            var codeRequest = new SearchCodeRequest(term);

            if (name != null)      
            {
                codeRequest.User = name;
            }

            if (extension != null)   
            {
                codeRequest.Extension = extension;
            }

            //if (size != 0)
            //{
            //    codeRequest.Size = Range.GreaterThanOrEquals(size);
            //}

            return codeRequest;
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
