using System;
using Model =  GitHub_Tool.Model;

namespace GitHub_Tool.Action
{
    class Download
    {
        CodeSearch codeSearch;

        public async void downloadContent(Model.Commit commit)
        {
            codeSearch = new CodeSearch();

            var file = await codeSearch.getSpecificVersion(commit.Owner, commit.RepoName, commit.FilePath, commit.Sha).ConfigureAwait(false);              
            String path =  createFolder(commit.Owner, commit.RepoName, file.Name, commit.Order.ToString(), commit.CreatedAt);

            using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(path))
            {
                streamWriter.WriteLine(file.HtmlUrl + "\r\n" + commit.Sha + " " + commit.CreatedAt + "\r\n");
                streamWriter.Write(file.Content);

            }
        }


        public String createFolder(string owner, string repoName, string fileName, string commitName, DateTimeOffset createdAt)
        {

            string pathString = @"c:\GitHub-Tool";

            pathString = System.IO.Path.Combine(pathString, repoName + " by " + owner);
            pathString = System.IO.Path.Combine(pathString, fileName);
            System.IO.Directory.CreateDirectory(pathString);

            pathString = System.IO.Path.Combine(pathString, "Commit No" +" " + commitName + "   " + createdAt.DateTime.ToString("dd-M-yyyy"));

            return pathString;
        }
    }
}
