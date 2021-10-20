﻿using System;
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