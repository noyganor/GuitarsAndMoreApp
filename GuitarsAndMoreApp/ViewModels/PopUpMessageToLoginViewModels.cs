using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using GuitarsAndMoreApp.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using GuitarsAndMoreApp.Services;
using GuitarsAndMoreApp.Views;

namespace GuitarsAndMoreApp.ViewModels
{
    class PopUpMessageToLoginViewModels : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Command MoveToLoginPageButton => new Command(LoginPage);
        public async void LoginPage()
        {
            App app = (App)App.Current;
            app.MainPage.Navigation.PushAsync(new Login());
        }

        public Command MoveToSignUpPageButton => new Command(SignUpUserPage);
        public async void SignUpUserPage()
        {
            App app = (App)App.Current;
            app.MainPage.Navigation.PushAsync(new SignUp());
        }
    }
}
