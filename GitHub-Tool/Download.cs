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
            Console.WriteLine(owner);
            Console.WriteLine(repo);
            Console.WriteLine(filePath);


            var file = await client
                .Repository
                .Content
                .GetAllContents(owner, repo, "CodeHub/Services/SearchUtility.cs");


            //var file = await client
            //    .Repository
            //    .Content
            //    .GetAllContentsByRef(owner, repo, "docs/getting-started.md", "438a32639759b6af9a8fd2427099a718c858b3a6");



            Console.WriteLine(file.Count);  //= 1


            //Console.WriteLine(file.Count);

            foreach (var element in file)
            {
                Console.WriteLine(element.Content);
            }

            Console.WriteLine("done");
        }
    }
}
