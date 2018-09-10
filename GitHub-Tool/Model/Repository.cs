using System;
using System.Collections.Generic;

namespace GitHub_Tool.Model
{
    class Repository
    {
        public string Name { get; set; }
        public string Owner { get; set; }
        public string HtmlUrl { get; set; }
        public string Description { get; set; }
        public int StargazersCount { get; set; }
        public long Size { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }       
        public List<File> Files { get; set; }   
        public Folder RootFolder { get; set; }   

        public Repository(string name, string owner, long size, DateTimeOffset createdAt, DateTimeOffset updatedAt, int stargazersCount, string htmlUrl, string description) 
        {
            Name = name;
            Owner = owner;
            Size = size;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            StargazersCount = stargazersCount;
            HtmlUrl = htmlUrl;
            Description = description;
            Files = new List<File>();
        }
    }
}
