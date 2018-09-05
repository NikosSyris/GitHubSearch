using System.Windows;
using System.Windows.Controls;

namespace GitHub_Tool
{

    public partial class CommitWindow : Window
    {
        public CommitWindow()
        {
            InitializeComponent();
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

            Download download = new Download();

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
