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

        public Command CategoryPageButoon => new Command<int>(MoveToCategoryPage);
        public void MoveToCategoryPage(int index)
        {
            App app = (App)App.Current;

            if (index == 1)
                app.MainPage.Navigation.PushAsync(new CategoryPage());
            if (index == 2)
                app.MainPage.Navigation.PushAsync(new CategoryPage());
            if (index == 3)
                app.MainPage.Navigation.PushAsync(new CategoryPage());
            if (index == 4)
                app.MainPage.Navigation.PushAsync(new CategoryPage());
        }



    }
}
