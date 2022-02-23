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

        #region Posts List
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
     
        private async void GetFavoritePosts()
        {
            App app = (App)App.Current;
            app.Lookup.
            User u = app.CurrentUser;
            List<UserFavoritePost> favoritesList = new List<UserFavoritePost>();

            foreach(UserFavoritePost ufp in UserFavoritePost)
            {

            }

            foreach(Post p in FullPostsList)
            {
                foreach(UserFavoritePost ufp in )
                if(p.PostId == u.UserFavoritePosts.)
            }
        }

    }
}
