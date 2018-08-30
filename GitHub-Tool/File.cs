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
        public string Content { get; set; }
        //public string HtmlUrl { get; set; }

        public File(string name, string content) //, string htmlUrl
        {
            Name = name;
            Content = content;
            //HtmlUrl = htmlUrl;

        }
    }
}

