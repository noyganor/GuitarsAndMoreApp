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
            Models = new ObservableCollection<Model>();
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
                    ProducerChanged();
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

        #region Model
        private Model model;
        public Model Model
        {
            get
            {
                return this.model;
            }
            set
            {
                if (this.model != value)
                {
                    this.model = value;
                    OnPropertyChanged("Model");
                }
            }
        }

        private ObservableCollection<Model> models;
        public ObservableCollection<Model> Models
        {
            get
            {
                return this.models;
            }
            set
            {
                if (this.models != value)
                {
                    this.models = value;
                    OnPropertyChanged("Models");
                }
            }
        }
        #endregion

        #region Post Description
        private string pdescription;
        public string Pdescription
        {
            get
            {
                return this.pdescription;
            }

            set
            {
                if (this.pdescription != value)
                {
                    this.pdescription = value;
                    OnPropertyChanged("Pdescription");
                }
            }
        }
        #endregion

        #region Video Link
        private string link;
        public string Link
        {
            get
            {
                return this.link;
            }
            set
            {
                if (this.link != value)
                {
                    this.link = value;
                    OnPropertyChanged("Link");
                }
            }
        }
        #endregion

        #region Phone Number
        private string phoneNum;
        public string PhoneNum
        {
            get
            {
                return this.phoneNum;
            }
            set
            {
                if (this.phoneNum != value)
                {
                    this.phoneNum = value;
                    OnPropertyChanged("PhoneNum");
                }
            }
        }
        #endregion

        private async void ProducerChanged()
        {
            GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
            List<Model> mList = await proxy.GetListOfModelsAsync();
            if (mList != null)
            {
                foreach (Model m in mList)
                {
                    if (m.ProducerId == Producer.ProducerId)
                        Models.Add(m);
                }
            }
        }

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
