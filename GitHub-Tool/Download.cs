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



        // download the contents of a file form whichever commit or branch you want
        public async void downloadContent(CommitTemp fileInformation)
        {

            var client = MainWindow.createGithubClient();

            var owner = fileInformation.Owner;
            var repo = fileInformation.Repo;
            var filePath = fileInformation.FilePath;
            var sha = fileInformation.Sha;



            //var file = await client
            //    .Repository
            //   .Content
            //   .GetAllContents(owner, repo, filePath)
            //    .ConfigureAwait(false);

            var file = await client
                .Repository
                .Content
                .GetAllContentsByRef(owner, repo, filePath, sha)
                .ConfigureAwait(false);


            foreach (var element in file)
            {

                createFolder(element.Name, sha, element.HtmlUrl, element.Content);
                    
                    
            }
        }


        //TODO there's a bug when there's 2 files with the same name
        public void createFolder(String folderName, String fileName, String path, String content)
        {

            string pathString = @"c:\GitHub-Tool";

            pathString = System.IO.Path.Combine(pathString, folderName);
            System.IO.Directory.CreateDirectory(pathString);

            pathString = System.IO.Path.Combine(pathString, fileName);
            //Console.WriteLine("Path to my file: {0}\n", pathString);


            using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(pathString))
            {
                streamWriter.WriteLine(path + "     " + DateTime.Today.ToString("dd-MM-yyyy") + " " + DateTime.Now.ToString("h:mm:ss tt") + "\r\n");
                streamWriter.Write(content);

            }

        }


    }
}
