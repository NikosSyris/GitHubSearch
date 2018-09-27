using System;
using System.Linq;
using System.Windows;

namespace GitHubSearch.GUI
{

    public partial class PageDialog : Window
    {

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        public int FirstPage { get; set; }
        public int LastPage { get; set; }

        public PageDialog(int totalPages)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            pagesCountTextBlock.Text = totalPages.ToString();
            firstPageComboBox.ItemsSource = Enumerable.Range(1, totalPages);
            lastPageComboBox.ItemsSource = Enumerable.Range(1, totalPages);
            firstPageComboBox.SelectedItem = 1;         
            lastPageComboBox.SelectedItem = 1;
            Loaded += removeCloseButton;
        }

        private void buttonClick(object sender, RoutedEventArgs e)
        {
            FirstPage = Int32.Parse(firstPageComboBox.Text);
            LastPage = Int32.Parse(lastPageComboBox.Text);
            this.Hide();
        }

        void removeCloseButton(object sender, RoutedEventArgs e)
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
    }
}
