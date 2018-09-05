using System;
using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Octokit.Internal;

namespace GitHub_Tool
{
    class Download
    {
        Search search;

        public async void downloadContent(Commit commit)
        {
            search = new Search();

            var file = await search.getSpecificVersion(commit.Owner, commit.RepoName, commit.FilePath, commit.Sha).ConfigureAwait(false);              
            String path =  createFolder(commit.Owner, commit.RepoName, file.Name, commit.Sha);

            using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(path))
            {
                streamWriter.WriteLine(file.HtmlUrl + "     " + DateTime.Today.ToString("dd-MM-yyyy") + " " + DateTime.Now.ToString("h:mm:ss tt") + "\r\n");
                streamWriter.Write(file.Content);

            }
        }


        public String createFolder(String owner, String repoName, String fileName, String commitName)
        {

            string pathString = @"c:\GitHub-Tool";

            pathString = System.IO.Path.Combine(pathString, repoName + " by " + owner);
            pathString = System.IO.Path.Combine(pathString, fileName);
            System.IO.Directory.CreateDirectory(pathString);

            pathString = System.IO.Path.Combine(pathString, commitName);

            return pathString;
        }
    }
}
