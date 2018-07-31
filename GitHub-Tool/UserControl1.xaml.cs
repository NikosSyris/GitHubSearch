using System;
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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {


        TabItem tabUserPage;
        Search search = new Search();
        Commit commit = new Commit();
        Download download = new Download();



        public UserControl1()
        {
            InitializeComponent();
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.accessToken = accessTokenTextBox.Text;

            var result = await search.SearchCode(term: " var apiInfo = client.GetLastApiInfo(); ", minNumberOfCommits: 15);
            var s = commit.getAllCommitsThatChangedAFile(result);
            testBlock2.Text = s;
            // download.downloadContent(fileInformation);
            testBlock.Text = "done baby";
        }
    }
}
