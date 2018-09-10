using System.Windows;
using System.Windows.Controls;
using GitHub_Tool.Model;
using GitHub_Tool.Action;

namespace GitHub_Tool.GUI
{

    public partial class CommitWindow : Window
    {

        Download download;

        public CommitWindow()
        {
            InitializeComponent();
            download = new Download();
        }


        private void checkAllCommits(object sender, RoutedEventArgs e)     //doesn't work for some reason
        {
            foreach (Commit commit in CommitsDataGrid.ItemsSource)
            {
                commit.IsSelected = true;
            }
        }

        private void uncheckAllCommits(object sender, RoutedEventArgs e)    //doesn't work for some reason
        {
            foreach (Commit commit in CommitsDataGrid.ItemsSource)
            {
                commit.IsSelected = false;
            }
        }



        private void downloadButtonClick(object sender, RoutedEventArgs e)    
        {
          
            foreach (Commit commit in CommitsDataGrid.ItemsSource)
            {
                if (commit.IsSelected == true)
                {
                    download.downloadContent(commit); 
                }
            }
        }

        private void enableDataGridCopying(object sender, DataGridRowClipboardEventArgs e)
        {

            var currentCell = e.ClipboardRowContent[CommitsDataGrid.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);

        }
    }
}
