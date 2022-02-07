﻿using System;
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
    class HomePageViewModels : INotifyPropertyChanged
    {
        public HomePageViewModels()
        {
            FullPostsList = new List<Post>();
            PostsList = new ObservableCollection<Post>();
            InitPosts();
            AddToFavButton = new Command<Post>(AddPostToFavorites);
            SelectionChanged= new Command(PostView);
            isVisible = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Search Term
        private string searchTerm;
        public string SearchTerm
        {
            get
            {
                return this.searchTerm;
            }
            set
            {
                if (this.searchTerm != value)
                {

                    this.searchTerm = value;
                    OnTextChanged(value);
                    OnPropertyChanged("SearchTerm");
                }
            }
        }
        #endregion

        #region Page Title
        private string pageTitle;
        public string PageTitle
        {
            get
            {
                return this.pageTitle;
            }

            set
            {
                if (this.pageTitle != value)
                {
                    this.pageTitle = value;
                    OnPropertyChanged("PageTitle");
                }
            }
        }
        #endregion

        #region Posts List
        private ObservableCollection<Post> postsList;
        public ObservableCollection<Post> PostsList
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

        #region Is Visible
        private bool isVisible;
        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }
            set
            {
                if (this.isVisible != value)
                {
                    this.isVisible = value;
                    OnPropertyChanged("IsVisible");
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
                    PostsList.Add(p);
                }
            }
        }

        public void GetPostsWhere(int category)
        {
            foreach (Post p in FullPostsList)
            {
                if (p.CategoryId == category && !PostsList.Contains(p))
                {
                    PostsList.Add(p);
                }
                if (p.CategoryId != category && PostsList.Contains(p))
                {
                    PostsList.Remove(p);
                }
            }
        }

        #region Search
        public void OnTextChanged(string search)
        {
            //Filter the list of posts based on the search term
            if (this.fullPostsList == null)
                return;
            if (String.IsNullOrWhiteSpace(search) || String.IsNullOrEmpty(search))
            {
                foreach (Post p in this.fullPostsList)
                {
                    if (!this.PostsList.Contains(p))
                        this.PostsList.Add(p);
                }
            }

            else
            {
                foreach (Post p in this.fullPostsList)
                {
                    string postString = $"{p.Category}|{p.Model}|{p.Price}|{p.Town}";
                    if (!this.PostsList.Contains(p) && postString.Contains(search))
                        this.PostsList.Add(p);

                    else if (this.PostsList.Contains(p) && !postString.Contains(search))
                        this.PostsList.Remove(p);
                }
            }

            this.PostsList = new ObservableCollection<Post>(this.PostsList.OrderBy(po => po.Town));
        }
        #endregion

        public ICommand AddToFavButton { get; set; }
        public async void AddPostToFavorites(Post post)
        {
            App app = (App)App.Current;

            if (app.CurrentUser == null)
            {
                await App.Current.MainPage.DisplayAlert("שגיאה", " יש להתחבר למערכת...", "אישור", FlowDirection.RightToLeft);
                await app.MainPage.Navigation.PushModalAsync(new Login());
                return;
            }

            else
            {
                GuitarsAndMoreAPIProxy cProxy = GuitarsAndMoreAPIProxy.CreateProxy();

                if (post != null)
                {
                    bool succeeded = await cProxy.AddPostToUserFavorites(post.PostId);

                    if (succeeded)
                    {
                        User user = app.CurrentUser;
                        bool found = false;
                        foreach (UserFavoritePost uf in user.UserFavoritesPosts)
                        {
                            if (post.PostId == uf.PostId)
                            {
                                user.UserFavoritesPosts.Remove(uf);
                                found = true;
                            }
                        }
                        if (!found)
                        {
                            user.UserFavoritesPosts.Add(new UserFavoritePost()
                            {
                                PostId = post.PostId,
                                UserId = user.UserId,
                                Post = post,
                                User = user
                            });
                        }
                        OnPropertyChanged("PostsList");

                    }

                    else
                    {
                        await App.Current.MainPage.DisplayAlert("שגיאה", " הפוסט לא נוסף לרשימת המועדפים שלך", "ביטול", FlowDirection.RightToLeft);
                    }
                }

                else
                    throw new Exception("הפוסט לא נמצא במאגר! נסו לרענן את העמוד");
            }
        }

        public Command MoveToUploadPostPage => new Command(UploadAPostPage);
        public void UploadAPostPage()
        {
            App app = (App)App.Current;
            app.MainPage.Navigation.PushAsync(new UploadAPost());

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
