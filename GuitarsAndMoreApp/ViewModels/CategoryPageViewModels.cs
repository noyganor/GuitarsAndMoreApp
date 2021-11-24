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
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Page title
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
    }
}
