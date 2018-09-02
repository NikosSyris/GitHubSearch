using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GitHub_Tool
{

    public partial class UserControl1 : UserControl
    {

        Search search;

        public UserControl1()
        {
            InitializeComponent();
            search = new Search();
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.accessToken = accessTokenTextBox.Text;
            GlobalVariables.client = MainWindow.createGithubClient();

            var result = await search.SearchCode(term.Text, Int32.Parse(minNumberOfCommits.Text), Extension.Text, name.Text/*, Int32.Parse(size.Text)*/ );
            dgCandidate.ItemsSource = result;
        }


        private  async void ShowContentOnClick(object sender, RoutedEventArgs e)
        {
            ContentWindow w = new ContentWindow();
            var x = (FileInformation)dgCandidate.CurrentCell.Item;   

            var file = await GlobalVariables.client
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
    }
}
