using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuitarsAndMoreApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GuitarsAndMoreApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            this.BindingContext = new HomePageViewModels();
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            HomePageViewModels vm = (HomePageViewModels)this.BindingContext;
            vm.RefreshPosts();
            App app = (App)App.Current;
            if (app.CurrentUser == null)
                vm.IsLoggedIn = "התחברות";
            else
                vm.IsLoggedIn = "התנתקות";
        }
    }
}