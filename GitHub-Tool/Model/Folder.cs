using System.Collections.Generic;


namespace GitHub_Tool.Model
{
    public class Folder
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public List<File> FileList { get; set; }
        public List<Folder> FolderList { get; set; }

        public Folder(string name, string path) 
        {
            Name = name;
            Path = path;
            FileList = new List<File>();
            FolderList = new List<Folder>();
        }
    }
}
