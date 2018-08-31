using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub_Tool
{
    class Repository
    {
        public string Name { get; set; }
        public string Owner { get; set; }
        public long Size { get; set; }
        public List<File> RepositoryContentList { get; set; }   //ta files tou project
        public Folder RootFolder { get; set; }

        public Repository(string name, string owner, long size) //, string htmlUrl
        {
            Name = name;
            Owner = owner;
            Size = size;
            RepositoryContentList = new List<File>();
        }
    }
}
