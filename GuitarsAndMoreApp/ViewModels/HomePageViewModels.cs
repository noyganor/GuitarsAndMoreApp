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
    class HomePageViewModels : INotifyPropertyChanged
    {
        public HomePageViewModels()
        {
            FullPostsList = new List<Post>();
            PostsList = new ObservableCollection<Post>();
            SearchTerm = string.Empty;
            InitPosts();
            AddToFavButton = new Command<Post>(AddPostToFavorites);
            SelectionChanged = new Command(PostView);
            SearchCommand = new Command<string>(OnTextChanged);
            App app = (App)App.Current;
            if (app.CurrentUser == null)
                IsLoggedIn = "התחברות";
            else
                IsLoggedIn = "התנתקות";


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
        public bool IsVisible
        {
            get
            {
                App theApp = (App)Application.Current;
                if (theApp.CurrentUser == null)
                    return false;
                else
                    return true;
            }
            set
            {
                OnPropertyChanged("IsVisible");
            }
        }
        #endregion

        #region Is Logged In
        private string isLoggedIn;
        public string IsLoggedIn
        {
            get => isLoggedIn;
            set
            {
                if (this.isLoggedIn != value)
                {
                    this.isLoggedIn = value;
                    OnPropertyChanged("IsLoggedIn");
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
        public ICommand SearchCommand { get; }
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
                this.postsList.Clear();
                foreach (Post p in this.fullPostsList)
                {

                    string postString = $"{p.Category.Category1}|{p.Model?.ModelName}|{p.Price}|{p.Town.Town1}|{p.Pdescription}|{p.Producer?.Producer1}";
                    if (!this.PostsList.Contains(p) && postString.Contains(search))
                        this.PostsList.Add(p);

                    else if (this.PostsList.Contains(p) && !postString.Contains(search))
                        this.PostsList.Remove(p);
                }
            }

            this.PostsList = new ObservableCollection<Post>(this.PostsList.OrderBy(po => po.Town.Town1));
        }
        #endregion

        #region Favorite Button
        public ICommand AddToFavButton { get; set; }
        public async void AddPostToFavorites(Post post)
        {
            bool fromDelete = true;
            App app = (App)App.Current;
            Page current = ((TabbedPage)app.MainPage.Navigation.NavigationStack.Last()).CurrentPage;
            if (current != null && current is HomePage || current is Favorites)
                fromDelete = false;
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
                        UserFavoritePost foundedFavorite = null;
                        foreach (UserFavoritePost uf in user.UserFavoritePosts)
                        {
                            if (post.PostId == uf.PostId && uf.UserId == user.UserId)
                            {
                                foundedFavorite = uf;
                                found = true;
                            }
                        }
                        if (found)
                            user.UserFavoritePosts.Remove(foundedFavorite);
                        else
                        {
                            user.UserFavoritePosts.Add(new UserFavoritePost()
                            {
                                PostId = post.PostId,
                                UserId = user.UserId,
                                Post = post,
                                User = user
                            });
                        }
                        RefreshPosts();


                    }

                    else if (!fromDelete)
                    {
                        await App.Current.MainPage.DisplayAlert("שגיאה", " הפוסט לא נוסף לרשימת המועדפים שלך", "ביטול", FlowDirection.RightToLeft);
                    }
                }

                else
                    throw new Exception("הפוסט לא נמצא במאגר! נסו לרענן את העמוד");
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

        #region Go To Upload A Post Page
        public Command MoveToUploadPostPage => new Command(UploadAPostPage);
        public void UploadAPostPage()
        {
            App app = (App)App.Current;
            app.MainPage.Navigation.PushAsync(new UploadAPost());

        }
        #endregion

        #region Go To Login Page
        public Command MoveToLoginPage => new Command(GoToLogin);
        public async void GoToLogin()
        {
            App app = (App)App.Current;

            if (app.CurrentUser == null)
            {
                await app.MainPage.Navigation.PushModalAsync(new Login());
                IsLoggedIn = "התנתקות";
                // return;
            }

            else
            {
                bool result = await App.Current.MainPage.DisplayAlert("הינך מחובר", " האם ברצונך להתנתק?", "אישור", "ביטול", FlowDirection.RightToLeft);
                if (result)
                {
                    app.CurrentUser = null;
                    await app.MainPage.Navigation.PopToRootAsync();
                    NavigationPage nv = (NavigationPage)app.MainPage;
                    await nv.PopToRootAsync();
                    MainTab mt = (MainTab)nv.CurrentPage;
                    HomePage home = (HomePage)mt.Children[0];
                    HomePageViewModels vm = (HomePageViewModels)home.BindingContext;
                    vm.IsVisible = false;
                    vm.IsLoggedIn = "התחברות";
                    mt.SwitchToHomeTab();
                    RefreshPosts();
                }
            }
        }
        #endregion

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


        public async void RefreshPosts()
        {
            GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
            List<Post> pList = await proxy.GetListOfPostsAsync();
            if (pList != null)
            {
                FullPostsList.Clear();
                PostsList.Clear();
                foreach (Post p in pList)
                {
                    FullPostsList.Add(p);
                    PostsList.Add(p);
                }
            }
        }

    }
}
