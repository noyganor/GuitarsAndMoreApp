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
    class EditMyPostsViewModels : INotifyPropertyChanged
    {
        public EditMyPostsViewModels()
        {
            FullPostsList = new List<Post>();
            MyPostsList = new ObservableCollection<Post>();
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

        #region My Posts List
        private ObservableCollection<Post> myPostsList;
        public ObservableCollection<Post> MyPostsList
        {
            get
            {
                return this.myPostsList;
            }

            set
            {
                if (this.myPostsList != value)
                {
                    this.myPostsList = value;
                    OnPropertyChanged("MyPostsList");
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
        public Command DeleteButton => new Command<Post>(DeleteFromMyPosts);
        public async void DeleteFromMyPosts(Post selected)
        {

            bool result = await App.Current.MainPage.DisplayAlert("אתה בטוח?", "  ", "אישור", "ביטול", FlowDirection.RightToLeft);
            if (result)
            {
                GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
                bool b = await proxy.DeletePost(selected.PostId);
                if (b)
                {
                    MyPostsList.Remove(selected);
                    App app = (App)App.Current;
                    User u = app.CurrentUser;
                    Post p = u.Posts.Where(t => t.PostId == selected.PostId).FirstOrDefault();
                    if (p != null)
                        u.Posts.Remove(p);
                }
                else
                {
                    b = false;
                }
            }
        }

        #endregion

        #region Edit Button
        public Command EditButton => new Command<Post>(EditPost);
        public async void EditPost(Post selected)
        {

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
        private async void Operate()
        {
            await InitPosts();
            await GetMyPosts();
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
                    MyPostsList.Add(p);
                }
            }
        }

        private async Task GetMyPosts()
        {
            App app = (App)App.Current;
            User u = app.CurrentUser;

            MyPostsList.Clear();
            foreach (Post p in FullPostsList)
            {
                if (u.UserId == p.UserId)
                    MyPostsList.Add(p);
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
