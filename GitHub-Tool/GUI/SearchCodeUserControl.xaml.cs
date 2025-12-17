using System;
using Octokit;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using GitHubSearch.Action;
using GitHubSearch.Model;
using GitHubSearch.Services;
using Model = GitHubSearch.Model;
using GitHubSearch.Action.Validation;

namespace GitHubSearch.GUI
{

    public partial class SearchCodeUserControl : UserControl
    {

        public string AccessToken { get; set; }
        public string Owner { get; set; }
        public string FileName { get; set; }
        public string Term { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public int Size { get; set; }

        private GitHubClientService _clientService;
        private CodeSearchManager _codeSearchManager;
        private DownloadLocallyManager _downloadManager;
        private DateValidator _dateValidator;
        private SearchCodeRequestParameters _requestParameters;
        private PageDialog _pageDialog;
        private CommitWindow _commitWindow;
        private DownloadResultsWindow _downloadResultsWindow;

        // Track if we're authenticated to avoid recreating the client unnecessarily
        private string _lastUsedToken;

        public SearchCodeUserControl()
        {
            InitializeComponent();

            _clientService = new GitHubClientService();
            _dateValidator = new DateValidator();
            // Note: _downloadManager will be created after authentication

            languageComboBox.ItemsSource = Enum.GetValues(typeof(Language));
            languageComboBox.SelectedIndex = (int)Enum.Parse(typeof(Language), "Unknown");
            DataContext = this;
        }


        private void searchCodeButtonClick(object sender, RoutedEventArgs e)
        {
            if (IsFormValid())
            {
                searchButton.IsEnabled = false;
                downloadResultsButton.Visibility = Visibility.Hidden;
                noResultsLabel.Visibility = Visibility.Hidden;

                // Only create new client if token has changed
                InitializeClientIfNeeded(accessTokenTextBox.Text);

                _requestParameters = new SearchCodeRequestParameters(
                    term: termTextBox.Text,
                    extension: extensionTextBox.Text,
                    owner: ownerTextBox.Text,
                    size: Int32.Parse(sizeTextBox.Text),
                    language: languageComboBox.Text,
                    pathIncluded: pathIncludedCheckBox.IsChecked,
                    forksIncluded: forkComboBox.Text,
                    fileName: fileNameTextBox.Text,
                    path: pathTextBox.Text,
                    sizeChoice: sizeComboBox.Text
                );

                TryShowResultsAsync(_requestParameters);
                searchButton.IsEnabled = true;
            }
        }


        private void InitializeClientIfNeeded(string accessToken)
        {
            if (_lastUsedToken != accessToken)
            {
                _clientService.Authenticate(accessToken);
                _codeSearchManager = new CodeSearchManager(_clientService);
                _downloadManager = new DownloadLocallyManager(_clientService);
                _lastUsedToken = accessToken;
            }
        }


        private async void TryShowResultsAsync(SearchCodeRequestParameters requestParameters)
        {
            try
            {
                var pagesCount = await _codeSearchManager.GetPageCountAsync(requestParameters);

                if (pagesCount != 0)
                {
                    _pageDialog = new PageDialog(pagesCount);
                    _pageDialog.ShowDialog();

                    var result = await _codeSearchManager.SearchCodeAsync(
                        requestParameters,
                        _pageDialog.FirstPage,
                        _pageDialog.LastPage
                    );

                    filesDataGrid.ItemsSource = result;
                    downloadResultsButton.Visibility = Visibility.Visible;
                }
                else
                {
                    filesDataGrid.ItemsSource = null;
                    noResultsLabel.Visibility = Visibility.Visible;
                }
            }
            catch (ApiValidationException exception)
            {
                ShowError(exception.Message);
            }
            catch (RateLimitExceededException exception)
            {
                ShowError(exception.Message);
            }
            catch (System.Net.Http.HttpRequestException)
            {
                ShowError("An error occurred while trying to send the request. Please check your internet connection.");
            }
            catch (AuthorizationException)
            {
                ShowError("Authentication failed: bad credentials");
                // Reset the token so next search will re-authenticate
                _lastUsedToken = null;
            }
            catch (AbuseException exception)
            {
                ShowError($"You have triggered an abuse detection mechanism. Try again after {exception.RetryAfterSeconds} seconds.");
            }
        }


        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }


        private bool IsFormValid()
        {
            return !Validation.GetHasError(termTextBox)
                && !Validation.GetHasError(ownerTextBox)
                && !Validation.GetHasError(pathTextBox)
                && !Validation.GetHasError(fileNameTextBox)
                && !Validation.GetHasError(sizeTextBox)
                && !Validation.GetHasError(extensionTextBox)
                && !Validation.GetHasError(accessTokenTextBox);
        }


        private async void showCommitsOnClick(object sender, RoutedEventArgs e)
        {
            _commitWindow = new CommitWindow(_clientService);
            var selectedFile = (Model.File)filesDataGrid.CurrentCell.Item;

            List<Model.Commit> commitList = await _codeSearchManager.GetCommitsForFileAsync(
                selectedFile.Owner,
                selectedFile.RepoName,
                selectedFile.Path
            );

            this.Dispatcher.Invoke(() =>
            {
                _commitWindow.commitsDataGrid.ItemsSource = commitList;
                _commitWindow.Show();
            });
        }


        private void downloadResultsButtonClick(object sender, RoutedEventArgs e)
        {
            _downloadResultsWindow = new DownloadResultsWindow(_requestParameters, filesDataGrid.Items, _clientService);
            _downloadResultsWindow.Show();
        }


        private void enableDataGridCopying(object sender, DataGridRowClipboardEventArgs e)
        {
            var currentCell = e.ClipboardRowContent[filesDataGrid.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);
        }


        private void HyperlinkOnClick(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            Process.Start(link.NavigateUri.AbsoluteUri);
        }
    }
}