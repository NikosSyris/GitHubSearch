using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Octokit;
using System.Windows.Controls;


namespace GitHub_Tool
{
    public partial class MainWindow : Window
    {
        
        TabItem tabUserPage;

        public MainWindow()
        {
            InitializeComponent();
        }
        

        private void searchCodeUserControlButtonClick(object sender, RoutedEventArgs e)
        {
            MainTab.Items.Clear(); 
            var searchCodeUserControl = new SearchCodeUserControl();
            tabUserPage = new TabItem { Content = searchCodeUserControl };
            MainTab.Items.Add(tabUserPage); 
            MainTab.Items.Refresh();
        }


        private void searchReposUserControlButtonClick(object sender, RoutedEventArgs e)
        {
            MainTab.Items.Clear(); 
            var searchRepoUserControl = new SearchRepoUserControl();
            tabUserPage = new TabItem { Content = searchRepoUserControl };
            MainTab.Items.Add(tabUserPage); 
            MainTab.Items.Refresh();
        }
    }
}










