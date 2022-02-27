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
            InitPosts();
            GetFavoritePosts();
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
        public Command DeleteButton => new Command<int>(DeleteFromFavorites);
        public async void DeleteFromFavorites(int selected)
        {
            GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
            bool b = await proxy.AddPostToUserFavorites(selected);
            if (b)
            {
                FavoritePostsList.Remove(SelectedPost);
                
            }
        }

        #endregion

        private async void InitPosts()
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

        private void GetFavoritePosts()
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

    }
}
