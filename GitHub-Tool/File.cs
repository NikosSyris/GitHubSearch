using Octokit;
using System.Collections.Generic;

namespace GitHub_Tool
{
    public class File
    {
        public string Name { get; set; }
        public string Owner { get; set; }
        public string RepoName { get; set; }
        public string Path { get; set; }
        public List<Commit> AllCommits { get; set; }
        //public string HtmlUrl { get; set; }

        public File(string name, string owner, string repoName, string path, List<Commit> allCommits = null) //, string htmlUrl , string content
        {
            Name = name;
            Path = path;
            Owner = owner;
            RepoName = repoName;
            AllCommits = allCommits;
            //HtmlUrl = htmlUrl;
        }
    }
}

