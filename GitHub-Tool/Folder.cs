using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub_Tool
{
    public class Folder
    {
        public string Name { get; set; }
        //public string HtmlUrl { get; set; }
        public List<File> FileList { get; set; }
        public List<Folder> FolderList { get; set; }

        public Folder(string name) //, string htmlUrl
        {
            Name = name;
            FileList = new List<File>();
            FolderList = new List<Folder>();
            //HtmlUrl = htmlUrl;

        }
    }
}
