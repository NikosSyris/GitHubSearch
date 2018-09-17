using System;
using System.ComponentModel;

namespace GitHub_Tool.Model
{
    public class Commit
    {

        public string Owner { get; set; }
        public string RepoName { get; set; }
        public string FilePath { get; set; }
        public string Sha { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int Size { get; set; }
        public int Order { get; set; }
        private bool _IsSelected = false;
        public bool IsSelected { get { return _IsSelected; } set { _IsSelected = value; OnChanged("IsSelected"); } }

        public Commit(string owner, string repoName, string filePath, string sha, DateTimeOffset createdAt, int order)  
        {
            Owner = owner;
            RepoName = repoName;
            FilePath = filePath;
            Sha = sha;
            CreatedAt = createdAt;
            Order = order;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

    }
}




