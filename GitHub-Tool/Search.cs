using System;
using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GitHub_Tool
{
    class Search
    {

        public async Task<List<File>> searchCode(String term, int minNumberOfCommits, String extension, String name, int size) 
        {

            List<File> files = new List<File>();

            var codeRequest = getSearchCodeRequest(term, extension, name,size);  
            var result = await GlobalVariables.client.Search.SearchCode(codeRequest).ConfigureAwait(false);
            var numberOfResults = result.TotalCount;
            var parsedSoFar = 0;

            while (parsedSoFar != numberOfResults)
            {
                codeRequest.Page += 1;

                for (var i = 0; i < codeRequest.PerPage; i++)       
                {
                    var tempFile = result.Items.ElementAt(i);
                    var owner = tempFile.Repository.Owner.Login;
                    var repoName = tempFile.Repository.Name;
                    var path = tempFile.Path;

                    List<Commit> commitList = await getCommitsForFIle(owner, repoName, path).ConfigureAwait(false);

                    if (commitList.Count >= minNumberOfCommits)    
                    {
                        files.Add(new File(tempFile.Name, owner, repoName, path, commitList));
                    }
                    parsedSoFar++;

                    if (parsedSoFar == numberOfResults)
                    {
                        break;
                    }
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

            codeRequest.Page = 0;

            return codeRequest;
        }


        public async Task<List<Commit>> getCommitsForFIle(string owner, string repoName, string path)
        {

            List<Commit> commits = new List<Commit>();
            var commitRequest = new CommitRequest { Path = path };
            var commitsForFile = await GlobalVariables.client.Repository.Commit.GetAll(owner, repoName, commitRequest).ConfigureAwait(false);

            foreach (var commit in commitsForFile)
            {
                commits.Add(new Commit(owner, repoName, path, commit.Sha, commit.Commit.Committer.Date));
            }

            return commits;
        }


        public async Task<RepositoryContent> getLatestVersion(string owner, string repoName, string path)
        {
            var file = await GlobalVariables.client
                    .Repository
                    .Content
                    .GetAllContents(owner, repoName, path)
                    .ConfigureAwait(false);

            return file[0];
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


        public async Task<List<Repository>> searchRepos(String owner, String name)
        {


            List<Repository> repos = new List<Repository>();

            var repoRequest = new SearchRepositoriesRequest();
            repoRequest.Size = Range.GreaterThan(1);
            repoRequest.User = owner;

            var result1 = await GlobalVariables.client.Search.SearchRepo(repoRequest);

            foreach (var rep in result1.Items)
            {
                repos.Add(new Repository(rep.Name, rep.Owner.Login, rep.Size));
            }

            return repos;
        }


        public async Task<Repository> getRepoStructure(Repository repository)
        {
            Folder rootFolder = new Folder("root");
            var result = await GlobalVariables.client.Repository.Content.GetAllContents(repository.Owner, repository.Name).ConfigureAwait(false);

            foreach (var item in result)
            {
                if (item.Type == "dir")
                {
                    Folder newFolder = new Folder(item.Name);
                    rootFolder.FolderList.Add(newFolder);
                    newFolder = await getFolderContent(repository, newFolder).ConfigureAwait(false);
                }
                else if (item.Type == "file")
                {
                    rootFolder.FileList.Add(new File(item.Name, item.Path, repository.Owner, repository.Name));
                    repository.Files.Add(new File(item.Name, repository.Owner, repository.Name, item.Path));
                }
            }

            repository.RootFolder = rootFolder;

            return repository;
        }



        private async Task<Folder> getFolderContent(Repository repository, Folder folder)
        {

            var result = await GlobalVariables.client.Repository.Content.GetAllContents(repository.Owner, repository.Name, folder.Name).ConfigureAwait(false);
            
            foreach (var item in result)
            {
                if (item.Type == "file")
                {
                    folder.FileList.Add(new File(item.Name, item.Path, repository.Owner, repository.Name));
                    repository.Files.Add(new File(item.Name, repository.Owner, repository.Name, item.Path));
                }
                else if (item.Type == "dir")
                {
                    var newFolderName = folder.Name + "/" + item.Name;
                    Folder newFolder = new Folder(newFolderName);
                    folder.FolderList.Add(newFolder);
                    newFolder = await getFolderContent(repository, newFolder).ConfigureAwait(false);
                }
            }

            return folder;
        }








    }
}
