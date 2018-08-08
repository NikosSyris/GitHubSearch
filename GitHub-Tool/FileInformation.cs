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
    class FileInformation
    {
        public string Owner { get; set; }
        public string Repo { get; set; }
        public string FilePath { get; set; }
        public IReadOnlyList<GitHubCommit> AllCommits { get; set; }
        private bool _IsSelected = false;
        public bool IsSelected { get { return _IsSelected; } set { _IsSelected = value; OnChanged("IsSelected"); } }

        public FileInformation(string owner, string repo, string filePath, IReadOnlyList<GitHubCommit> allCommits)
        {
            Owner = owner;
            Repo = repo;
            FilePath = filePath;
            AllCommits = allCommits;
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




