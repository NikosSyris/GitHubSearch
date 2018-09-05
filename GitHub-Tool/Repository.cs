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
        public List<File> Files { get; set; }   
        public Folder RootFolder { get; set; }

        public Repository(string name, string owner, long size) //, string htmlUrl
        {
            Name = name;
            Owner = owner;
            Size = size;
            Files = new List<File>();
        }
    }
}
