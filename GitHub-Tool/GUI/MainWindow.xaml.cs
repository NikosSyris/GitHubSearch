using System.Windows;
using System.Windows.Controls;


namespace GitHubSearch.GUI
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










