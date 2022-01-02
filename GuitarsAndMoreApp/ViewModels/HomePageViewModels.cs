//using System;
//using System.Collections.Generic;
//using System.Text;
//using GuitarsAndMoreApp.Models;
//using System.ComponentModel;
//using System.Collections.ObjectModel;
//using Xamarin.Forms;
//using GuitarsAndMoreApp.Services;
//using GuitarsAndMoreApp.Views;
//using System.Threading.Tasks;

//namespace GuitarsAndMoreApp.ViewModels
//{
//    class HomePageViewModels : INotifyPropertyChanged
//    {
//        public HomePageViewModels()
//        {
//            FullPostsList = new List<Post>();
//            PostsList = new ObservableCollection<Post>();
//            InitPosts();
//        }

//        public event PropertyChangedEventHandler PropertyChanged;
//        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }

//        #region Search Term
//        private string searchTerm;
//        public string SearchTerm
//        {
//            get
//            {
//                return this.searchTerm;
//            }
//            set
//            {
//                if (this.searchTerm != value)
//                {

//                    this.searchTerm = value;
//                    OnTextChanged(value);
//                    OnPropertyChanged("SearchTerm");
//                }
//            }
//        }
//        #endregion

//        #region Page Title
//        private string pageTitle;
//        public string PageTitle
//        {
//            get
//            {
//                return this.pageTitle;
//            }

//            set
//            {
//                if (this.pageTitle != value)
//                {
//                    this.pageTitle = value;
//                    OnPropertyChanged("PageTitle");
//                }
//            }
//        }
//        #endregion

//        #region Posts List
//        private ObservableCollection<Post> postsList;
//        public ObservableCollection<Post> PostsList
//        {
//            get
//            {
//                return this.postsList;
//            }

//            set
//            {
//                if (this.postsList != value)
//                {
//                    this.postsList = value;
//                    OnPropertyChanged("PostsList");
//                }
//            }
//        }
//        #endregion

//        #region Full Posts List
//        private List<Post> fullPostsList;
//        public List<Post> FullPostsList
//        {
//            get
//            {
//                return this.fullPostsList;
//            }

//            set
//            {
//                if (this.fullPostsList != value)
//                {
//                    this.fullPostsList = value;
//                    OnPropertyChanged("FullPostsList");
//                }
//            }
//        }
//        #endregion
//        private async void InitPosts()
//        {
//            GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
//            List<Post> pList = await proxy.GetListOfPostsAsync();
//            if (pList != null)
//            {
//                foreach (Post p in pList)
//                {
//                    FullPostsList.Add(p);
//                    PostsList.Add(p);
//                }
//            }
//        }

//        public void GetPostsWhere(int category)
//        {
//            foreach (Post p in FullPostsList)
//            {
//                if (p.CategoryId == category && !PostsList.Contains(p))
//                {
//                    PostsList.Add(p);
//                }
//                if (p.CategoryId != category && PostsList.Contains(p))
//                {
//                    PostsList.Remove(p);
//                }
//            }
//        }

//        #region Search
//        public void OnTextChanged(string search)
//        {
//            //Filter the list of contacts based on the search term
//            if (this.fullPostsList == null)
//                return;
//            if (String.IsNullOrWhiteSpace(search) || String.IsNullOrEmpty(search))
//            {
//                foreach (UserContact uc in this.fullPostsList)
//                {
//                    if (!this.FilteredContacts.Contains(uc))
//                        this.FilteredContacts.Add(uc);


//                }
//            }
//            else
//            {
//                foreach (UserContact uc in this.fullPostsList)
//                {
//                    string contactString = $"{uc.FirstName}|{uc.LastName}|{uc.Email}";

//                    if (!this.FilteredContacts.Contains(uc) &&
//                        contactString.Contains(search))
//                        this.FilteredContacts.Add(uc);
//                    else if (this.FilteredContacts.Contains(uc) &&
//                        !contactString.Contains(search))
//                        this.FilteredContacts.Remove(uc);
//                }
//            }

//            this.FilteredContacts = new ObservableCollection<UserContact>(this.FilteredContacts.OrderBy(uc => uc.LastName));
//        }
//        #endregion


//        public Command CategoryPageButton => new Command<int>(MoveToCategoryPage);
//        public void MoveToCategoryPage(int index)
//        {
//            App app = (App)App.Current;
//            app.MainPage.Navigation.PushAsync(new CategoryPage(index));

//        }



//    }
//}
