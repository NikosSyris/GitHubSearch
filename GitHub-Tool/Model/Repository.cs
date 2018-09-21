using System;
using System.Collections.Generic;

namespace GitHub_Tool.Model
{
    public class Repository
    {
        public string Name { get; set; }
        public string Owner { get; set; }
        public string HtmlUrl { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int StargazersCount { get; set; }
        public int ForksCount { get; set; }
        public long Size { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }       
        public List<File> Files { get; set; }   
        public Folder RootFolder { get; set; }


        public Repository()
        {
            Files = new List<File>();
        }
    }
}
