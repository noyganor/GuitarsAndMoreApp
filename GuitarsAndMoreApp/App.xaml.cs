using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using GuitarsAndMoreApp.Models;
using GuitarsAndMoreApp.Views;
using GuitarsAndMoreApp.Services;

namespace GuitarsAndMoreApp
{
    public partial class App : Application
    {
        public User CurrentUser { get; set; }
        public LookUpTables Lookup { get; set; }    
        //public Post Post { get; set; }
       
        public App()
        {
            InitializeComponent();
            //Loading view
            LoadingView p = new LoadingView();
            p.SetMessage("Loading data from server...");
            MainPage = p;
        }

        protected async override void OnStart()
        {
            //Read look up tables 
            GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
            this.Lookup = await proxy.GetLookupsAsync();
            
            if (this.Lookup == null)
            {
                LoadingView loadingPage = (LoadingView)MainPage;
                loadingPage.SetMessage("The server is down! Please try again later!!");
            }
            else
            {
                //Switch to home page
                NavigationPage p = new NavigationPage(new CategoryPage());
                p.BarBackgroundColor = Color.White;
                MainPage = p;
            }
            
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
