using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using GuitarsAndMoreApp.Services;
using GuitarsAndMoreApp.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Essentials;
using GuitarsAndMoreApp.Views;

namespace GuitarsAndMoreApp.ViewModels
{
    class UpdateViewModels : INotifyPropertyChanged
    {
        public UpdateViewModels()
        {
            App app = (App)App.Current;
            if (app.CurrentUser != null)
            {
                Email = app.CurrentUser.Email;
                Nickname = app.CurrentUser.Nickname;
                Password = app.CurrentUser.Pass;
                PhoneNumber = app.CurrentUser.PhoneNum;
                Gender = app.CurrentUser.Gender;
                //int genderId = app.CurrentUser.GenderId;
                //FindGender(genderId);
                FavBand = app.CurrentUser.FavBand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region email
        private string email;
        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                if (this.email != value)
                {
                    this.email = value;
                    ValidateEmail();
                    OnPropertyChanged("Email");
                }
            }
        }

        private bool showEmailError;
        public bool ShowEmailError
        {
            get => showEmailError;
            set
            {
                showEmailError = value;
                OnPropertyChanged("ShowEmailError");
            }
        }

        private string emailError;
        public string EmailError
        {
            get => emailError;
            set
            {
                emailError = value;
                OnPropertyChanged("EmailError");
            }
        }

        private void ValidateEmail()
        {
            this.ShowEmailError = string.IsNullOrEmpty(Email);
            if (!this.ShowEmailError)
            {
                if (!Regex.IsMatch(this.Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                {
                    this.ShowEmailError = true;
                    this.EmailError = ERROR_MESSAGES.BAD_EMAIL;
                }
            }
            else
            {
                this.EmailError = ERROR_MESSAGES.REQUIRED_FIELD;
            }
        }
        #endregion

        #region nickname
        private string nickname;
        public string Nickname
        {
            get
            {
                return this.nickname;
            }
            set
            {
                if (this.nickname != value)
                {
                    this.nickname = value;
                    ValidateNickname();
                    OnPropertyChanged("Nickname");
                }
            }
        }

        private string nicknameError;
        public string NicknameError
        {
            get => nicknameError;
            set
            {
                nicknameError = value;
                OnPropertyChanged("NicknameError");
            }
        }

        private bool showNicknameError;
        public bool ShowNicknameError
        {
            get => showNicknameError;
            set
            {
                showNicknameError = value;
                OnPropertyChanged("ShowNicknameError");
            }
        }
        private void ValidateNickname()
        {
            this.ShowNicknameError = string.IsNullOrEmpty(Nickname);
            this.NicknameError = ERROR_MESSAGES.REQUIRED_FIELD;
        }
        #endregion

        #region password
        private string password;
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                if (this.password != value)
                {
                    this.password = value;
                    ValidatePassword();
                    OnPropertyChanged("Password");
                }
            }
        }

        private bool showPasswordError;
        public bool ShowPasswordError
        {
            get => showPasswordError;
            set
            {
                showPasswordError = value;
                OnPropertyChanged("ShowPasswordError");
            }
        }

        private string passwordError;
        public string PasswordError
        {
            get => passwordError;
            set
            {
                passwordError = value;
                OnPropertyChanged("PasswordError");
            }
        }

        private void ValidatePassword()
        {
            if (Password.Length > 0 && Password.Length < 8)
            {
                PasswordError = "הסיסמה חייבת לכלול לפחות 8 תווים";
                ShowPasswordError = true;
            }
            else if (string.IsNullOrEmpty(Password))
                this.PasswordError = ERROR_MESSAGES.REQUIRED_FIELD;
            else
                ShowPasswordError = false;

        }
        #endregion

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
        private string favBand;
        public string FavBand
        {
            get
            {
                return this.favBand;
            }
            set
            {
                if (this.favBand != value)
                {
                    this.favBand = value;
                    OnPropertyChanged("FavBand");
                }
            }
        }
        #endregion

        
        public async void FindGender(int genderId)
        {
            try
            {
                GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
                Gender.Gender1 = await proxy.FindGender(genderId);
            }

            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
          
        }


        public bool ValidateForm()
        {
            ValidateEmail();
            ValidateNickname();
            ValidatePassword();
            ValidatePhoneNumber();

            if (ShowEmailError || ShowNicknameError || ShowPasswordError)
                return false;

            return true;
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

        public Command UpdateButton => new Command(UpdateUserDetails);
        public async void UpdateUserDetails()
        {
            if (ValidateForm())
            {
                App theApp = (App)App.Current;
                    User newUser = new User()
                {
                    UserId = theApp.CurrentUser.UserId,
                    Email = theApp.CurrentUser.Email,
                    Nickname = this.Nickname,
                    GenderId = this.Gender.GenderId,
                    Gender = this.Gender,
                    PhoneNum = this.PhoneNumber,
                    FavBand = this.FavBand,
                    Pass = this.Password,

                };

                GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
                User user = await proxy.UpdateUserDetails(newUser);

                if (user == null)
                {
                    await App.Current.MainPage.DisplayAlert("שגיאה", "העדכון נכשל", "אישור", FlowDirection.RightToLeft);
                }
                else
                {
                    theApp.CurrentUser = user;
                    await App.Current.MainPage.DisplayAlert("עדכון", "העדכון בוצע בהצלחה", "אישור", FlowDirection.RightToLeft);
                    await theApp.MainPage.Navigation.PopToRootAsync();
                }
            }

            else
                Message = "אחד או יותר מהנתונים שהזנת לא תקין";
        }


    }

}
