﻿using System;
using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Octokit.Internal;

namespace GitHub_Tool { 

    class Commit
    {

        public async void getAllCommitsThatChangedAFile()  //SearchCode file
        {

            var client = GithubApi.createGithubClient();




            string username = "octokit";
            string repo = "octokit.net";



            //Only commits containing this file path will be returned.
            var request = new CommitRequest { Path = "Octokit.Reactive/Clients/IObservableIssueCommentsClient.cs" };

            //for some reason the i think the request does not contain the actual commits. They are returned by the getAll.
            var commitsForFile = await client.Repository.Commit.GetAll(username, repo, request);


            Console.WriteLine(commitsForFile.Count);

            foreach (GitHubCommit commit in commitsForFile)
            {
                Console.WriteLine(commit.Sha);          //returns sha        60e9733c91f8923ada9c04d8a7acd3b66ad515c8
                Console.WriteLine(commit.Author.Url);       // returns url of the author
                Console.WriteLine(commit.Committer.Login);    // returns the name of the committer
            }
        }



        public async void getAllCommits()
        {

            var client = GithubApi.createGithubClient();

            string username = "octokit";
            string repo = "octokit.net";


            var repository = await client.Repository.Get(username, repo);
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

            var client = GithubApi.createGithubClient();

            string username = "octokit";
            string repo = "octokit.net";



            var files = await client.Git.Tree.GetRecursive(username, repo, "master");
            var filePaths = files.Tree.Select(x => x.Path);
            var stats = new Dictionary<string, int>();
            foreach (var path in filePaths)
            {
                //Only commits containing this file path will be returned.
                var request = new CommitRequest { Path = path };

                // for some reason  i think the request does not contain the actual commits. They are returned by the getAll.
                var commitsForFile = await client.Repository.Commit.GetAll(username, repo, request);

                stats.Add(path, commitsForFile.Count);
                Console.WriteLine(path);
                Console.WriteLine(commitsForFile.Count);

            }
        }



    }
}