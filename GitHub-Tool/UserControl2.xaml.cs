﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GitHub_Tool
{
    /// <summary>
    /// Interaction logic for UserControl2.xaml
    /// </summary>
    public partial class UserControl2 : UserControl
    {


        Search search = new Search();
        Commit commit = new Commit();
        Download download = new Download();


        public UserControl2()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.accessToken = accessTokenTextBox.Text;

            var result = await search.SearchRepos(owner : owner.Text, name : repo.Text);
            dgCandidate.ItemsSource = result;

        }

        private async void ShowStructureOnClick(object sender, RoutedEventArgs e)
        {
            var client = MainWindow.createGithubClient();
            RepositoryWindow w;
            List<Repository> repoList = new List<Repository>();

            var x = (Repository)dgCandidate.CurrentCell.Item;
            Folder root = await search.getRepoStructure(x.Owner, x.Name);
            w = new RepositoryWindow(root);

            this.Dispatcher.Invoke(() =>
            {
                //w.testBlock.Text = temp;

                w.Show();
            });
        }
    }
}