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
        private const string Source1 = "arrowdown.png";
        private const string Source2 = "arrowup.png";
        public UploadAPostViewModels()
        {
            this.SliderValue = 0;
            Models = new ObservableCollection<Model>();
            Button1 = false;
            Button2 = false;
            ImgSource1 = Source1;
            ImgSource2 = Source1;
            Button1PressedCommand = new Command(Button1Pressed);
            Button2PressedCommand = new Command(Button2Pressed);
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
                    ValidatePrice();
                    OnPropertyChanged("SliderValue");
                }
            }
        }

        private string priceError;
        public string PriceError
        {
            get => priceError;
            set
            {
                priceError = value;
                OnPropertyChanged("PriceError");
            }
        }

        private bool showPriceError;
        public bool ShowPriceError
        {
            get => showPriceError;
            set
            {
                showPriceError = value;
                OnPropertyChanged("ShowPriceError");
            }
        }
        private void ValidatePrice()
        {
            this.ShowPriceError = SliderValue.Equals(0);
            this.PriceError = "שים לב! הפריט מיועד לתרומה";
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
                    ValidateTown();
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

        private string townError;
        public string TownError
        {
            get => townError;
            set
            {
                townError = value;
                OnPropertyChanged("TownError");
            }
        }

        private bool showTownError;
        public bool ShowTownError
        {
            get => showTownError;
            set
            {
                showTownError = value;
                OnPropertyChanged("ShowTownError");
            }
        }
        private void ValidateTown()
        {

            this.ShowTownError = Town == null;
            this.TownError = ERROR_MESSAGES.REQUIRED_FIELD;
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
                    ValidateDescription();
                    OnPropertyChanged("Pdescription");
                }
            }
        }

        private string descriptionError;
        public string DescriptionError
        {
            get => descriptionError;
            set
            {
                descriptionError = value;
                OnPropertyChanged("DescriptionError");
            }
        }

        private bool showDescriptionError;
        public bool ShowDescriptionError
        {
            get => showDescriptionError;
            set
            {
                showDescriptionError = value;
                OnPropertyChanged("ShowDescriptionError");
            }
        }
        private void ValidateDescription()
        {

            this.ShowDescriptionError = string.IsNullOrEmpty(Pdescription);
            this.DescriptionError = ERROR_MESSAGES.REQUIRED_FIELD;
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
                    ValidatePhoneNumber();
                    OnPropertyChanged("PhoneNum");
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
            this.ShowPhoneNumberError = string.IsNullOrEmpty(PhoneNum);
            if (!this.ShowPhoneNumberError)
            {

                int num;
                bool ok = int.TryParse(PhoneNum, out num);

                if (!ok)
                {
                    this.ShowPhoneNumberError = true;
                    this.PhoneNumberError = ERROR_MESSAGES.BAD_PHONE;
                }


                else if (this.PhoneNum.Length != 10)
                {
                    this.ShowPhoneNumberError = true;
                    this.PhoneNumberError = ERROR_MESSAGES.BAD_PHONE_NUMBER;
                }
            }

            else
                this.PhoneNumberError = ERROR_MESSAGES.REQUIRED_FIELD;

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

        #region Category
        public Category category;
        public Category Category
        {
            get
            {
                return this.category;
            }
            set
            {
                if (this.category != value)
                {
                    this.category = value;
                    ValidateCategory();
                    OnPropertyChanged("Category");
                }
            }
        }

        public List<Category> Categories
        {
            get
            {
                App app = (App)Application.Current;
                return app.Lookup.Categories;
            }
        }

        private string categoryError;
        public string CategoryError
        {
            get => categoryError;
            set
            {
                categoryError = value;
                OnPropertyChanged("CategoryError");
            }
        }

        private bool showCategoryError;
        public bool ShowCategoryError
        {
            get => showCategoryError;
            set
            {
                showCategoryError = value;
                OnPropertyChanged("ShowCategoryError");
            }
        }
        private void ValidateCategory()
        {

            this.ShowCategoryError = Category == null;
            this.CategoryError = ERROR_MESSAGES.REQUIRED_FIELD;
        }

        #endregion

        private void ProducerChanged()
        {
            App app = (App)Application.Current;
            //GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();

            //List<Model> mList = await proxy.GetListOfModelsAsync();
            List<Model> mList = app.Lookup.Models;
            if (mList != null)
            {
                Models.Clear();

                foreach (Model m in mList)
                {
                    if (m.ProducerId == Producer.ProducerId)
                        Models.Add(m);
                }
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

        public async void ShowUploadAPostPage()
        {
            App app = (App)App.Current;

            if (app.CurrentUser == null)
            {
                bool result = await App.Current.MainPage.DisplayAlert("שגיאה", " יש להתחבר למערכת...", "אישור", "ביטול", FlowDirection.RightToLeft);
                if (result)
                    await app.MainPage.Navigation.PushModalAsync(new Login());
                else
                    await app.MainPage.Navigation.PopToRootAsync();
                return;
            }
        }


        #region Save Data Button + Validation
        private bool ValidateForm()
        {
            ValidatePrice(); //make it possible
            ValidateTown();
            ValidatePhoneNumber();
            ValidateDescription();
            ValidateCategory();

            //Include validate town
            if (ShowCategoryError || ShowDescriptionError || ShowPhoneNumberError || ShowTownError)
                return false;
            else
                return true;
        }

      
        public Command SaveDataCommand => new Command(SaveData);
        public async void SaveData()
        {
            if (ValidateForm())
            {
                App app = (App)App.Current;
                if (ShowPriceError)
                {
                    bool result = await App.Current.MainPage.DisplayAlert("שים לב!", "  המוצר שהינך עומד לפרסם מיועד לתרומה", "אישור", "ביטול", FlowDirection.RightToLeft);
                    if (result)
                    {
                        GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();

                        Post p = new Post
                        {
                            UserId = app.CurrentUser.UserId,
                            Price = this.SliderValue,
                            Pdescription = this.Pdescription,
                            PhoneNum = this.PhoneNum,
                            ModelId = this.Model?.ModelId,
                            TownId = this.Town.TownId,
                            ProducerId = this.Producer?.ProducerId,
                            CategoryId = this.Category.CategoryId,
                            Link = this.Link,
                        };

                        Post newPost = await proxy.AddPost(p);
                        if (newPost == null)
                        {
                            Message = "המודעה לא עלתה";
                        }

                        else
                        {
                            //Upload image to server
                            if (this.imageFileResult != null)
                            {

                                bool success = await proxy.UploadImage(new FileInfo()
                                {
                                    Name = this.imageFileResult.FullPath
                                }, $"{newPost.PostId}.jpg");
                            }
                            Message = "המודעה הועלתה בהצלחה!";

                            Page page = new HomePage();
                            await app.MainPage.Navigation.PopToRootAsync();
                        }
                    }              
                }

                else
                {
                    GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();

                    Post p = new Post
                    {
                        UserId = app.CurrentUser.UserId,
                        Price = this.SliderValue,
                        Pdescription = this.Pdescription,
                        PhoneNum = this.PhoneNum,
                        ModelId = this.Model?.ModelId,
                        TownId = this.Town.TownId,
                        ProducerId = this.Producer?.ProducerId,
                        CategoryId = this.Category.CategoryId,
                        Link = this.Link,
                    };

                    Post newPost = await proxy.AddPost(p);
                    if (newPost == null)
                    {
                        Message = "המודעה לא עלתה";
                    }

                    else
                    {
                        //Upload image to server
                        if (this.imageFileResult != null)
                        {

                            bool success = await proxy.UploadImage(new FileInfo()
                            {
                                Name = this.imageFileResult.FullPath
                            }, $"{newPost.PostId}.jpg");
                        }
                        Message = "המודעה הועלתה בהצלחה!";

                        Page page = new HomePage();
                        await app.MainPage.Navigation.PopToRootAsync();
                    }
                }

            }

        }
        #endregion

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

