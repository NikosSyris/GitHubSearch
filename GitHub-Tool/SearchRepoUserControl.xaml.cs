using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace GitHub_Tool
{

    public partial class SearchRepoUserControl : UserControl
    {

        Search search; 

        public SearchRepoUserControl()
        {
            InitializeComponent();
            search = new Search();
        }

        private async void searchReposButtonClick(object sender, RoutedEventArgs e)
        {
            GlobalVariables.accessToken = accessTokenTextBox.Text;
            GlobalVariables.client = GlobalVariables.createGithubClient();

            var result = await search.searchRepos(owner : owner.Text, name : repo.Text);
            reposDataGrid.ItemsSource = result;

        }

        private async void showStructureOnClick(object sender, RoutedEventArgs e)
        {
            RepositoryWindow repositoryWindow;
            var tempRepo = (Repository)reposDataGrid.CurrentCell.Item;

            tempRepo = await search.getRepoStructure(tempRepo);
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
    }
}
