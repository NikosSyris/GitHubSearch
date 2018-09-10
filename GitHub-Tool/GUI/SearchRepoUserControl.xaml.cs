using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using GitHub_Tool.Action;
using GitHub_Tool.Model;

namespace GitHub_Tool.GUI
{

    public partial class SearchRepoUserControl : UserControl
    {

        RepoSearch repoSearch;
        RepoManager repoManager;

        public SearchRepoUserControl()
        {
            InitializeComponent();
            repoSearch = new RepoSearch();
            repoManager = new RepoManager();
        }

        private async void searchReposButtonClick(object sender, RoutedEventArgs e)
        {
            GlobalVariables.accessToken = accessTokenTextBox.Text;
            GlobalVariables.client = GlobalVariables.createGithubClient();

            var result = await repoSearch.searchRepos(ownerTextBox.Text, Int32.Parse(starsTextBox.Text), Int32.Parse(sizeTextBox.Text));
            reposDataGrid.ItemsSource = result;

        }

        private async void showStructureOnClick(object sender, RoutedEventArgs e)
        {
            RepositoryWindow repositoryWindow;
            var tempRepo = (Repository)reposDataGrid.CurrentCell.Item;

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
    }
}
