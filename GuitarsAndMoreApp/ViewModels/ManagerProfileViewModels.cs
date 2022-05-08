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
    class ManagerProfileViewModels : INotifyPropertyChanged
    {
        public ManagerProfileViewModels()
        {
            //Set default image url for non connected users
            GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
            this.ImgUrl = $"{proxy.GetPhotoUri()}stam.jpg";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Log Out Button
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
        #endregion

        #region Manager NickName
        private string nickname;
        public string Nickname
        {
            get => this.nickname;
            set
            {
                if (this.nickname != value)
                {

                    this.nickname = value;
                    OnPropertyChanged("Nickname");
                }
            }
        }
        #endregion

        #region Image Url
        private string imgUrl;
        public string ImgUrl
        {
            get
            {
                return this.imgUrl;
            }
            set
            {
                if (this.imgUrl != value)
                {

                    this.imgUrl = value;
                    OnPropertyChanged("ImgUrl");
                }
            }
        }
        #endregion
    }
}
