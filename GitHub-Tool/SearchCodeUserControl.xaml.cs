using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GitHub_Tool
{

    public partial class SearchCodeUserControl : UserControl
    {

        Search search;

        public SearchCodeUserControl()
        {
            InitializeComponent();
            search = new Search();
        }


        private async void searchCodeButtonClick(object sender, RoutedEventArgs e)
        {
            GlobalVariables.accessToken = accessTokenTextBox.Text;
            GlobalVariables.client = GlobalVariables.createGithubClient();

            var result = await search.searchCode(term.Text, Int32.Parse(minNumberOfCommits.Text), Extension.Text, name.Text, Int32.Parse(size.Text) );
            filesDataGrid.ItemsSource = result;
            numberOfResults.Text = result.Count.ToString();
        }


        private  async void showContentOnClick(object sender, RoutedEventArgs e)
        {
            ContentWindow contentWindow = new ContentWindow();
            var tempFile = (File)filesDataGrid.CurrentCell.Item;

            var filesLatestVersion = await search.getLatestVersion(tempFile.Owner, tempFile.RepoName, tempFile.Path);

            this.Dispatcher.Invoke(() =>
            {
                contentWindow.Show();
                contentWindow.fileContentTextBox.Text = filesLatestVersion.Content;
            });
        }


        private  void showCommitsOnClick(object sender, RoutedEventArgs e)
        {

            CommitWindow commitWindow = new CommitWindow();
            List<Commit> commitList = new List<Commit>();
            var tempFile = (File)filesDataGrid.CurrentCell.Item;

            this.Dispatcher.Invoke(() =>
            {
                commitWindow.CommitsDataGrid.ItemsSource = tempFile.AllCommits;
                commitWindow.Show();
            });
        }



        private void enableDataGridCopying(object sender, DataGridRowClipboardEventArgs e)
        {

            var currentCell = e.ClipboardRowContent[filesDataGrid.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);

        }
    }
}
