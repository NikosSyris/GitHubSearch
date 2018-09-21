using Octokit;

namespace GitHub_Tool.Model
{
    class SearchCodeRequestParameters
    {
        public string Term { get; }
        public string Extension { get; }
        public string Owner { get; }
        public string ForksIncluded { get; }
        public string FileName { get; }
        public string Path { get; }
        public string SizeChoice { get; }
        public int Size { get; }
        public string Language { get; }
        public bool? PathIncluded { get; }
        


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
