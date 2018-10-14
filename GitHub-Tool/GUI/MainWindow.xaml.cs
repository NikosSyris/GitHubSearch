using System.Windows;
using System.Windows.Controls;


namespace GitHubSearch.GUI
{
    public partial class MainWindow : Window
    {
        
        TabItem tabUserPage;
        SearchCodeUserControl searchCodeUserControl;
        SearchRepoUserControl searchRepoUserControl;

        public MainWindow()
        {
            InitializeComponent();
        }
        

        private void searchCodeUserControlButtonClick(object sender, RoutedEventArgs e)
        {
            MainTab.Items.Clear(); 
            searchCodeUserControl = new SearchCodeUserControl();
            tabUserPage = new TabItem { Content = searchCodeUserControl };
            MainTab.Items.Add(tabUserPage); 
            MainTab.Items.Refresh();
        }


        private void searchReposUserControlButtonClick(object sender, RoutedEventArgs e)
        {
            MainTab.Items.Clear(); 
            searchRepoUserControl = new SearchRepoUserControl();
            tabUserPage = new TabItem { Content = searchRepoUserControl };
            MainTab.Items.Add(tabUserPage); 
            MainTab.Items.Refresh();
        }
    }
}










