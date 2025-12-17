using GitHubSearch.Action;
using GitHubSearch.Model;
using GitHubSearch.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GitHubSearch.GUI
{
    public partial class DownloadResultsWindow : Window
    {
        private DownloadLocallyManager _downloadManager;
        private RequestParameters _requestParameters;
        private ItemCollection _files;

        public DownloadResultsWindow(RequestParameters requestParameters, ItemCollection files, GitHubClientService clientService)
        {
            InitializeComponent();
            _downloadManager = new DownloadLocallyManager(clientService);
            downloadDestinationTextBox.Text = _downloadManager.DefaultDownloadDestination;
            fileNameTextBox.Text = _downloadManager.DefaultName;
            _requestParameters = requestParameters;
            _files = files;
        }

        private void downloadButtonClick(object sender, RoutedEventArgs e)
        {
            downloadTextBlock.Visibility = Visibility.Hidden;
            downloadButton.IsEnabled = false;

            try
            {
                _downloadManager.DownloadAllSearchResults(_requestParameters, _files, downloadDestinationTextBox.Text, fileNameTextBox.Text);
                downloadTextBlock.Visibility = Visibility.Visible;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            downloadButton.IsEnabled = true;
        }
    }
}