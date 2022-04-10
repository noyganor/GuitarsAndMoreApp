using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using GuitarsAndMoreApp.Models;
using GuitarsAndMoreApp.Services;
using GuitarsAndMoreApp.Views;
using Xamarin.Essentials;

namespace GuitarsAndMoreApp.ViewModels
{
    class SignUpSecondPageViewModels : INotifyPropertyChanged
    {
        private string email;
        private string nickname;
        private string password;
        private string verPassword;
        private FileResult userImg;
        public SignUpSecondPageViewModels(string email, string nickname, string password, string verPassword, FileResult userImg)
        {
            this.email = email;
            this.nickname = nickname;
            this.password = password;
            this.verPassword = verPassword;
            this.userImg = userImg;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Phone number
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
                    ValidatePhoneNumber();

                    OnPropertyChanged("PhoneNumber");
                }
            }
        }

        private string phoneNumberError;
        public string PhoneNumberError
        {
            get => phoneNumberError;
            set
            {
                phoneNumberError = value;
                OnPropertyChanged("PhoneNumberError");
            }
        }

        private bool showPhoneNumberError;
        public bool ShowPhoneNumberError
        {
            get => showPhoneNumberError;
            set
            {
                showPhoneNumberError = value;
                OnPropertyChanged("ShowPhoneNumberError");
            }
        }

        private void ValidatePhoneNumber()
        {
            this.ShowPhoneNumberError = string.IsNullOrEmpty(PhoneNumber);
            if (!this.ShowPhoneNumberError)
            {

                int num;
                bool ok = int.TryParse(PhoneNumber, out num);

                if (!ok)
                {
                    this.ShowPhoneNumberError = true;
                    this.PhoneNumberError = ERROR_MESSAGES.BAD_PHONE;
                }


                else if (this.PhoneNumber.Length != 10)
                {
                    this.ShowPhoneNumberError = true;
                    this.PhoneNumberError = ERROR_MESSAGES.BAD_PHONE_NUMBER;
                }
            }

            else
                this.PhoneNumberError = ERROR_MESSAGES.REQUIRED_FIELD;
        }
        #endregion

        #region Gender
        public Gender gender;
        public Gender Gender
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

        public List<Gender> Genders
        {
            get
            {
                App app = (App)Application.Current;
                return app.Lookup.Genders;
            }
        }

        #endregion

        #region favorite Band
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
        #endregion

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

        public bool ValidateForm()
        {
            ValidatePhoneNumber();
            if (ShowPhoneNumberError)
                return false;
            return true;
        }
        public Command SignUpSumbitButton => new Command(SignUpSubmitButton);
        public async void SignUpSubmitButton()
        {
            if (ValidateForm())
            {
                GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();

                User uu = new User
                {
                    Nickname = this.nickname,
                    Email = this.email,
                    Pass = this.password,
                    VerPassword = this.verPassword,
                    PhoneNum = this.phoneNumber,
                    GenderId = this.Gender.GenderId,
                    FavBand = this.favoriteBand
                };
                User u = await proxy.RegisterUser(uu);

                if (u == null)
                {
                    Message = "ההרשמה לא בוצעה כראוי ";
                }

                else
                {
                    App app = (App)App.Current;
                    app.CurrentUser = u;
                    if (this.userImg != null)
                    {

                        bool success = await proxy.UploadImage(new FileInfo()
                        {
                            Name = this.userImg.FullPath
                        }, $"U{u.UserId}.jpg");
                    }
                    Message = "ההרשמה בוצעה בהצלחה!";
                    NavigationPage p = new NavigationPage(new MainTab());
                    app.MainPage = p;

                }
            }
        }

        public Command BackToHomePageButton => new Command(BackToHomePage);
        public async void BackToHomePage()
        {
            App app = (App)App.Current;
            NavigationPage nv = (NavigationPage)app.MainPage;
            await nv.PopToRootAsync();
            MainTab mt = (MainTab)nv.CurrentPage;
            mt.SwitchToHomeTab();
            await app.MainPage.Navigation.PopModalAsync();
        }

    }
}
