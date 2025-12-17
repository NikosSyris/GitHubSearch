using System;
using System.Windows;
using System.Windows.Controls;
using GitHubSearch.Model;
using GitHubSearch.Action;
using GitHubSearch.Services;
using System.Collections.Generic;

namespace GitHubSearch.GUI
{
    public partial class CommitWindow : Window
    {
        private DownloadLocallyManager _downloadManager;

        public CommitWindow(GitHubClientService clientService)
        {
            InitializeComponent();
            _downloadManager = new DownloadLocallyManager(clientService);
            downloadDestinationTextBox.Text = _downloadManager.DefaultDownloadDestination;
        }

        private void checkAllCommits(object sender, RoutedEventArgs e)
        {
            CheckOrUncheck(true);
        }

        private void uncheckAllCommits(object sender, RoutedEventArgs e)
        {
            CheckOrUncheck(false);
        }

        private void CheckOrUncheck(bool value)
        {
            IEnumerable<Commit> commitList = commitsDataGrid.ItemsSource as IEnumerable<Commit>;
            if (commitList != null)
            {
                foreach (var commit in commitList)
                {
                    commit.IsSelected = value;
                }
            }
            commitsDataGrid.Items.Refresh();
        }

        private async void downloadButtonClick(object sender, RoutedEventArgs e)
        {
            downloadTextBlock.Visibility = Visibility.Hidden;
            downloadButton.IsEnabled = false;

            if (_downloadManager.DirectoryExists(downloadDestinationTextBox.Text))
            {
                try
                {
                    await _downloadManager.DownloadFileContentAsync(commitsDataGrid.ItemsSource, downloadDestinationTextBox.Text);
                    downloadTextBlock.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error downloading files: " + ex.Message,
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
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