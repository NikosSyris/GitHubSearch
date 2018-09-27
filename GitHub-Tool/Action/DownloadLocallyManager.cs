using Octokit;
using System;
using System.Collections;
using System.IO;
using System.Windows.Controls;
using Model = GitHubSearch.Model;

namespace GitHubSearch.Action
{
    class DownloadLocallyManager
    {

        public string DefaultDownloadDestination { get; } = @"c:\";
        public string DefaultName { get; } = "result";
        CodeSearchManager codeSearch;

        public async void downloadFIleContent(IEnumerable commits, string downloadDestination)
        {
            codeSearch = new CodeSearchManager();

            try
            {
                foreach (Model.Commit commit in commits)
                {
                    if (commit.IsSelected == true)
                    {
                        var file = await codeSearch.getSpecificVersion(commit.Owner, commit.RepoName, commit.FilePath, commit.Sha);
                        string path = createFolder(commit, file.Name, downloadDestination);

                        using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(path))
                        {
                            streamWriter.WriteLine(file.HtmlUrl + "\r\n" + commit.Sha + "\r\n");
                            streamWriter.Write(file.Content);
                        }
                    }
                }
            }
            catch (NotFoundException)
            {
                return;
            }
        }


        public void downloadAllSearchResults(Model.RequestParameters requestParameters, ItemCollection items,
                                                   string downloadDestination, string fileName)
        {
            if (!directoryExists(downloadDestination))
            {
                throw new DirectoryNotFoundException();
            }

            string pathString = downloadDestination;
            pathString = System.IO.Path.Combine(pathString, fileName);

            try
            {
                using (TextWriter textWriter = new StreamWriter(pathString))
                {
                    textWriter.WriteLine(requestParameters.ToString() + DateTime.Now + "\r\n");

                    foreach (var item in items)
                    {
                        if (item is Model.File)
                        {
                            var file = item as Model.File;
                            textWriter.WriteLine(file.Name + "\t" + file.Owner + "\t" + file.RepoName + "\t" + file.HtmlUrl);
                        }
                        else
                        {
                            var repo = item as Model.Repository;
                            textWriter.WriteLine(repo.Name + "\t" + repo.Owner + "\t" + repo.Size + "\t" + repo.Language + "\t"
                                                 + repo.CreatedAt + "\t" + repo.UpdatedAt + "\t" + repo.StargazersCount + "\t" 
                                                 + repo.ForksCount + "\t" + repo.HtmlUrl + "\t" + repo.Description);
                        }
                    }

                    textWriter.Close();
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException();
            }
        }


        public bool directoryExists(string path)
        {
            if (Directory.Exists(path))
            {
                return true;
            }
            return false;
        }


        private string createFolder(Model.Commit commit, string fileName, string downloadDestination)
        {

            string pathString = System.IO.Path.Combine(downloadDestination, commit.RepoName + " by " + commit.Owner);
            pathString = System.IO.Path.Combine(pathString, fileName);
            System.IO.Directory.CreateDirectory(pathString);

            pathString = System.IO.Path.Combine(pathString, commit.CreatedAt.DateTime.ToString("yyyy-MM-dd--HH-mm-ss") +
                                                            "-" + commit.Order.ToString());

            return pathString;
        }
    }
}
