using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace GitHub_Tool
{

    public partial class UserControl2 : UserControl
    {

        Search search; 

        public UserControl2()
        {
            InitializeComponent();
            search = new Search();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.accessToken = accessTokenTextBox.Text;
            GlobalVariables.client = MainWindow.createGithubClient();

            var result = await search.SearchRepos(owner : owner.Text, name : repo.Text);
            dgCandidate.ItemsSource = result;

        }

        private async void ShowStructureOnClick(object sender, RoutedEventArgs e)
        {
            RepositoryWindow w;
            List<Repository> repoList = new List<Repository>();

            var tempRepo = (Repository)dgCandidate.CurrentCell.Item;
            tempRepo = await search.getRepoStructure(tempRepo);
            w = new RepositoryWindow(tempRepo.RootFolder);
            w.dgCandidate.ItemsSource = tempRepo.RepositoryContentList;

            this.Dispatcher.Invoke(() =>
            {
                w.Show();
            });
        }
    }
}
