using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using GitHubSearch.Action;
using GitHubSearch.Model;
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
        RepoSearchManager repoSearch;
        RepoManager repoManager;
        SearchRepositoriesRequestParameters requestParameters;
        DateValidator DateValidator;
        PageDialog pageDialog;
        DownloadResultsWindow downloadResultsWindow;
        RepositoryWindow repositoryWindow;

        public SearchRepoUserControl()
        {
            InitializeComponent();
            datePicker.DisplayDateEnd = DateTime.Today;
            endDatePicker.DisplayDateEnd = DateTime.Today;
            updateDate.DisplayDateEnd = DateTime.Today;
            DateValidator = new DateValidator();
            languageComboBox.ItemsSource = Enum.GetValues(typeof(Language));
            languageComboBox.SelectedIndex = (int)Enum.Parse(typeof(Language), "Unknown");
            AccessToken = "b662ba89eb7878f9e75b885789bda4dbbb5115ec";
            repoSearch = new RepoSearchManager();
            repoManager = new RepoManager();
            DataContext = this;
        }


        private void searchReposButtonClick(object sender, RoutedEventArgs e)
        {
            if ( isValid(termTextBox, ownerTextBox, starsTextBox, forksTextBox, sizeTextBox, accessTokenTextBox) ) 
            {
                searchButton.IsEnabled = false;
                downloadResultsButton.Visibility = Visibility.Hidden;
                noResultsLabel.Visibility = Visibility.Hidden;
                endDatePicker.SelectedDate = DateValidator.validate(datePicker, endDatePicker);
                GlobalVariables.client = GlobalVariables.createGithubClient(accessTokenTextBox.Text);                

                requestParameters = new SearchRepositoriesRequestParameters(termTextBox.Text, ownerTextBox.Text,
                                                                            Int32.Parse(starsTextBox.Text), Int32.Parse(forksTextBox.Text),
                                                                            Int32.Parse(sizeTextBox.Text), sortComboBox.Text, orderComboBox.Text,
                                                                            datePicker.SelectedDate, dateComboBox.Text, updateDate.SelectedDate,
                                                                            languageComboBox.Text, endDatePicker.SelectedDate, ReadmeIncludedCheckBox.IsChecked,
                                                                            starsComboBox.Text, forksComboBox.Text, sizeComboBox.Text);

                tryShowResults(requestParameters);
                searchButton.IsEnabled = true;
            }
        }


        private async void tryShowResults(SearchRepositoriesRequestParameters requestParameters)
        {
            try
            {
                var pagesCount = await repoSearch.getNumberOfPages(requestParameters);

                if (pagesCount != 0)
                {
                    pageDialog = new PageDialog(pagesCount);
                    pageDialog.ShowDialog();
                    var result = await repoSearch.searchRepos(requestParameters, pageDialog.FirstPage, pageDialog.LastPage);
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
                MessageBox.Show("The listed users and repositories cannot be searched either because the " +
                    "resources do not exist or you do not have permission to view them.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            catch (RateLimitExceededException exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (System.Net.Http.HttpRequestException)
            {
                MessageBox.Show("An error occurred while trying to send the request. Please check your" +
                    " internet connection", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (AuthorizationException)
            {
                MessageBox.Show("Authentication failed: bad credentials", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (AbuseException exception)
            {
                MessageBox.Show("You have triggered an abuse detection mechanism. Try again after " + exception.RetryAfterSeconds +
                                " seconds", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private bool isValid(TextBox term, TextBox owner, TextBox stars, TextBox forks, TextBox size, TextBox accessToken)
        {
            return !Validation.GetHasError(term) && !Validation.GetHasError(owner) && !Validation.GetHasError(stars)
                    && !Validation.GetHasError(forks) && !Validation.GetHasError(size) && !Validation.GetHasError(accessToken);
        }

        private void downloadResultsButtonClick(object sender, RoutedEventArgs e)
        {
            downloadResultsWindow = new DownloadResultsWindow(requestParameters, reposDataGrid.Items);
            downloadResultsWindow.Show();
        }


        private async void showStructureOnClick(object sender, RoutedEventArgs e)
        {
            var tempRepo = (Model.Repository)reposDataGrid.CurrentCell.Item;

            tempRepo = await repoManager.getRepoStructure(tempRepo);
            repositoryWindow = new RepositoryWindow(tempRepo.RootFolder);
            repositoryWindow.filesDataGrid.ItemsSource = tempRepo.Files;
            repositoryWindow.test.Items.Add(tempRepo);

            this.Dispatcher.Invoke(() =>
            {
                repositoryWindow.Show();
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
