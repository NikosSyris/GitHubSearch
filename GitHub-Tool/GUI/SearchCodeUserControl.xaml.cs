using System;
using Octokit;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using GitHubSearch.Action;
using GitHubSearch.Model;
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
        CodeSearchManager codeSearch;
        DownloadLocallyManager download;
        DateValidator DateValidator;
        SearchCodeRequestParameters requestParameters;
        PageDialog pageDialog;
        CommitWindow commitWindow;
        DownloadResultsWindow downloadResultsWindow;

        public SearchCodeUserControl()
        {
            InitializeComponent();
            codeSearch = new CodeSearchManager();
            download = new DownloadLocallyManager();
            DateValidator = new DateValidator();
            languageComboBox.ItemsSource = Enum.GetValues(typeof(Language));
            languageComboBox.SelectedIndex = (int)Enum.Parse(typeof(Language), "Unknown");
            AccessToken = "b662ba89eb7878f9e75b885789bda4dbbb5115ec";
            DataContext = this;
        }


        private void searchCodeButtonClick(object sender, RoutedEventArgs e)
        {

            if (isValid(termTextBox, ownerTextBox, fileNameTextBox, extensionTextBox, pathTextBox, sizeTextBox, accessTokenTextBox))
            {
                searchButton.IsEnabled = false;
                downloadResultsButton.Visibility = Visibility.Hidden;
                noResultsLabel.Visibility = Visibility.Hidden;
                GlobalVariables.client = GlobalVariables.createGithubClient(accessTokenTextBox.Text);

                requestParameters = new SearchCodeRequestParameters(termTextBox.Text, extensionTextBox.Text, ownerTextBox.Text,
                                                                    Int32.Parse(sizeTextBox.Text), languageComboBox.Text,
                                                                    pathIncludedCheckBox.IsChecked, forkComboBox.Text,
                                                                    fileNameTextBox.Text, pathTextBox.Text, sizeComboBox.Text);
                tryShowResults(requestParameters);
                searchButton.IsEnabled = true;
            }
        }


        private async void tryShowResults(SearchCodeRequestParameters requestParameters)
        {
                try
                {
                    var pagesCount = await codeSearch.getNumberOfPages(requestParameters);

                    if (pagesCount != 0)
                    {
                        pageDialog = new PageDialog(pagesCount);
                        pageDialog.ShowDialog();
                        var result = await codeSearch.searchCode(requestParameters, pageDialog.FirstPage, pageDialog.LastPage);
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
                    MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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
        


        private bool isValid(TextBox term, TextBox owner, TextBox fileName, TextBox extension, TextBox path,
                            TextBox size,TextBox accessToken)
        {
            return !Validation.GetHasError(term) && !Validation.GetHasError(owner) && !Validation.GetHasError(path)
                    && !Validation.GetHasError(fileName) && !Validation.GetHasError(size)
                    && !Validation.GetHasError(extension) && !Validation.GetHasError(accessToken);
        }


        private async void showCommitsOnClick(object sender, RoutedEventArgs e)
        {

            commitWindow = new CommitWindow();
            var selectedFile = (Model.File)filesDataGrid.CurrentCell.Item;
            List<Model.Commit> commitList = await codeSearch.getCommitsForFIle(selectedFile.Owner, selectedFile.RepoName, selectedFile.Path);

            this.Dispatcher.Invoke(() =>
            {
                commitWindow.commitsDataGrid.ItemsSource = commitList;
                commitWindow.Show();
            });
        }


        private  void downloadResultsButtonClick(object sender, RoutedEventArgs e)
        {
            downloadResultsWindow = new DownloadResultsWindow(requestParameters, filesDataGrid.Items);
            downloadResultsWindow.Show();           
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
