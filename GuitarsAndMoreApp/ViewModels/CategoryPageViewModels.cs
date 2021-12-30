using System;
using System.Collections.Generic;
using System.Text;
using GuitarsAndMoreApp.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using GuitarsAndMoreApp.Services;
using GuitarsAndMoreApp.Views;

namespace GuitarsAndMoreApp.ViewModels
{
    public class CategoryPageViewModels : INotifyPropertyChanged
    {
        private int index;
        public CategoryPageViewModels(int index)
        {
            this.index = index;
            if (this.index == 1)
            {
                this.pageTitle = "גיטרות";
            }

            if (this.index == 2)
            {
                this.pageTitle = "סאונד והגברה";
            }

            if (this.index == 3)
            {
                this.pageTitle = "קייסים לגיטרות";
            }

            if (this.index == 4)
            {
                this.pageTitle = "אביזרים לגיטרות";
            }

            this.postsList = GetPostsWhere(this.index);
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
        private Post postsList;
        public Post PostsList
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

        public async List<Post> GetPostsWhere(int category)
        {
            GuitarsAndMoreAPIProxy proxy = GuitarsAndMoreAPIProxy.CreateProxy();
            try
            {
                List<Post> pList = await proxy.GetListOfPostsAsync();

                if (pList == null)
                {
                    Message = "";
                }

                else
                {
                    List<Post> postsL = new List<Post>();
                    foreach (Post p in pList)
                    {
                        if (p.CategoryId == category)
                        {
                            postsL.Add(p);
                        }
                    }

                    return postsL;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

    }
}
