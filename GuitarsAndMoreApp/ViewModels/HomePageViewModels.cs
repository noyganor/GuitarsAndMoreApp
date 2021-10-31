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

        public Command GuitarsPageButoon => new Command(MoveToGuitarsPage);
        public void MoveToGuitarsPage()
        {
            App app = (App)App.Current;
            app.MainPage.Navigation.PushAsync(new Guitars());
        }

        public Command SoundPageButton => new Command(MoveToSoundPage);
        public void MoveToSoundPage()
        {
            App app = (App)App.Current;
            app.MainPage.Navigation.PushAsync(new Sound());
        }

        public Command AccessoriesPageButton => new Command(MoveToAccessoriesPage);
        public void MoveToAccessoriesPage()
        {
            App app = (App)App.Current;
            app.MainPage.Navigation.PushAsync(new Accessories());
        }

        public Command CasesPageButton => new Command(MoveToCasesPage);
        public void MoveToCasesPage()
        {
            App app = (App)App.Current;
            app.MainPage.Navigation.PushAsync(new Cases());
        }

    }
}
