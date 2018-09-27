using Octokit;

namespace GitHubSearch.Model
{
    public static class GlobalVariables
    {

        public static GitHubClient client;
        public const int MaximumNumberOfPages = 10;

        public static GitHubClient createGithubClient(string accessToken)
        {
            var client = new GitHubClient(new ProductHeaderValue("GitHub-Tool"));
            var tokenAuth = new Credentials(accessToken);
            client.Credentials = tokenAuth;

            return client;
        }
    }
}
