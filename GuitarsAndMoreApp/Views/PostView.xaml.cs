using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuitarsAndMoreApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using GuitarsAndMoreApp.Models;

namespace GuitarsAndMoreApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostView : ContentPage
    {
        public PostView(Post p)
        {
            this.BindingContext = new PostViewViewModels(p);
            InitializeComponent();
           
        }

        private void Button_Clicked(string phoneNumber)
        {
            try
            {
                PhoneDialer.Open(phoneNumber);
            }
            catch(Exception e)
            {
                DisplayAlert("Unable to make call", "Please enter a number", "OK");
            }
           
        }
    }
}