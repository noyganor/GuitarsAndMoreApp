using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using GuitarsAndMoreApp.Models;
using GuitarsAndMoreApp.Views;
namespace GuitarsAndMoreApp
{
    public partial class App : Application
    {
        public User CurrentUser { get; set; }
        public List<Gender> Genders { get; set; }
        public App()
        {
            InitializeComponent();
            Genders = new List<Gender>();
            FillGenders();
            NavigationPage p = new NavigationPage(new Guitars());
            p.BarBackgroundColor = Color.White;
            MainPage = p;
            


        }

        private void FillGenders()
        {
            this.Genders.Add(new Gender()
            {
                GenderId = 2,
                Gender1 = "זכר"
            });
            this.Genders.Add(new Gender()
            {
                GenderId = 1,
                Gender1 = "נקבה"
            });
            this.Genders.Add(new Gender()
            {
                GenderId = 3,
                Gender1 = "אחר"
            });
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
