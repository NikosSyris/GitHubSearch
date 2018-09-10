using Octokit;
using System;
using System.Diagnostics;

namespace GitHub_Tool.Model
{
    public static class GlobalVariables
    {
        public static String accessToken;
        public static GitHubClient client;

        public static GitHubClient createGithubClient()
        {

            var client = new GitHubClient(new ProductHeaderValue("GitHub-Tool"));
            var tokenAuth = new Credentials(accessToken);
            client.Credentials = tokenAuth;

            return client;
        }

        public static void printAPILimitInfo()   
        {

            // Prior to first API call, this will be null, because it only deals with the last call.
            var apiInfo = GlobalVariables.client.GetLastApiInfo();

            // If the ApiInfo isn't null, there will be a property called RateLimit
            var rateLimit = apiInfo?.RateLimit;

            var howManyRequestsCanIMakePerHour = rateLimit?.Limit;
            var howManyRequestsDoIHaveLeft = rateLimit?.Remaining;
            var whenDoesTheLimitReset = rateLimit?.Reset; // UTC time

            Debug.WriteLine(howManyRequestsCanIMakePerHour);
            Debug.WriteLine(howManyRequestsDoIHaveLeft);
            Debug.WriteLine(whenDoesTheLimitReset);
        }

    }



}
