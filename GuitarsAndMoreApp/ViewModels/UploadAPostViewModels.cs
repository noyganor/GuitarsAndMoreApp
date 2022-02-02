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
using Xamarin.Essentials;
using System.Windows.Input;

namespace GuitarsAndMoreApp.ViewModels
{
    class UploadAPostViewModels : INotifyPropertyChanged
    {
        public UploadAPostViewModels()
        {
            this.SliderValue = 0;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Slider Value
        private int sliderValue;
        public int SliderValue
        {
            get
            {
                return this.sliderValue;
            }

            set
            {
                if (this.sliderValue != value)
                {
                    this.sliderValue = value;
                    OnPropertyChanged("SliderValue");
                }
            }
        }
        #endregion

        #region Town
        public Town town;
        public Town Town
        {
            get
            {
                return this.town;
            }
            set
            {
                if (this.town != value)
                {
                    this.town = value;
                    OnPropertyChanged("Town");
                }
            }
        }

        public List<Town> Towns
        {
            get
            {
                App app = (App)Application.Current;
                return app.Lookup.Towns;
            }
        }

        #endregion

        #region Producer
        public Producer producer;
        public Producer Producer
        {
            get
            {
                return this.producer;
            }
            set
            {
                if (this.producer != value)
                {
                    this.producer = value;
                    OnPropertyChanged("Producer");
                }
            }
        }

        public List<Producer> Producers
        {
            get
            {
                App app = (App)Application.Current;
                return app.Lookup.Producers;
            }
        }

        #endregion


        FileResult imageFileResult;
        public event Action<ImageSource> SetImageSourceEvent;
        public async void OnPickImage()
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

        ///The following command handle the take photo button
        public ICommand CameraImageCommand => new Command(OnCameraImage);
        public async void OnCameraImage()
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

        public async void ShowUploadAPostPage()
        {
            App app = (App)App.Current;

            if (app.CurrentUser == null)
            {
                await App.Current.MainPage.DisplayAlert("שגיאה", " יש להתחבר למערכת...", "אישור", FlowDirection.RightToLeft);
                await app.MainPage.Navigation.PushModalAsync(new Login());
                return;
            }
        }
    }
}
