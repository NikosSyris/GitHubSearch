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




        public async Task<SearchCode> SearchCode(String term) // and return the first result 
        {

            var client = GithubApi.createGithubClient();


            // 100 results per page as default
            //request.PerPage = 30;

            // set this when you want to fetch subsequent pages
            //request.Page = 2;
            

            int resultPicked = 0;


            var request = new SearchCodeRequest(term, "NikosSyris", "GitHub-Tool")
            {

                // we may want to restrict the file based on file extension
                Extension = "cs",

            };


            var result = await client.Search.SearchCode(request);

            Console.WriteLine(result.TotalCount);
            //Console.WriteLine(result.Items.ElementAt(resultPicked).HtmlUrl);
            // Console.WriteLine(result.Items.ElementAt(resultPicked).Path);
            // Console.WriteLine(result.Items.ElementAt(resultPicked).Url);

            var repo = result.Items.ElementAt(resultPicked).Repository;
            var owner = repo.Owner;
            var fileName = result.Items.ElementAt(resultPicked).Name;

            return result.Items.ElementAt(resultPicked);

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
