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
using Xamarin.Essentials;

namespace GuitarsAndMoreApp.ViewModels
{
    class EditViewModels : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //public EditViewModels(Post p)
        //{
        //    App app = (App)App.Current;
        //    if (app.CurrentUser != null)
        //    {
        //        UserId = app.CurrentUser.UserId,
        //            Price = app.CurrentUser.Posts.Where(x => x.PostId == p.PostId).Select(SliderValue)
        //            Pdescription = app.CurrentUser.Pdescription,
        //            PhoneNum = app.CurrentUser.PhoneNum,
        //            ModelId = app.CurrentUser.Model?.ModelId, 
        //            TownId = app.CurrentUser.Town.TownId, 
        //            ProducerId = app.CurrentUser.Producer?.ProducerId, 
        //            CategoryId = app.CurrentUser.Category.CategoryId,
        //            Link = app.CurrentUser.Link ,
        //    }
        //}
            

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

        private async void ProducerChanged()
        {
            GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
            List<Model> mList = await proxy.GetListOfModelsAsync();
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

        public Command SaveDataCommand => new Command(SaveData);
        public async void SaveData()
        {
            if (ValidateForm())
            {
                App app = (App)App.Current;
                if (ShowPriceError)
                {
                    await App.Current.MainPage.DisplayAlert("שים לב!", "  המוצר שהינך עומד לפרסם מיועד לתרומה", "אישור", FlowDirection.RightToLeft);
                }
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
}
