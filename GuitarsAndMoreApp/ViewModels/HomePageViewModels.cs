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


        public Command CategoryPageButton => new Command<int>(MoveToCategoryPage);
        public void MoveToCategoryPage(int index)
        {
            App app = (App)App.Current;
            app.MainPage.Navigation.PushAsync(new CategoryPage(index));

        }



    }
}
