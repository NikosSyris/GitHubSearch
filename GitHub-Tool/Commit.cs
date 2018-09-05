using System;
using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Octokit.Internal;
using System.ComponentModel;

namespace GitHub_Tool
{
    public class Commit
    {

        public string Owner { get; set; }
        public string RepoName { get; set; }
        public string FilePath { get; set; }
        public string Sha { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int Size { get; set; }
        private bool _IsSelected = false;
        public bool IsSelected { get { return _IsSelected; } set { _IsSelected = value; OnChanged("IsSelected"); } }

        public Commit(string owner, string repoName, string filePath, string sha, DateTimeOffset createdAt)  //, int size
        {
            Owner = owner;
            RepoName = repoName;
            FilePath = filePath;
            Sha = sha;
            CreatedAt = createdAt;
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




