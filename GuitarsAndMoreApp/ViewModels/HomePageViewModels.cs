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

namespace GuitarsAndMoreApp.ViewModels
{
    class HomePageViewModels : INotifyPropertyChanged
    {
        public HomePageViewModels()
        {
            FullPostsList = new List<Post>();
            PostsList = new ObservableCollection<Post>();
            InitPosts();
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

        //#region Image Url 
        //private string imageUrl;
        //public string ImageUrl
        //{
        //    get
        //    {
        //        return this.imageUrl;
        //    }

        //    set
        //    {
        //        if (this.imageUrl != value)
        //        {
        //            this.imageUrl = value;
        //            OnPropertyChanged("ImageUrl");
        //        }
        //    }
        //}
        //#endregion

        //#region Post Description
        //private string pDescription;
        //public string PDescription
        //{
        //    get
        //    {
        //        return this.pDescription;
        //    }

        //    set
        //    {
        //        if (this.pDescription != value)
        //        {
        //            this.pDescription = value;
        //            OnPropertyChanged("PDescription");
        //        }
        //    }
        //}
        //#endregion

        //#region Post Town
        //private Town pTown;
        //public Town PTown
        //{
        //    get
        //    {
        //        return this.pTown;
        //    }

        //    set
        //    {
        //        if (this.pTown != value)
        //        {
        //            this.pTown = value;
        //            OnPropertyChanged("PTown");
        //        }
        //    }
        //}
        //#endregion

        //#region Post Price
        //private double pPrice;
        //public double PPrice
        //{
        //    get
        //    {
        //        return this.pPrice;
        //    }

        //    set
        //    {
        //        if (this.pPrice != value)
        //        {
        //            this.pPrice = value;
        //            OnPropertyChanged("PPrice");
        //        }
        //    }
        //}
        //#endregion

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

        public Command AddToFavButton => new Command<int>(AddPostToFavorites);
        public async void AddPostToFavorites(int postID)
        {
            App app = (App)App.Current;

            if (app.CurrentUser == null)
            {
                await app.MainPage.Navigation.PushModalAsync(new PopUpMessageToLogin());   
            }

            else
            {
                GuitarsAndMoreAPIProxy cProxy = GuitarsAndMoreAPIProxy.CreateProxy();
                List<Post> lPost = await cProxy.GetListOfPostsAsync();

                Post addPost = null;
                foreach (Post p in lPost)
                {
                    if (p.PostId == postID)
                    {
                        addPost = new Post(p);
                        return;
                    }
                }

                if (addPost != null)
                {
                    bool succeeded = await cProxy.AddPostToUserFavorites(addPost);

                    if (succeeded)
                    {
                        //Color.Black;
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

        public Command CategoryPageButton => new Command<int>(MoveToCategoryPage);
        public void MoveToCategoryPage(int index)
        {
            App app = (App)App.Current;
            app.MainPage.Navigation.PushAsync(new CategoryPage(index));

        }



    }
}
