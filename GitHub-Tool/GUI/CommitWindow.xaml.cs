using System.Windows;
using System.Windows.Controls;
using GitHub_Tool.Model;
using GitHub_Tool.Action;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace GitHub_Tool.GUI
{

    public partial class CommitWindow : Window
    {

        Download download;

        public CommitWindow()
        {
            InitializeComponent();
            download = new Download();
            downloadDestinationTextBox.Text = download.DefaultDownloadDestination;
        }


        private void checkAllCommits(object sender, RoutedEventArgs e)     //doesn't work for some reason
        {
            checkOrUncheck(true);
        }

        private void uncheckAllCommits(object sender, RoutedEventArgs e)    //doesn't work for some reason
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

            if (download.checkIfDirectoryExists(downloadDestinationTextBox.Text))
            {
                foreach (Commit commit in commitsDataGrid.ItemsSource)
                {
                    if (commit.IsSelected == true)
                    {
                        download.downloadContent(commit, downloadDestinationTextBox.Text);
                    }
                }
                
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
