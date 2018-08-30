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
using System.Windows.Shapes;

namespace GitHub_Tool
{
    /// <summary>
    /// Interaction logic for CommitWindow.xaml
    /// </summary>
    public partial class CommitWindow : Window
    {
        public CommitWindow()
        {
            InitializeComponent();
        }


        private void chkSelectAll_Checked(object sender, RoutedEventArgs e)     //doesn't work for some reason
        {
            foreach (CommitTemp c in dgCandidate.ItemsSource)
            {
                c.IsSelected = true;
            }
        }

        private void chkSelectAll_Unchecked(object sender, RoutedEventArgs e)    //doesn't work for some reason
        {
            foreach (CommitTemp c in dgCandidate.ItemsSource)
            {
                c.IsSelected = false;
            }
        }



        private void download_Button(object sender, RoutedEventArgs e)    
        {

            Download download = new Download();

            foreach (CommitTemp c in dgCandidate.ItemsSource)
            {
                if (c.IsSelected == true)
                {
                    download.downloadContent(c); 
                }
            }
        }

        


    }
}
