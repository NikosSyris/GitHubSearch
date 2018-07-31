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
        //.ConfigureAwait(false)



        public async Task<IList<FileInformation>> SearchCode(String term, int minNumberOfCommits) 
        {

            var client = MainWindow.createGithubClient();


            IList<FileInformation> files = new List<FileInformation>();


            var codeRequest = new SearchCodeRequest(term)
            {
                Extension = "cs",
            };

            var result = await client.Search.SearchCode(codeRequest).ConfigureAwait(false);


            var numberOfResults = result.TotalCount;
            codeRequest.Page = 0;

            while (true)
            {
                codeRequest.Page += 1;

                for (var j = 0; j < 100; j++)       // na valw .perPage instead of 100
                {
                    var temp = result.Items.ElementAt(j);
                    var filePath = temp.Path;

                    var repo = temp.Repository.Name;
                    var owner = temp.Repository.Owner.Login;

                    var commitRequest = new CommitRequest { Path = filePath };

                    var commitsForFile = await client.Repository.Commit.GetAll(owner, repo, commitRequest).ConfigureAwait(false);

                    if (commitsForFile.Count > minNumberOfCommits)    // TODO also put equals
                    {
                        files.Add(new FileInformation(owner, repo, filePath, commitsForFile));
                    }
                    numberOfResults--;

                    if (numberOfResults == 0)
                    {
                        break;
                    }

                }

                if (numberOfResults == 0)
                {
                    break;
                }
            }

            IList<FileInformation> Sortedfiles = files.OrderByDescending(x => x.AllCommits.Count).ToList();


            return Sortedfiles;

        }

    }
}
