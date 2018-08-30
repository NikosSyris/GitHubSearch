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







//namespace GitHub_Tool
//{

//    public class GithubApi
//    {

//        static String acceessToken;

//        public static void Main(string[] args)
//        {
//            Console.WriteLine("Hello World!");

//            acceessToken = args[0];

//            Search search = new Search();
//            Commit commit = new Commit();
//            Download download = new Download();

//            //search.SearchRepositories();
//            var file = search.searchCode(term: " var apiInfo = client.GetLastApiInfo(); ");
//            //todo When the type of a variable is not clear from the context, use an explicit type.

//            //search.searchUsers();


//            //Think of the Task<SearchCode> return type as a 'promise' to return a value in the future.
//            //to get the value of the parameter use: parameter.result
//            var fileInformation = commit.getAllCommitsThatChangedAFile(file.Result);
//            // commit.getAllCommits();
//            // commit.downloadCommits();



//            download.downloadContent(fileInformation.Result);
//            //download.createFolder("test1","test2.cs");





//            //GetUserInfo();
//            //APILImits();


//            // var commitNumber = CountCommitsSimpleTest();
//            // Console.WriteLine(commitNumber);




//            // Keep the console window open in debug mode.
//            Console.WriteLine("Press any key to exit.");
//            Console.ReadKey();
//        }













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




//        public async static Task GetUserInfo()
//        {

//            var client = GithubApi.createGithubClient();

//            var user = await client.User.Get("shiftkey");
//            Console.WriteLine("{0} has {1} public repositories - go check out their profile at {2}",
//            user.Name,
//            user.PublicRepos,
//            user.Url);


//            var myProfile = await client.User.Current();
//            Console.WriteLine("{0} has {1} public repositories - go check out their profile at {2}",
//            myProfile.Name,
//            myProfile.PublicRepos,
//            myProfile.Url);

//        }





