using System.Collections.Generic;

namespace GitHubSearch.Model
{
    public class File
    {
        public string Name { get; set; }
        public string Owner { get; set; }
        public string RepoName { get; set; }
        public string Path { get; set; }
        public string HtmlUrl { get; set; }
        public List<Commit> AllCommits { get; set; }

        public File() { }

        public File(string name, string owner, string repoName, string path, string htmlUrl, List<Commit> allCommits = null) 
        {
            Name = name;
            Path = path;
            Owner = owner;
            RepoName = repoName;
            HtmlUrl = htmlUrl;
            AllCommits = allCommits;           
        }
    }
}

