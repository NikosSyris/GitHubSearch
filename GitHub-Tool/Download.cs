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
        public async void downloadContent(FileInformation fileInformation)
        {

            var client = GithubApi.createGithubClient();

            var owner = fileInformation.Owner;
            var repo =  fileInformation.Repo;
            var filePath = fileInformation.FilePath;
            //    Console.WriteLine(owner);
            //     Console.WriteLine(repo);
            //    Console.WriteLine(filePath);


            //var file = await client
            //    .Repository
            //   .Content
            //   .GetAllContents(owner, repo, filePath);





            for (var i = 0; i < fileInformation.AllCommits.Count; i++)
            {

                var file = await client
                    .Repository
                    .Content
                    .GetAllContentsByRef(owner, repo, filePath, fileInformation.AllCommits[i].Sha);

                Console.WriteLine(fileInformation.AllCommits[i].Sha);



                //Console.WriteLine(file.Count);  //= 1


                //Console.WriteLine(file.Count);

                foreach (var element in file)
                {
                    //Console.WriteLine(element.HtmlUrl);
                    //Console.WriteLine(element.Content);
                    createFolder(element.Name, fileInformation.AllCommits[i].Sha, element.HtmlUrl, element.Content);  

                }
            }

            GithubApi.printAPILimitInfo();
            Console.WriteLine("done");
        }



        public void createFolder(String folderName, String fileName, String path, String content)
        {

            string pathString = @"c:\GitHub-Tool";

            pathString = System.IO.Path.Combine(pathString, folderName);
            System.IO.Directory.CreateDirectory(pathString);

            pathString = System.IO.Path.Combine(pathString, fileName);
            //Console.WriteLine("Path to my file: {0}\n", pathString);


            using (System.IO.StreamWriter streamWriter =  new System.IO.StreamWriter(pathString))
            {
                streamWriter.WriteLine(path + "     " + DateTime.Today.ToString("dd-MM-yyyy")  + " " +  DateTime.Now.ToString("h:mm:ss tt") + "\r\n");
                streamWriter.Write(content);

            }

        }


    }
}
