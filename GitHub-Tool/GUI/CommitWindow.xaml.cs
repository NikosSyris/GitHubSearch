using System.Windows;
using System.Windows.Controls;
using GitHubSearch.Model;
using GitHubSearch.Action;
using System.Collections.Generic;

namespace GitHubSearch.GUI
{

    public partial class CommitWindow : Window
    {

        DownloadLocallyManager download;

        public CommitWindow()
        {
            InitializeComponent();
            download = new DownloadLocallyManager();
            downloadDestinationTextBox.Text = download.DefaultDownloadDestination;
        }


        private void checkAllCommits(object sender, RoutedEventArgs e)
        {
            checkOrUncheck(true);
        }

        private void uncheckAllCommits(object sender, RoutedEventArgs e)
        {
            checkOrUncheck(false);
        }


        private void checkOrUncheck(bool value)
        {
            IEnumerable<Commit> commitList = commitsDataGrid.ItemsSource as IEnumerable<Model.Commit>;
            if (commitList != null)
            {
                foreach (var commit in commitList)
                {
                    commit.IsSelected = value;
                }

            }
            commitsDataGrid.Items.Refresh();
        }



        private void downloadButtonClick(object sender, RoutedEventArgs e)    
        {
            downloadTextBlock.Visibility = Visibility.Hidden;
            downloadButton.IsEnabled = false;

            if (download.directoryExists(downloadDestinationTextBox.Text))
            {
                download.downloadFIleContent(commitsDataGrid.ItemsSource, downloadDestinationTextBox.Text);
                downloadTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("The directory doesn't exist or you don't have the right to access it",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
            downloadButton.IsEnabled = true;
        }


        private void enableDataGridCopying(object sender, DataGridRowClipboardEventArgs e)
        {
            var currentCell = e.ClipboardRowContent[commitsDataGrid.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);
        }
    }
}
