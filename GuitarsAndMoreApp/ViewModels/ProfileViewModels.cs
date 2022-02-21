using System;
using System.Collections.Generic;
using System.Text;
using GuitarsAndMoreApp.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using GuitarsAndMoreApp.Services;
using GuitarsAndMoreApp.Views;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Input;

namespace GuitarsAndMoreApp.ViewModels
{
    class ProfileViewModels : INotifyPropertyChanged
    {
        public ProfileViewModels()
        {
            //ShowProfilePage();

        }
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Command NavigateToFavoritesPage => new Command(MoveToFavoritesPage);
        public async void MoveToFavoritesPage()
        {
            App app = (App)App.Current;
            await app.MainPage.Navigation.PushAsync(new TabbedPageFavoritesAndEdit());
        }

        public Command NavigateToUpdatePage => new Command(UpdatePage);
        public async void UpdatePage()
        {
            App app = (App)App.Current;
            await app.MainPage.Navigation.PushAsync(new Update());
        }
        public Command NavigateToEditMyPostsPage => new Command(EditMyPostsPage);
        public async void EditMyPostsPage()
        {
            App app = (App)App.Current;
            await app.MainPage.Navigation.PushAsync(new TabbedPageFavoritesAndEdit(1));
        }
        public Command NavigateToUploadAPostPage => new Command(UploadAPostPage);
        public async void UploadAPostPage()
        {
            App app = (App)App.Current;
            await app.MainPage.Navigation.PushAsync(new UploadAPost());
        }

        public async void ShowProfilePage()
        {
            App app = (App)App.Current;

            if (app.CurrentUser == null)
            {
                await App.Current.MainPage.DisplayAlert("שגיאה", " יש להתחבר למערכת...", "אישור", FlowDirection.RightToLeft);
                await app.MainPage.Navigation.PushModalAsync(new Login());
                return;
            }
        }

        public Command LogOutButton => new Command(LogOut);
        public async void LogOut()
        {
            App app = (App)App.Current;
            app.CurrentUser = null;
          
            await app.MainPage.Navigation.PopToRootAsync();
            NavigationPage nv = (NavigationPage)app.MainPage;
            await nv.PopToRootAsync();
            MainTab mt = (MainTab)nv.CurrentPage;
            HomePage home = (HomePage)mt.Children[0];
            HomePageViewModels vm = (HomePageViewModels)home.BindingContext;
            vm.IsVisible = false;
            mt.SwitchToHomeTab();
        }

        //public Command NavigateToFavoritesPageCommand => new Command(FavoritesPage);
        //public async void FavoritesPage()
        //{
        //    App app = (App)App.Current;
        //    await app.MainPage.Navigation.PushAsync(new FavoritesPage());
        //}

    }
}
