using Octokit;
using System;
using System.IO;
using Model =  GitHub_Tool.Model;

namespace GitHub_Tool.Action
{
    class Download
    {

        public string DefaultDownloadDestination { get; } = @"c:\";
        CodeSearch codeSearch;

        public async void downloadContent(Model.Commit commit, string downloadDestination)
        {
            codeSearch = new CodeSearch();

            try
            {
                var file = await codeSearch.getSpecificVersion(commit.Owner, commit.RepoName, commit.FilePath, commit.Sha);
                string path = createFolder(commit.Owner, commit.RepoName, file.Name, commit.Order.ToString(),
                              commit.CreatedAt, downloadDestination);

                using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(path))
                {
                    streamWriter.WriteLine(file.HtmlUrl + "\r\n" + commit.Sha + " " + commit.CreatedAt + "\r\n");
                    streamWriter.Write(file.Content);

                }
            }
            catch (NotFoundException)
            {
                return;
            }
        }

        public bool checkIfDirectoryExists(string path)
        {
            if (Directory.Exists(path))
            {
                return true;
            }
            return false;
        }


        public string createFolder(string owner, string repoName, string fileName, string commitName,
                                   DateTimeOffset createdAt,string downloadDestination)
        {

            string pathString = System.IO.Path.Combine(downloadDestination, repoName + " by " + owner);
            pathString = System.IO.Path.Combine(pathString, fileName);
            System.IO.Directory.CreateDirectory(pathString);

            pathString = System.IO.Path.Combine(pathString, "Commit No" +" " + commitName + "   " 
                                                + createdAt.DateTime.ToString("dd-M-yyyy"));

            return pathString;
        }
    }
}
