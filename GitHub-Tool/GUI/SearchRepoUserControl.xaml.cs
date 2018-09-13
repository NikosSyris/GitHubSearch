using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using GitHub_Tool.Action;
using GitHub_Tool.Model;
using Model = GitHub_Tool.Model;
using Octokit;
using System.ComponentModel;

namespace GitHub_Tool.GUI
{

    public partial class SearchRepoUserControl : UserControl
    {

        public string Owner { get; set; }
        public string Term { get; set; }
        public int Stars { get; set; }
        public int Forks { get; set; }
        public int Size { get; set; }
        RepoSearch repoSearch;
        RepoManager repoManager;
        DateValidator DateValidator;

        public SearchRepoUserControl()
        {
            InitializeComponent();
            datePicker.DisplayDateEnd = DateTime.Today;
            endDatePicker.DisplayDateEnd = DateTime.Today;
            updateDate.DisplayDateEnd = DateTime.Today;
            DateValidator = new DateValidator();
            languageComboBox.ItemsSource = Enum.GetValues(typeof(Language));
            languageComboBox.SelectedIndex = (int)Enum.Parse(typeof(Language), "Unknown");
            repoSearch = new RepoSearch();
            repoManager = new RepoManager();
            DataContext = this;
        }


        private async void searchReposButtonClick(object sender, RoutedEventArgs e)
        {
            if ( isValid(termTextBox, ownerTextBox, starsTextBox, forksTextBox, sizeTextBox) ) 
            {
                GlobalVariables.accessToken = accessTokenTextBox.Text;
                GlobalVariables.client = GlobalVariables.createGithubClient();
                noResultsLabel.Visibility = Visibility.Hidden;
                endDatePicker.SelectedDate = DateValidator.validate( datePicker, endDatePicker);

                SearchRepositoriesRequestParameters requestParameters = new SearchRepositoriesRequestParameters(termTextBox.Text, ownerTextBox.Text, Int32.Parse(starsTextBox.Text)
                                                                            ,Int32.Parse(forksTextBox.Text), Int32.Parse(sizeTextBox.Text), sortComboBox.Text, orderComboBox.Text,
                                                                            datePicker.SelectedDate, dateComboBox.Text, updateDate.SelectedDate, languageComboBox.Text,
                                                                            endDatePicker.SelectedDate, ReadmeIncludedCheckBox.IsChecked);
                try
                {
                    var result = await repoSearch.searchRepos(requestParameters);
                    noResultsLabel.Visibility = pickVisibility(result.Count);
                    reposDataGrid.ItemsSource = result;
                }
                catch (ApiValidationException exception)
                {
                    MessageBox.Show(exception.Message + ": The listed users and repositories cannot be searched either because the resources do not exist" +
                                            " or you do not have permission to view them.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (RateLimitExceededException exception)
                {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }


        private bool isValid(TextBox term, TextBox owner, TextBox stars, TextBox forks, TextBox size)
        {
            return !Validation.GetHasError(term) && !Validation.GetHasError(owner) && !Validation.GetHasError(stars)
                    && !Validation.GetHasError(forks) && !Validation.GetHasError(size);
        }


        private Visibility pickVisibility(int numberOfResults)
        {
            if (numberOfResults == 0)
            {
                return Visibility.Visible;
            }
            return Visibility.Hidden;
        }


        private async void showStructureOnClick(object sender, RoutedEventArgs e)
        {
            RepositoryWindow repositoryWindow;
            var tempRepo = (Model.Repository)reposDataGrid.CurrentCell.Item;

            tempRepo = await repoManager.getRepoStructure(tempRepo);
            repositoryWindow = new RepositoryWindow(tempRepo.RootFolder);
            repositoryWindow.filesDataGrid.ItemsSource = tempRepo.Files;

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
