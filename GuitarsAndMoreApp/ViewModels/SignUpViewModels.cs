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
    public static class ERROR_MESSAGES
    {
        public const string REQUIRED_FIELD = "זהו שדה חובה";
        public const string BAD_EMAIL = "מייל לא תקין";
        public const string BAD_PHONE = "טלפון לא תקין";
        public const string BAD_PHONE_NUMBER = "מספר הטלפון חייב להכיל 10 ספרות";
    }

    class SignUpViewModels : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignUpViewModels()
        {
            Button1 = false;
            Button2 = false;
            ImgSource1 = Source1;
            ImgSource2 = Source1;
            Button1PressedCommand = new Command(Button1Pressed);
            Button2PressedCommand = new Command(Button2Pressed);
        }

        private const string Source1 = "outline_favorite_border_black_24dp.png";
        private const string Source2 = "fullHeart.png";        

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
                    ValidateForm();
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
                    ValidateForm();
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
                    ValidateForm();
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

        #region verPassword
        private string verPassword;
        public string VerPassword
        {
            get
            {
                return this.verPassword;
            }
            set
            {
                if (this.verPassword != value)
                {
                    this.verPassword = value;
                    ValidateVerPassword();
                    ValidateForm();
                    OnPropertyChanged("VerPassword");
                }
            }
        }

        private bool showVerPasswordError;
        public bool ShowVerPasswordError
        {
            get => showVerPasswordError;
            set
            {
                showVerPasswordError = value;
                OnPropertyChanged("ShowVerPasswordError");
            }
        }

        private string verPasswordError;
        public string VerPasswordError
        {
            get => verPasswordError;
            set
            {
                verPasswordError = value;
                OnPropertyChanged("VerPasswordError");
            }
        }


        private void ValidateVerPassword()
        {
            if (VerPassword != Password)
            {
                VerPasswordError = "הסיסמאות חייבות להיות תואמות";
                ShowVerPasswordError = true;
            }
            else if (string.IsNullOrEmpty(VerPassword))
                this.VerPasswordError = ERROR_MESSAGES.REQUIRED_FIELD;
            else
                ShowVerPasswordError = false;


        }
        #endregion

        #region Message
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
        #endregion

        #region Is Enable
        private bool isEnable;
        public bool IsEnable
        {
            get
            {
                return this.isEnable;
            }
            set
            {
                if (this.isEnable != value)
                {
                    this.isEnable = value;
                    OnPropertyChanged("IsEnable");
                }
            }
        }
        #endregion

        public Command SignUpNextButton => new Command(SignUpNextPage);

        public void ValidateForm()
        {
            if (ShowEmailError || ShowNicknameError || ShowPasswordError || ShowVerPasswordError)
            {
                this.IsEnable = false;
            }

            else
            {
                this.IsEnable = true;

            }
        }

        #region Upload Image
        FileResult imageFileResult;
        public event Action<ImageSource> SetImageSourceEvent;
        public ICommand PickImageCommand => new Command(OnPickImage);
        public async void OnPickImage()
        {
            try
            {
                FileResult result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions()
                {
                    Title = "בחר תמונה"
                });

                if (result != null)
                {
                    this.imageFileResult = result;

                    var stream = await result.OpenReadAsync();
                    ImageSource imgSource = ImageSource.FromStream(() => stream);
                    if (SetImageSourceEvent != null)
                        SetImageSourceEvent(imgSource);
                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("שגיאה", " לא ניתן לפתוח את הגלריה ...", "אישור", FlowDirection.RightToLeft);
            }

        }

        ///The following command handle the take photo button
        public ICommand CameraImageCommand => new Command(OnCameraImage);
        public async void OnCameraImage()
        {
            try
            {
                var result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions()
                {
                    Title = "צלם תמונה"
                });

                if (result != null)
                {
                    this.imageFileResult = result;
                    var stream = await result.OpenReadAsync();
                    ImageSource imgSource = ImageSource.FromStream(() => stream);
                    if (SetImageSourceEvent != null)
                        SetImageSourceEvent(imgSource);
                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("שגיאה", " לא ניתן לפתוח את המצלמה ...", "אישור", FlowDirection.RightToLeft);
            }
        }
        #endregion

        public void SignUpNextPage()
        {
            App app = (App)App.Current;
            app.MainPage = new SignUpSecondPage(Email, Nickname, Password, VerPassword, this.imageFileResult);
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


        #region ButtonPressed
        private bool button1;
        public bool Button1
        {
            get { return this.button1; }

            set
            {
                if (this.button1 != value)
                {
                    this.button1 = value;
                    OnPropertyChanged(nameof(Button1));
                }
            }
        }

        private bool button2;
        public bool Button2
        {
            get { return this.button2; }

            set
            {
                if (this.button2 != value)
                {
                    this.button2 = value;
                    OnPropertyChanged(nameof(Button2));
                }
            }
        }
        #endregion

        #region imgChange
        private string imgSource1;
        public string ImgSource1
        {
            get { return this.imgSource1; }

            set
            {
                if (this.imgSource1 != value)
                {
                    this.imgSource1 = value;
                    OnPropertyChanged(nameof(ImgSource1));
                }
            }
        }

        private string imgSource2;
        public string ImgSource2
        {
            get { return this.imgSource2; }

            set
            {
                if (this.imgSource2 != value)
                {
                    this.imgSource2 = value;
                    OnPropertyChanged(nameof(ImgSource2));
                }
            }
        }
        #endregion


        #region
        public ICommand Button1PressedCommand { protected set; get; }
        public void Button1Pressed()
        {
            if (Button1 == false) { Button1 = true; }
            else { Button1 = false; }

            if (ImgSource1 == Source1) { ImgSource1 = Source2; }
            else { ImgSource1 = Source1; }
        }

        public ICommand Button2PressedCommand { protected set; get; }
        public void Button2Pressed()
        {
            if (Button2 == false) { Button2 = true; }
            else { Button2 = false; }

            if (ImgSource2 == Source1) { ImgSource2 = Source2; }
            else { ImgSource2 = Source1; }
        }
        #endregion
    }
}