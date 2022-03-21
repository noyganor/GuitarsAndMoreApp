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


namespace GuitarsAndMoreApp.ViewModels
{
    class FavoritesViewModels : INotifyPropertyChanged
    {
        public FavoritesViewModels()
        {
            FullPostsList = new List<Post>();
            FavoritePostsList = new ObservableCollection<Post>();
            SelectionChanged = new Command(PostView);         
            //Operate(); => we dropped this call because it is being fired from the OnAppearing of the page
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Full Posts List
        private List<Post> fullPostsList;
        public List<Post> FullPostsList
        {
            get
            {
                return this.fullPostsList;
            }

            set
            {
                if (this.fullPostsList != value)
                {
                    this.fullPostsList = value;
                    OnPropertyChanged("FullPostsList");
                }
            }
        }
        #endregion

        #region Favorite Posts List
        private ObservableCollection<Post> favoritePostsList;
        public ObservableCollection<Post> FavoritePostsList
        {
            get
            {
                return this.favoritePostsList;
            }

            set
            {
                if (this.favoritePostsList != value)
                {
                    this.favoritePostsList = value;
                    OnPropertyChanged("FavoritePostsList");
                }
            }
        }
        #endregion

        #region Selected Post
        private object selectedPost;
        public Post SelectedPost
        {
            get => (Post)selectedPost;
            set
            {
                if (this.selectedPost != value)
                {
                    this.selectedPost = value;
                    OnPropertyChanged("SelectedPost");
                }
            }
        }
        #endregion

        #region Delete Button
        public Command DeleteButton => new Command<Post>(DeleteFromFavorites);
        public async void DeleteFromFavorites(Post selected)
        {

            bool result = await App.Current.MainPage.DisplayAlert("אתה בטוח?", null, "אישור", "ביטול", FlowDirection.RightToLeft);
            if (result)
            {
                
                HomePageViewModels hp = new HomePageViewModels();
                hp.AddPostToFavorites(selected);
                GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
                bool b = await proxy.AddPostToUserFavorites(selected.PostId);
                if (b)
                {
                    FavoritePostsList.Remove(selected);
                    App app = (App)App.Current;
                    User u = app.CurrentUser;
                    UserFavoritePost ufp = u.UserFavoritePosts.Where(t => t.PostId == selected.PostId).FirstOrDefault();
                    if (ufp != null)
                    {
                        u.UserFavoritePosts.Remove(ufp);
                        Message = "אין לך מודעות במועדפים";
                    }
                }
                else
                {
                    b = false;
                }
            }
        }

        #endregion

        #region Logo Command
        public Command LogoCommand => new Command(Logo);
        public async void Logo()
        {
            App app = (App)App.Current;
            await app.MainPage.Navigation.PopToRootAsync();
            NavigationPage nv = (NavigationPage)app.MainPage;
            await nv.PopToRootAsync();
            MainTab mt = (MainTab)nv.CurrentPage;
            mt.SwitchToHomeTab();
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

        public  async void Operate()
        {
            await InitPosts();
            await GetFavoritePosts();
        }
        private async Task InitPosts()
        {
            GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
            List<Post> pList = await proxy.GetListOfPostsAsync();
            if (pList != null)
            {
                FullPostsList.Clear();
                foreach (Post p in pList)
                {
                    FullPostsList.Add(p);
                }
            }
        }

        private async Task GetFavoritePosts()
        {
            App app = (App)App.Current;
            User u = app.CurrentUser;
            ICollection<UserFavoritePost> checkList = u.UserFavoritePosts;
            FavoritePostsList.Clear();
            if (checkList.Count() > 0)
            {
                foreach (Post p in FullPostsList)
                {
                    foreach (UserFavoritePost ufp in checkList)
                        if (ufp.PostId == p.PostId)
                            FavoritePostsList.Add(p);
                }
            }
            if(checkList.Count() == 0)
                Message = "אין לך מודעות במועדפים";


        }

        public ICommand SelectionChanged { get; set; }
        public void PostView()
        {
            if (SelectedPost != null)
            {
                App app = (App)App.Current;
                app.MainPage.Navigation.PushAsync(new PostView(SelectedPost));
                SelectedPost = null;
            }
        }

    }
}
