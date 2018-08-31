using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace GitHub_Tool
{


    public partial class RepositoryWindow : Window
    {

        private Folder root;


        public RepositoryWindow(Folder rootFolder)
        {
            InitializeComponent();
            root = rootFolder;
        }


        private void TreeView_Loaded(object sender, RoutedEventArgs e)
        {

            //Image image = new Image();
            //image.Height = 16;
            //image.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/folder.png"));

            TreeViewItem item = new TreeViewItem();
            item.Header = root.Name;

            item = GetFolder(root,item);

            var tree = sender as TreeView;
            tree.Items.Add(item);
        }




        private  TreeViewItem  GetFolder(Folder folder, TreeViewItem item)
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




        private async void ShowCommitsOnClick(object sender, RoutedEventArgs e)
        {
            var client = MainWindow.createGithubClient();
            Search search = new Search();
            CommitWindow w = new CommitWindow();
            List<CommitTemp> commitTempList = new List<CommitTemp>();


            var x = (File)dgCandidate.CurrentCell.Item;

            var commitsForFile = await search.getCommitsForFIle(x.Owner, x.RepoName, x.Path).ConfigureAwait(false);


            this.Dispatcher.Invoke(() =>
            {

                for (var i = 0; i < commitsForFile.Count; i++)
                {
                    commitTempList.Add(new CommitTemp(x.Owner, x.RepoName, x.Path, commitsForFile[i].Sha));
                }

                w.dgCandidate.ItemsSource = commitTempList;
                w.Show();

            });

        }









        //private void DatagridLoaded(object sender, RoutedEventArgs e)
        //{

        //    //// ... Create a List of objects.
        //    //var items = new List<Dog>();
        //    //items.Add(new Dog("Fido", 10));
        //    //items.Add(new Dog("Spark", 20));
        //    //items.Add(new Dog("Fluffy", 4));

        //    //// ... Assign ItemsSource of DataGrid.
        //    //var datagrid = sender as DataGrid;
        //    //datagrid.ItemsSource = items;



        //}

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var tree = sender as TreeView;

            if (tree.SelectedItem is TreeViewItem)
            {
                var item = tree.SelectedItem as TreeViewItem;
                this.Title = "Selected header: " + item.Header.ToString();
            }
            else if (tree.SelectedItem is string)
            {
                this.Title = "Selected: " + tree.SelectedItem.ToString();
            }
        }

    }
}
