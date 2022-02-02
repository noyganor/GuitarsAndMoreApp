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
    class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Email
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

        #region Password
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
            if (Password.Length < 8)
            {
                PasswordError = "הסיסמה לא תקינה ";
                ShowPasswordError = true;
                this.PasswordError = ERROR_MESSAGES.REQUIRED_FIELD;
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

        public Command LoginButton => new Command(LoginAsync);
        public async void LoginAsync()
        {
            GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
            User u = await proxy.LoginAsync(Email, Password);
            if (u == null)
            {
                Message = "אחד מהנתונים שהזנת שגוי";
            }
            else
            {
                App app = (App)App.Current;
                app.CurrentUser = u;


                await app.MainPage.Navigation.PopModalAsync();
            }
        }

        public Command MoveToSignUpPage => new Command(SignUpUserPage);
        public async void SignUpUserPage()
        {
            App app = (App)App.Current;
            await app.MainPage.Navigation.PopModalAsync();
            await app.MainPage.Navigation.PushModalAsync(new SignUp());           
        }

        public Command BackToHomePageButton => new Command(BackToHomePage);
        public async void BackToHomePage()
        {
            App app = (App)App.Current;
            NavigationPage nv = (NavigationPage)app.MainPage;
            await nv.PopToRootAsync();
            MainTab mt = (MainTab) nv.CurrentPage;
            mt.SwitchToHomeTab();
            await app.MainPage.Navigation.PopModalAsync();
        }
       



    }
}
