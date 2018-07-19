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
    class Search
    {




        public async Task<SearchCode> SearchCode() // and return the first result 
        {

            var client = GithubApi.createGithubClient();

            var request = new SearchCodeRequest("SELECT")
            {

                // we may want to restrict the file based on file extension
                Extension = "sql",



            };

            var result = await client.Search.SearchCode(request);


            //Console.WriteLine(result.TotalCount);
            // Console.WriteLine(result.Items.ElementAt(0).HtmlUrl);
            // Console.WriteLine(result.Items.ElementAt(0).Path);
            // Console.WriteLine(result.Items.ElementAt(0).Url);

            var repo = result.Items.ElementAt(0).Repository;
            Console.WriteLine(repo.Name);
            Console.WriteLine(repo.FullName);

            var username = repo.Owner;
            Console.WriteLine(username.Url);

            var fileName = result.Items.ElementAt(0).Name;
            Console.WriteLine(fileName);


            return result.Items.ElementAt(0);

        }




        public async Task<Repository> SearchRepositories()
        {

            var client = GithubApi.createGithubClient();

            // Initialize a new instance of the SearchRepositoriesRequest class
            var request = new SearchRepositoriesRequest("javascript")   // language used
            {

                // lets find a library with over 1k stars
                Stars = Range.GreaterThan(1000),

                // sort by the number of stars
                SortField = RepoSearchSort.Stars,
            };


            var repos = await client.Search.SearchRepo(request);


            Console.WriteLine(repos.TotalCount);

            Console.WriteLine(repos.Items.ElementAt(0).FullName);
            Console.WriteLine(repos.Items.ElementAt(0).HtmlUrl);
            Console.WriteLine(repos.Items.ElementAt(0).Language);
            Console.WriteLine(repos.Items.ElementAt(0).Size);
            Console.WriteLine(repos.Items.ElementAt(0).ForksCount);

            //  foreach (var element in repos.Items) {

            //      Console.WriteLine(element);
            //}


            return repos.Items.ElementAt(0);


        }






        //  Ctrl + k + c to comment ctrl+k+u  to undo
        public async void searchUsers()   //and find all their repos
        {

            var client = GithubApi.createGithubClient();

            var request = new SearchRepositoriesRequest()
            {


                User = "sfikas"

            };


            var repos = await client.Search.SearchRepo(request);

            var numberOfRepos = repos.TotalCount;


            Console.WriteLine(numberOfRepos);           //excludes forks

            foreach (var element in repos.Items)
            {

                Console.WriteLine(element.FullName);
                Console.WriteLine(element.HtmlUrl);
                Console.WriteLine(element.Language);
                Console.WriteLine(element.Size);
                Console.WriteLine(element.ForksCount);
            }




        }








    }
}
