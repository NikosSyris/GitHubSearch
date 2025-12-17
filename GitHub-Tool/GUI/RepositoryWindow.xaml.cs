using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using GitHubSearch.Action;
using GitHubSearch.Model;
using GitHubSearch.Services;

namespace GitHubSearch.GUI
{
    public partial class RepositoryWindow : Window
    {
        private Folder _root;
        private GitHubClientService _clientService;
        private CodeSearchManager _codeSearchManager;
        private CommitWindow _commitWindow;

        public RepositoryWindow(Folder rootFolder, GitHubClientService clientService)
        {
            InitializeComponent();
            _root = rootFolder;
            _clientService = clientService;
            _codeSearchManager = new CodeSearchManager(clientService);
        }

        private void loadTreeView(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = _root.Name;
            item = GetFolder(_root, item);
            var tree = sender as TreeView;
            tree.Items.Add(item);
        }

        private TreeViewItem GetFolder(Folder folder, TreeViewItem item)
        {
            foreach (Folder tempFolder in folder.FolderList)
            {
                TreeViewItem tempItem = new TreeViewItem();
                tempItem.Header = tempFolder.Name;
                item.Items.Add(tempItem);
                tempItem = GetFolder(tempFolder, tempItem);
            }

            foreach (File file in folder.FileList)
            {
                item.Items.Add(new TreeViewItem() { Header = file.Name });
            }

            return item;
        }

        private async void showCommitsOnClick(object sender, RoutedEventArgs e)
        {
            _commitWindow = new CommitWindow(_clientService);
            var selectedFile = (File)filesDataGrid.CurrentCell.Item;

            List<Commit> commitList = await _codeSearchManager.GetCommitsForFileAsync(
                selectedFile.Owner,
                selectedFile.RepoName,
                selectedFile.Path
            ).ConfigureAwait(false);

            this.Dispatcher.Invoke(() =>
            {
                _commitWindow.commitsDataGrid.ItemsSource = commitList;
                _commitWindow.Show();
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