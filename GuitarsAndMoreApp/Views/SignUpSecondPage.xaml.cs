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
    public partial class SignUpSecondPage : ContentPage
    {
        public SignUpSecondPage(string email, string nickname, string password, string verPassword)
        {
            this.BindingContext = new SignUpSecondPageViewModels(email, nickname, password, verPassword);
            InitializeComponent();
        }
    }
}