using System;
using System.ComponentModel;

namespace GitHubSearch.Model
{
    public class Commit
    {

        public string Owner { get; }
        public string RepoName { get; }
        public string FilePath { get; }
        public string Sha { get; }
        public DateTimeOffset CreatedAt { get; }
        public int Order { get; }
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




