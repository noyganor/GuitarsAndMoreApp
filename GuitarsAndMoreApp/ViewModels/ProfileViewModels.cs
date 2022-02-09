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

        #region Is Visible
        private bool isVisible;
        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }

            set
            {
                if (this.isVisible != value)
                {
                    this.isVisible = value;
                    OnPropertyChanged("IsVisible");
                }
            }
        }
        #endregion

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
            App app = ((App)App.Current);
            app.CurrentUser = null;
            await app.MainPage.Navigation.PopToRootAsync();
        }

        
    }
}
