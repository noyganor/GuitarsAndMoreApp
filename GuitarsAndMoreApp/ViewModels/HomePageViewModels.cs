using System;
using System.Collections.Generic;
using System.Text;
using GuitarsAndMoreApp.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using GuitarsAndMoreApp.Services;
using GuitarsAndMoreApp.Views;

namespace GuitarsAndMoreApp.ViewModels
{
    class HomePageViewModels : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //public Command PostButton => new Command(MoveToPostPage);
        //public void MoveToPostPage()
        //{
        //    App app = (App)App.Current;
        //    app.MainPage.Navigation.PushAsync(new Game());
        //}

        //public Command LoginPageGame => new Command(MoveToLoginPage);
        //public void MoveToLoginPage()
        //{
        //    App app = (App)App.Current;
        //    app.MainPage.Navigation.PushAsync(new Login());
        //}

        //public Command RegisterPageGame => new Command(MoveToRegisterPage);
        //public void MoveToRegisterPage()
        //{
        //    App app = (App)App.Current;
        //    app.MainPage.Navigation.PushAsync(new Register());
        //}

        //private string user1;
        //public string User1
        //{
        //    get
        //    {
        //        return this.user1;
        //    }
        //    set
        //    {
        //        if (this.user1 != value)
        //        {
        //            this.user1 = value;
        //            OnPropertyChanged("User1");
        //        }
        //    }
        //}
    }
}
