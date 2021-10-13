using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GuitarsAndMoreApp.Models;
using GuitarsAndMoreApp.Views;
namespace GuitarsAndMoreApp
{
    public partial class App : Application
    {
        public User CurrentUser { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Login());


        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static bool IsDevEnv
        {
            get
            {
                return true;
            }
        }


    }
}
