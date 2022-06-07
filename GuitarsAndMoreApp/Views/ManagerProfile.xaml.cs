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
    public partial class ManagerProfile : ContentPage
    {
        public ManagerProfile()
        {
            this.BindingContext = new ManagerProfileViewModels();
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
          
            ManagerProfileViewModels vm = (ManagerProfileViewModels)this.BindingContext;
            vm.ShowManagerProfilePage();
            //base.OnAppearing();
        }
    }
}