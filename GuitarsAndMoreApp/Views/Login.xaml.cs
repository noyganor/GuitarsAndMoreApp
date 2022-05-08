using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuitarsAndMoreApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GuitarsAndMoreApp.ViewModels;

namespace GuitarsAndMoreApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            this.BindingContext = new LoginViewModel();
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            this.BindingContext = BindingContext;
        }
    }
}