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
            Operate();
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
            
            bool result = await App.Current.MainPage.DisplayAlert("אתה בטוח?", "  ", "אישור", "ביטול", FlowDirection.RightToLeft);
            if (result)
            {
                GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
                bool b = await proxy.AddPostToUserFavorites(selected.PostId);
                if (b)
                {
                    FavoritePostsList.Remove(selected);
                    App app = (App)App.Current;
                    User u = app.CurrentUser;
                    UserFavoritePost ufp = u.UserFavoritePosts.Where(t => t.PostId == selected.PostId).FirstOrDefault();
                    if (ufp != null)
                        u.UserFavoritePosts.Remove(ufp);
                }
                else
                {
                    b = false;
                }
            }
        }

        #endregion

        private async void Operate()
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
                foreach (Post p in pList)
                {
                    FullPostsList.Add(p);
                    FavoritePostsList.Add(p);
                }
            }
        }

        private async Task GetFavoritePosts()
        {
            App app = (App)App.Current;
            User u = app.CurrentUser;
            ICollection<UserFavoritePost> checkList = u.UserFavoritePosts;
            FavoritePostsList.Clear();
            if (checkList != null)
            {
                foreach (Post p in FullPostsList)
                {
                    foreach (UserFavoritePost ufp in checkList)
                        if (ufp.PostId == p.PostId)
                            FavoritePostsList.Add(p);
                }
            }

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
