using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Octokit;
using System.Windows.Controls;


namespace GitHub_Tool
{
    public partial class MainWindow : Window
    {
        public static String accessToken;
        TabItem tabUserPage;

        public MainWindow()
        {
            InitializeComponent();

        }



        private void BtnUser1_Click(object sender, RoutedEventArgs e)
        {
            MainTab.Items.Clear(); //Clear previous Items in the user controls which is my tabItems
            var userControls = new UserControl1();
            tabUserPage = new TabItem { Content = userControls };
            MainTab.Items.Add(tabUserPage); // Add User Controls
            MainTab.Items.Refresh();
        }


        private void BtnUser2_Click(object sender, RoutedEventArgs e)
        {
            MainTab.Items.Clear(); //Clear previous Items in the user controls which is my tabItems
            var userControls = new UserControl2();
            tabUserPage = new TabItem { Content = userControls };
            MainTab.Items.Add(tabUserPage); // Add User Controls
            //MainTab.Items.Remove(BtnUser2);
            MainTab.Items.Refresh();
        }




        public static GitHubClient createGithubClient()
        {

            var client = new GitHubClient(new ProductHeaderValue("my-cool-app"));
            var tokenAuth = new Credentials(accessToken);

            client.Credentials = tokenAuth;

            return client;

        }
    }
}


//        public static void printAPILimitInfo()   //for some reason it didn't work as a method bit works if i put the code in download
//        {

//            var client = GithubApi.createGithubClient();

//            // Prior to first API call, this will be null, because it only deals with the last call.
//            var apiInfo = client.GetLastApiInfo();

//            // If the ApiInfo isn't null, there will be a property called RateLimit
//            var rateLimit = apiInfo?.RateLimit;

//            var howManyRequestsCanIMakePerHour = rateLimit?.Limit;
//            var howManyRequestsDoIHaveLeft = rateLimit?.Remaining;
//            var whenDoesTheLimitReset = rateLimit?.Reset; // UTC time

//            Console.WriteLine(howManyRequestsCanIMakePerHour);
//            Console.WriteLine(howManyRequestsDoIHaveLeft);
//            Console.WriteLine(whenDoesTheLimitReset);
//        }







