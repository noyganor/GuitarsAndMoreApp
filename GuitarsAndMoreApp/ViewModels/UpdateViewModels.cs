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
            
                //Set default image url for non connected users
                this.UserImgSrc = app.CurrentUser.ImageUrl;
                
             //   this.UserImgSrc = app.CurrentUser.ImageUrl + $"?{r.Next()}";
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

        #region UserImgSrc
        private string userImgSrc;
        public string UserImgSrc
        {
            get => userImgSrc;
            set
            {
                userImgSrc = value;
                OnPropertyChanged("UserImgSrc");
            }
        }
        //private const string DEFAULT_PHOTO_SRC = "defaultphoto.jpg";
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

        #region Validate Form
        public bool ValidateForm()
        {
            ValidateEmail();
            ValidateNickname();
            ValidatePassword();
            ValidatePhoneNumber();

            if (ShowEmailError || ShowNicknameError || ShowPasswordError || ShowPhoneNumberError)
                return false;

            return true;
        }
        #endregion

        #region Update save data button
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
                    if (this.imageFileResult != null)
                    {
                        bool success = await proxy.UploadImage(new FileInfo()
                        {
                            Name = this.imageFileResult.FullPath
                        }, $"U{user.UserId}.jpg", false);

                        if (success)
                            await App.Current.MainPage.DisplayAlert("עדכון", "העדכון בוצע בהצלחה", "אישור", FlowDirection.RightToLeft);
                        else
                            await App.Current.MainPage.DisplayAlert("עדכון", "עדכון הנתונים בוצע בהצלחה אבל התמונה לא עודכנה נסה שנית", "אישור", FlowDirection.RightToLeft);
                    }
                    await theApp.MainPage.Navigation.PopToRootAsync();
                }
            }

            else
                Message = "אחד או יותר מהנתונים שהזנת לא תקין";
        }
        #endregion

        #region Upload Image
        FileResult imageFileResult;
        public event Action<ImageSource> SetImageSourceEvent;
        //public void SetUmageSource()
        //{
     
        //    var stream =
        //    ImageSource imgSource = ImageSource.FromStream(() => stream);
        //    if (SetImageSourceEvent != null)
        //        SetImageSourceEvent(imgSource);
        //}
        public Command PickImageCommand => new Command(OnPickImage);
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
        public Command CameraImageCommand => new Command(OnCameraImage);
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

    }

}
