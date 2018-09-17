using System.Windows;
using System.Windows.Controls;
using GitHub_Tool.Model;
using GitHub_Tool.Action;
using System.Collections;
using System.Linq;

namespace GitHub_Tool.GUI
{

    public partial class CommitWindow : Window
    {

        Download download;

        public CommitWindow()
        {
            InitializeComponent();
            download = new Download();
        }


        private void checkAllCommits(object sender, RoutedEventArgs e)     //doesn't work for some reason
        {
            //var firstCol = CommitsDataGrid.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
            foreach (Commit commit in CommitsDataGrid.ItemsSource)
            {
                //commit.IsSelected = true;
                //var chBx = firstCol.GetCellContent(commit) as CheckBox;
                //chBx.IsChecked = chkSelectAll.IsChecked;
            }
            //var itemsSource = CommitsDataGrid.ItemsSource as IEnumerable;
        }

        private void uncheckAllCommits(object sender, RoutedEventArgs e)    //doesn't work for some reason
        {
            foreach (Commit commit in CommitsDataGrid.ItemsSource)
            {
                commit.IsSelected = false;
            }
        }



        private void downloadButtonClick(object sender, RoutedEventArgs e)    
        {
            downloadTextBlock.Visibility = Visibility.Hidden;
            downloadButton.IsEnabled = false;

            foreach (Commit commit in CommitsDataGrid.ItemsSource)
            {
                if (commit.IsSelected == true)
                {
                    download.downloadContent(commit); 
                }
            }

            downloadButton.IsEnabled = true;
            downloadTextBlock.Visibility = Visibility.Visible;
        }

        private void enableDataGridCopying(object sender, DataGridRowClipboardEventArgs e)
        {

            var currentCell = e.ClipboardRowContent[CommitsDataGrid.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);

        }
    }
}
