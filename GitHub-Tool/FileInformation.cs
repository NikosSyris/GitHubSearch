using Octokit;
using System.Collections.Generic;

namespace GitHub_Tool
{
    class FileInformation
    {
        public string Owner { get; set; }
        public string Repo { get; set; }
        public string FilePath { get; set; }
        public IReadOnlyList<GitHubCommit> AllCommits { get; set; }

        public FileInformation(string owner, string repo, string filePath, IReadOnlyList<GitHubCommit> allCommits)
        {
            Owner = owner;
            Repo = repo;
            FilePath = filePath;
            AllCommits = allCommits;
        }
    }
}




