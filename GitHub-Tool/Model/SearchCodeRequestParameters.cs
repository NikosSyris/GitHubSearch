using Octokit;

namespace GitHub_Tool.Model
{
    class SearchCodeRequestParameters
    {
        public string Term { get; set; }
        public string Extension { get; set; }
        public string Owner { get; set; }
        public string ForksIncluded { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string SizeChoice { get; set; }
        public int Size { get; set; }
        public string Language { get; set; }
        public bool? PathIncluded { get; set; }
        


        public SearchCodeRequestParameters(string term, string extension, string owner, int size, string language,
                                           bool? pathIncluded, string forksIncluded, string fileName, string path,
                                           string sizeChoice)
        {
            Term = term;
            Extension = extension;
            Owner = owner;
            Size = size;
            Language =  language;
            PathIncluded = pathIncluded;
            ForksIncluded = forksIncluded;
            FileName = fileName;
            Path = path;
            SizeChoice = sizeChoice;
        }
    }
}
