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
using SkiaSharp;
using Microcharts;
using System.Windows.Input;

namespace GuitarsAndMoreApp.ViewModels
{
    class ManagerProfileViewModels : INotifyPropertyChanged
    {
        public ManagerProfileViewModels()
        {
            //Set default image url for non connected users
            GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
            this.ImgUrl = $"{proxy.GetPhotoUri()}stam.jpg";
            App app = (App)App.Current;
            Nickname = app.CurrentUser.Nickname;
            ImgUrl = app.CurrentUser.ImageUrl;
            InitChart();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Log Out Button
        public Command LogOutButton => new Command(LogOut);
        public async void LogOut()
        {
            App app = (App)App.Current;
            app.CurrentUser = null;

            await app.MainPage.Navigation.PopToRootAsync();
            NavigationPage nv = (NavigationPage)app.MainPage;
            await nv.PopToRootAsync();
            MainTab mt = (MainTab)nv.CurrentPage;
            HomePage home = (HomePage)mt.Children[0];
            HomePageViewModels vm = (HomePageViewModels)home.BindingContext;
            vm.IsVisible = false;
            mt.SwitchToHomeTab();
        }
        #endregion

        #region Manager NickName
        private string nickname;
        public string Nickname
        {
            get => this.nickname;
            set
            {
                if (this.nickname != value)
                {

                    this.nickname = value;
                    OnPropertyChanged("Nickname");
                }
            }
        }
        #endregion

        #region Image Url
        private string imgUrl;
        public string ImgUrl
        {
            get
            {
                return this.imgUrl;
            }
            set
            {
                if (this.imgUrl != value)
                {

                    this.imgUrl = value;
                    OnPropertyChanged("ImgUrl");
                }
            }
        }
        #endregion

        #region Entered Email
        private string enteredEmail;
        public string EnteredEmail
        {
            get => this.enteredEmail;
            set
            {
                if (this.enteredEmail != value)
                {
                    this.enteredEmail = value;
                    OnPropertyChanged(EnteredEmail);
                }
            }
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

        #region Count Posts
        private int countPosts;
        public int CountPosts
        {
            get => this.countPosts;
            set
            {
                if (this.countPosts != value)
                {
                    this.countPosts = value;
                    OnPropertyChanged("CountPosts");
                }
            }
        }
        #endregion

        #region Add Manager Button
        public Command AddManager => new Command(AddManagerButton);
        public async void AddManagerButton()
        {
            GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
            bool ok = await proxy.SetUserAsManager(EnteredEmail);
            if (ok)
                Message = "המשתמש עודכן כמנהל בהצלחה";
            else
                Message = "העדכון לא בוצע כראוי";
        }
        #endregion


        #region Posts List
        private List<Post> postsList;
        public List<Post> PostsList
        {
            get
            {
                return this.postsList;
            }

            set
            {
                if (this.postsList != value)
                {
                    this.postsList = value;
                    OnPropertyChanged("PostsList");
                }
            }
        }
        #endregion

        public async void ShowManagerProfilePage()
        {
            try
            {

                App app = (App)App.Current;

                if (app.CurrentUser == null || !app.CurrentUser.CheckIsManager())
                {
                    await App.Current.MainPage.DisplayAlert("שגיאה", " אינך מוגדר כמנהל...", "אישור", FlowDirection.RightToLeft);
                    await app.MainPage.Navigation.PopToRootAsync();             
                }

                else
                {
                   
                }
            }

            catch (Exception e)
            {
                Console.Write(e.Message);
            }

        }


        private Chart postByCategoryChart;
        public Chart PostByCategoryChart
        {
            get => this.postByCategoryChart;
            set
            {
                this.postByCategoryChart = value;
                OnPropertyChanged("PostByCategoryChart");
            }
        }

        public async void InitChart()
        {
            GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
            Chart chart = new PieChart();
            List<ChartEntry> chartEntries = new List<ChartEntry>();
            List<Post> lst = await proxy.GetListOfPostsAsync();

            int[] count = new int[lst.Count()];
            foreach (Post p in lst)
            {
                switch (p.CategoryId)
                {
                    case 1:
                        count[0]++;
                        break;
                    case 2:
                        count[1]++;
                        break;
                    case 3:
                        count[2]++;
                        break;
                    case 4:
                        count[3]++;
                        break;
                    default:
                        break;
                }

            }

            ChartEntry categoryone = new ChartEntry(count[0])
            {
                TextColor = SKColor.Parse("#d8e2dc"),
                ValueLabelColor = SKColor.Parse("#d8e2dc"),
                Color = SKColor.Parse("#d8e2dc"),
                Label = $" Guitars",
                ValueLabel = $"{count[0]:N0}"
            };

            ChartEntry categorytwo = new ChartEntry(count[1])
            {
                TextColor = SKColor.Parse("#fff3b0"),
                ValueLabelColor = SKColor.Parse("#fff3b0"),
                Color = SKColor.Parse("#fff3b0"),
                Label = $" Sound",
                ValueLabel = $"{count[1]:N0}"
            };

            ChartEntry categorythree = new ChartEntry(count[2])
            {
                TextColor = SKColor.Parse("#fec89a"),
                ValueLabelColor = SKColor.Parse("#fec89a"),
                Color = SKColor.Parse("#fec89a"),
                Label = $" Cases",
                ValueLabel = $"{count[2]:N0}"
            };

            ChartEntry categoryfour = new ChartEntry(count[3])
            {
                TextColor = SKColor.Parse("#e0c3fc"),              
                ValueLabelColor = SKColor.Parse("#e0c3fc"),
                Color = SKColor.Parse("#e0c3fc"),
                Label = $" Equipment",
                
                ValueLabel = $"{count[3]:N0}"
            };

            chartEntries.Add(categoryone);
            chartEntries.Add(categorytwo);
            chartEntries.Add(categorythree);
            chartEntries.Add(categoryfour);
            chart.Entries = chartEntries;

            this.PostByCategoryChart = chart;

            this.CountPosts = lst.Count();


        }

    }
}
