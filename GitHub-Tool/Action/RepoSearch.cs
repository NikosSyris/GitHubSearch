using System;
using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitHub_Tool.Model;
using Model = GitHub_Tool.Model;


namespace GitHub_Tool.Action
{
    class RepoSearch
    {
        public async Task<List<Model.Repository>> searchRepos(string owner, int stars, int size)
        {

            List<Model.Repository> repos = new List<Model.Repository>();

            var repoRequest = new SearchRepositoriesRequest(); 
            repoRequest.User = owner;
            repoRequest.Stars = Range.GreaterThanOrEquals(stars); 
            repoRequest.Size = Range.GreaterThan(size);
            //repoRequest.Created

            var result = await GlobalVariables.client.Search.SearchRepo(repoRequest);

            foreach (var repo in result.Items)
            {
                repos.Add(new Model.Repository(repo.Name, repo.Owner.Login, repo.Size, repo.CreatedAt, repo.UpdatedAt, repo.StargazersCount, repo.HtmlUrl, repo.Description));
            }

            return repos;
        }
    }
}
