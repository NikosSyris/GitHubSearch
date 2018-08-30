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





















            private void TreeView_SelectedItemChanged(object sender,
            RoutedPropertyChangedEventArgs<object> e)
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
