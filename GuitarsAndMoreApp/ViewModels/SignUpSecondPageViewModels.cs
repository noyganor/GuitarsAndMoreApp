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
    class SignUpSecondPageViewModels : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string phoneNumber;
        public string PhoneNumber
        {
            get
            {
                return this.phoneNumber;
            }
            set
            {
                if (this.phoneNumber != value)
                {
                    this.phoneNumber = value;
                    OnPropertyChanged("PhoneNumber");
                }
            }
        }

        private string gender;
        public string Gender
        {
            get
            {
                return this.gender;
            }
            set
            {
                if (this.gender != value)
                {
                    this.gender = value;
                    OnPropertyChanged("Gender");
                }
            }
        }

        private string favoriteBand;
        public string FavoriteBand
        {
            get
            {
                return this.favoriteBand;
            }
            set
            {
                if (this.favoriteBand != value)
                {
                    this.favoriteBand = value;
                    OnPropertyChanged("FavoriteBand");
                }
            }
        }

        private string message;
        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                if (this.message != value)
                {
                    this.message = value;
                    OnPropertyChanged("Message");
                }
            }
        }

        public Command SignUpSumbitButton => new Command(SignUpSubmitButton);

        public async void SignUpSubmitButton()
        {

            GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
            int gID = 0;
            if (Gender == "בת")
                gID = 1;
            if (Gender == "בן")
                gID = 2;
            else
                gID = 3;

            User uu = new User
            {
                NickName = this.nickname,
                Email = this.email,
                Password = this.password,
                PhoneNum = this.phoneNumber,
                GenderId = gID,
                FavBand = this.favoriteBand
            };
            bool u = await proxy.RegisterUser(uu);
            if (!u)
            {
                Message = "Registration isn't completed successfully";
            }
            else
            {
                App app = (App)App.Current;
                app.CurrentUser = uu;
                await app.MainPage.Navigation.PopModalAsync();
            }

        }
    }
}
