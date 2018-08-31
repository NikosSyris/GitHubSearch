using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub_Tool
{
    public class File
    {
        public string Name { get; set; }
        public string Owner { get; set; }
        public string RepoName { get; set; }
        public string Path { get; set; }
        //public string HtmlUrl { get; set; }

        public File(string name, string path, string owner, string repoName) //, string htmlUrl , string content
        {
            Name = name;
            Path = path;
            Owner = owner;
            RepoName = repoName;
            //HtmlUrl = htmlUrl;

        }
    }
}

