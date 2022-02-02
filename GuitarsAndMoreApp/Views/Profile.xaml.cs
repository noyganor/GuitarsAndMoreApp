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
    public partial class Profile : ContentPage
    {
        public Profile()
        {
            this.BindingContext = new ProfileViewModels();
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ProfileViewModels vm = (ProfileViewModels)this.BindingContext;
            vm.ShowProfilePage();
        }

    }
}