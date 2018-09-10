using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using GitHub_Tool.Action;
using GitHub_Tool.Model;

namespace GitHub_Tool.GUI
{

    public partial class SearchCodeUserControl : UserControl
    {

        CodeSearch codeSearch;

        public SearchCodeUserControl()
        {
            InitializeComponent();
            codeSearch = new CodeSearch();
        }


        private async void searchCodeButtonClick(object sender, RoutedEventArgs e)
        {
            GlobalVariables.accessToken = accessTokenTextBox.Text;
            GlobalVariables.client = GlobalVariables.createGithubClient();

            var result = await codeSearch.searchCode(term.Text, Int32.Parse(minNumberOfCommits.Text), Extension.Text, name.Text, Int32.Parse(size.Text) );
            filesDataGrid.ItemsSource = result;
            numberOfResults.Text = result.Count.ToString();
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

        private void HyperlinkOnClick(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            Process.Start(link.NavigateUri.AbsoluteUri);
        }
    }
}
