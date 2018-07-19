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

    public class GithubApi
    {


        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Search search = new Search();
            Commit commit = new Commit();

            search.SearchRepositories();
           // var file = search.SearchCode();  s auto eixa meinei
            //search.searchUsers();

            commit.getAllCommitsThatChangedAFile();
            commit.getAllCommits();
            commit.downloadCommits();



            //TryToDownload();


            //GetUserInfo();
            //APILImits();


            // var commitNumber = CountCommitsSimpleTest();
            // Console.WriteLine(commitNumber);

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }




        public static GitHubClient createGithubClient()
        {

            var client = new GitHubClient(new ProductHeaderValue("my-cool-app"));
            var tokenAuth = new Credentials("eb38e6af9ba503684c2fd7e000b09035d6f15c83");

            client.Credentials = tokenAuth;

            return client;

        }






















        // download the contents of a file form whichever commit or branch you want
        public async static void TryToDownload()
        {

            var client = GithubApi.createGithubClient();

            var docs = await client
                .Repository
                .Content
                .GetAllContents("octokit", "octokit.net", "docs/getting-started.md");


            //var docs = await client
            //    .Repository
            //    .Content
            //     .GetAllContentsByRef("octokit", "octokit.net", "docs/getting-started.md", "438a32639759b6af9a8fd2427099a718c858b3a6");



            Console.WriteLine(docs.Count);  //= 1


            //Console.WriteLine(docs.Count);

            foreach (var element in docs)
            {

                Console.WriteLine(element.ToString());
                Console.WriteLine(element.HtmlUrl);
                Console.WriteLine(element.Size);
                Console.WriteLine(element.Type);
                Console.WriteLine(element.Content);
            }



            Console.WriteLine("done");



        }


        public async static Task GetUserInfo()
        {

            var client = GithubApi.createGithubClient();

            var user = await client.User.Get("shiftkey");
            Console.WriteLine("{0} has {1} public repositories - go check out their profile at {2}",
            user.Name,
            user.PublicRepos,
            user.Url);


            var myProfile = await client.User.Current();
            Console.WriteLine("{0} has {1} public repositories - go check out their profile at {2}",
            myProfile.Name,
            myProfile.PublicRepos,
            myProfile.Url);

        }



        public async static Task<int> CountCommitsSimpleTest()
        {

            var github = new GitHubClient(new ProductHeaderValue("AkkaCounter"), new InMemoryCredentialStore
            (new Credentials("hidden_number")));

            var repos = await github.Repository.GetAllForUser("michal-franc");

            var countCommits = 0;

            foreach (var repository in repos)
            {
                var commits = await github.Repository.Commit.GetAll(repository.Owner.Login, repository.Name);
                countCommits = countCommits + commits.Count;
            }


            Console.WriteLine(countCommits);

            return countCommits;
        }
    }

}
