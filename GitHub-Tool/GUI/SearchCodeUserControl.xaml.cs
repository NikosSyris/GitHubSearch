using System;
using Octokit;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using GitHub_Tool.Action;
using GitHub_Tool.Model;
using Model = GitHub_Tool.Model;

namespace GitHub_Tool.GUI
{

    public partial class SearchCodeUserControl : UserControl
    {

        CodeSearch codeSearch;

        public SearchCodeUserControl()
        {
            InitializeComponent();
            codeSearch = new CodeSearch();
            languageComboBox.ItemsSource = Enum.GetValues(typeof(Language));
            languageComboBox.SelectedIndex = (int)Enum.Parse(typeof(Language), "Unknown");
        }


        private async void searchCodeButtonClick(object sender, RoutedEventArgs e)
        {
            GlobalVariables.accessToken = accessTokenTextBox.Text;
            GlobalVariables.client = GlobalVariables.createGithubClient();
            noResultsLabel.Visibility = Visibility.Hidden;

            SearchCodeRequestParameters requestParameters = new SearchCodeRequestParameters(termTextBox.Text, extensionTextBox.Text, ownerTextBox.Text,
                                                                Int32.Parse(sizeTextBox.Text), languageComboBox.Text, pathIncludedCheckBox.IsChecked, forkComboBox.Text,
                                                                fileNameTextBox.Text, pathTextBox.Text, sizeComboBox.Text);

            var result = await codeSearch.searchCode(requestParameters);
            noResultsLabel.Visibility = pickVisibility(result.Count);
            filesDataGrid.ItemsSource = result;
            numberOfResults.Text = result.Count.ToString();
        }


        private Visibility pickVisibility(int numberOfResults)
        {
            if (numberOfResults == 0)
            {
                return Visibility.Visible;
            }
            return Visibility.Hidden;
        }


        private async void showCommitsOnClick(object sender, RoutedEventArgs e)
        {

            CommitWindow commitWindow = new CommitWindow();
            var selectedFile = (File)filesDataGrid.CurrentCell.Item;
            List<Model.Commit> commitList = await codeSearch.getCommitsForFIle(selectedFile.Owner, selectedFile.RepoName, selectedFile.Path).ConfigureAwait(false);

            this.Dispatcher.Invoke(() =>
            {
                commitWindow.CommitsDataGrid.ItemsSource = commitList;
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
