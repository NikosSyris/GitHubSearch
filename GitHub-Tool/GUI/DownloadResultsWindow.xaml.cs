using GitHubSearch.Action;
using GitHubSearch.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GitHubSearch.GUI
{

    public partial class DownloadResultsWindow : Window
    {
        DownloadLocallyManager download;
        RequestParameters requestParameters;
        ItemCollection files;

        public DownloadResultsWindow(RequestParameters requestParameters, ItemCollection files)
        {
            InitializeComponent();
            download = new DownloadLocallyManager();
            downloadDestinationTextBox.Text = download.DefaultDownloadDestination;
            fileNameTextBox.Text = download.DefaultName;
            this.requestParameters = requestParameters;
            this.files = files;
        }

        private void downloadButtonClick(object sender, RoutedEventArgs e)
        {
            downloadTextBlock.Visibility = Visibility.Hidden;
            downloadButton.IsEnabled = false;

            try
            {
                download.downloadAllSearchResults(requestParameters, files, downloadDestinationTextBox.Text, fileNameTextBox.Text);
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
