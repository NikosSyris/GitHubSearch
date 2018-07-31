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

    class Commit
    {

        public  String getAllCommitsThatChangedAFile(IList<FileInformation> files)
        {

            var client = MainWindow.createGithubClient();

            var s = " ";


            foreach (var file in files)
            {
                //Console.WriteLine(file.Owner + "  " + file.Repo + "  " + file.FilePath + "  " + file.AllCommits.Count);
                s += file.Owner + "  " + file.Repo + "  " + file.FilePath + "  " + file.AllCommits.Count + "\r\n";
            }

            return s;
        }



        public async void getAllCommits()
        {

            var client = MainWindow.createGithubClient();

            string owner = "octokit";
            string repo = "octokit.net";


            var repository = await client.Repository.Get(owner, repo);
            var commits = await client.Repository.Commit.GetAll(repository.Id);


            foreach (GitHubCommit commit in commits)
            {

                Console.WriteLine(commit.Sha);

                //var commitDetails = client.Repository.Commit.Get(commit.Sha);
                //var files = commitDetails.Files;
            }

        }




        // Getting the number of commits for every file 
        public async void downloadCommits()
        {

            var client = MainWindow.createGithubClient();

            string owner = "octokit";
            string repo = "octokit.net";



            var files = await client.Git.Tree.GetRecursive(owner, repo, "master");
            var filePaths = files.Tree.Select(x => x.Path);
            var stats = new Dictionary<string, int>();
            foreach (var path in filePaths)
            {
                //Only commits containing this file path will be returned.
                var request = new CommitRequest { Path = path };

                // for some reason  i think the request does not contain the actual commits. They are returned by the getAll.
                var commitsForFile = await client.Repository.Commit.GetAll(owner, repo, request);

                stats.Add(path, commitsForFile.Count);
                Console.WriteLine(path);
                Console.WriteLine(commitsForFile.Count);

            }
        }



    }
}