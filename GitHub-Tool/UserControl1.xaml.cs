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
            

            var result = await search.SearchCode(term: term.Text, minNumberOfCommits: Int32.Parse(minNumberOfCommits.Text), name : name.Text, extension : Extension.Text);
            //var s = commit.getAllCommitsThatChangedAFile(result);
            //testBlock2.Text = s;
            // download.downloadContent(fileInformation);
            dgCandidate.ItemsSource = result;
        }





        private  async void ShowContentOnClick(object sender, RoutedEventArgs e)
        {
            var client = MainWindow.createGithubClient();
            ContentWindow w = new ContentWindow();


            var x = (FileInformation)dgCandidate.CurrentCell.Item;   //dgCandidate.Items.CurrentItem;  showed only the first item
            //var y = x.FilePath;

            var file = await client
                    .Repository
                    .Content
                    .GetAllContents(x.Owner, x.Repo, x.FilePath)
                    .ConfigureAwait(false);



            this.Dispatcher.Invoke(() =>
            {
                w.Show();

                foreach (var element in file)
                {
                    w.fileContentTextBox.Text = element.Content;
                }
            });

        }



        private  void ShowCommitsOnClick(object sender, RoutedEventArgs e)
        {
            var client = MainWindow.createGithubClient();
            CommitWindow w = new CommitWindow();
            List<CommitTemp> commitTempList = new List<CommitTemp>();


            var x = (FileInformation)dgCandidate.CurrentCell.Item;


            this.Dispatcher.Invoke(() =>
            {

                for (var i = 0; i < x.AllCommits.Count; i++)
                {
                    commitTempList.Add(new CommitTemp(x.Owner, x.Repo, x.FilePath, x.AllCommits[i].Sha));
                }

                w.dgCandidate.ItemsSource = commitTempList;
                w.Show();

            });

        }


        private void chkSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach (FileInformation c in dgCandidate.ItemsSource)
            {
                c.IsSelected = true;
            }
        }

        private void chkSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (FileInformation c in dgCandidate.ItemsSource)
            {
                c.IsSelected = false;
            }
        }



        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            foreach (FileInformation c in dgCandidate.ItemsSource)
            {
                testBlock.Text += c.IsSelected + "    ";
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
