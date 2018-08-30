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
    class CommitTemp
    {

        public string Owner { get; set; }
        public string Repo { get; set; }
        public string FilePath { get; set; }
        public string Sha { get; set; }
        public int Size { get; set; }
        private bool _IsSelected = false;
        public bool IsSelected { get { return _IsSelected; } set { _IsSelected = value; OnChanged("IsSelected"); } }

        public CommitTemp(string owner, string repo, string filePath, string sha)  //, int size
        {
            Owner = owner;
            Repo = repo;
            FilePath = filePath;
            Sha = sha;
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




