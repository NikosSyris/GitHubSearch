using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using GitHubSearch.Action;
using GitHubSearch.Model;
using GitHubSearch.Services;
using Model = GitHubSearch.Model;
using Octokit;
using System.ComponentModel;
using GitHubSearch.Action.Validation;

namespace GitHubSearch.GUI
{

    public partial class SearchRepoUserControl : UserControl
    {

        public string AccessToken { get; set; }
        public string Owner { get; set; }
        public string Term { get; set; }
        public int Stars { get; set; }
        public int Forks { get; set; }
        public int Size { get; set; }

        private GitHubClientService _clientService;
        private RepoSearchManager _repoSearchManager;
        private RepoManager _repoManager;
        private SearchRepositoriesRequestParameters _requestParameters;
        private DateValidator _dateValidator;
        private PageDialog _pageDialog;
        private DownloadResultsWindow _downloadResultsWindow;
        private RepositoryWindow _repositoryWindow;

        // Track if we're authenticated to avoid recreating the client unnecessarily
        private string _lastUsedToken;

        public SearchRepoUserControl()
        {
            InitializeComponent();
            datePicker.DisplayDateEnd = DateTime.Today;
            endDatePicker.DisplayDateEnd = DateTime.Today;
            updateDate.DisplayDateEnd = DateTime.Today;

            _clientService = new GitHubClientService();
            _dateValidator = new DateValidator();

            languageComboBox.ItemsSource = Enum.GetValues(typeof(Language));
            languageComboBox.SelectedIndex = (int)Enum.Parse(typeof(Language), "Unknown");
            DataContext = this;
        }

        private void searchReposButtonClick(object sender, RoutedEventArgs e)
        {
            if (IsFormValid())
            {
                searchButton.IsEnabled = false;
                downloadResultsButton.Visibility = Visibility.Hidden;
                noResultsLabel.Visibility = Visibility.Hidden;
                endDatePicker.SelectedDate = _dateValidator.validate(datePicker, endDatePicker);

                // Only create new client if token has changed
                InitializeClientIfNeeded(accessTokenTextBox.Text);

                _requestParameters = new SearchRepositoriesRequestParameters(
                    termTextBox.Text,
                    ownerTextBox.Text,
                    Int32.Parse(starsTextBox.Text),
                    Int32.Parse(forksTextBox.Text),
                    Int32.Parse(sizeTextBox.Text),
                    sortComboBox.Text,
                    orderComboBox.Text,
                    datePicker.SelectedDate,
                    dateComboBox.Text,
                    updateDate.SelectedDate,
                    languageComboBox.Text,
                    endDatePicker.SelectedDate,
                    ReadmeIncludedCheckBox.IsChecked,
                    starsComboBox.Text,
                    forksComboBox.Text,
                    sizeComboBox.Text
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
                _repoSearchManager = new RepoSearchManager(_clientService);
                _repoManager = new RepoManager(_clientService);
                _lastUsedToken = accessToken;
            }
        }


        private async void TryShowResultsAsync(SearchRepositoriesRequestParameters requestParameters)
        {
            try
            {
                var pagesCount = await _repoSearchManager.GetPageCountAsync(requestParameters);

                if (pagesCount != 0)
                {
                    _pageDialog = new PageDialog(pagesCount);
                    _pageDialog.ShowDialog();

                    var result = await _repoSearchManager.SearchRepositoriesAsync(
                        requestParameters,
                        _pageDialog.FirstPage,
                        _pageDialog.LastPage
                    );

                    reposDataGrid.ItemsSource = result;
                    downloadResultsButton.Visibility = Visibility.Visible;
                }
                else
                {
                    reposDataGrid.ItemsSource = null;
                    noResultsLabel.Visibility = Visibility.Visible;
                }
            }
            catch (ApiValidationException)
            {
                ShowError("The listed users and repositories cannot be searched either because the " +
                    "resources do not exist or you do not have permission to view them.");
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
                && !Validation.GetHasError(starsTextBox)
                && !Validation.GetHasError(forksTextBox)
                && !Validation.GetHasError(sizeTextBox)
                && !Validation.GetHasError(accessTokenTextBox);
        }


        private void downloadResultsButtonClick(object sender, RoutedEventArgs e)
        {
            _downloadResultsWindow = new DownloadResultsWindow(_requestParameters, reposDataGrid.Items, _clientService);
            _downloadResultsWindow.Show();
        }


        private async void showStructureOnClick(object sender, RoutedEventArgs e)
        {
            var tempRepo = (Model.Repository)reposDataGrid.CurrentCell.Item;

            tempRepo = await _repoManager.GetRepositoryStructureAsync(tempRepo);
            _repositoryWindow = new RepositoryWindow(tempRepo.RootFolder, _clientService);
            _repositoryWindow.filesDataGrid.ItemsSource = tempRepo.Files;
            _repositoryWindow.test.Items.Add(tempRepo);

            this.Dispatcher.Invoke(() =>
            {
                _repositoryWindow.Show();
            });
        }


        private void enableDataGridCopying(object sender, DataGridRowClipboardEventArgs e)
        {
            var currentCell = e.ClipboardRowContent[reposDataGrid.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);
        }


        private void HyperlinkOnClick(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            Process.Start(link.NavigateUri.AbsoluteUri);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}