using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using GuitarsAndMoreApp.Models;
using GuitarsAndMoreApp.Services;
using GuitarsAndMoreApp.Views;

namespace GuitarsAndMoreApp.ViewModels
{
    class PostViewViewModels : INotifyPropertyChanged
    {
        public PostViewViewModels(Post p)
        {
            
            Price = p.Price;
            //Is Needed?
            if (p.Town != null)
                Town = p.Town.Town1;
            else
                Town = "Town is null!";

           
            Producer = p.Producer.Producer1;
            Model = p.Model.ModelName;
            Pdescription = p.Pdescription;
            Link = p.Link;
            PhoneNumber = p.PhoneNum;
            ImageUrl = p.ImageUrl;

        }
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Image
        private ImageSource imageUrl;
        public ImageSource ImageUrl
        {
            get
            {
                return this.imageUrl;
            }
            set
            {
                if (this.imageUrl != value)
                {
                    this.imageUrl = value;
                    OnPropertyChanged("ImageUrl");
                }
            }
        }
        #endregion

        #region Price
        private double price;
        public double Price
        {
            get
            {
                return this.price;
            }

            set
            {
                if (this.price != value)
                {
                    this.price= value;
                    OnPropertyChanged("Price");
                }
            }
        }
        #endregion

        #region Town
        private string town;
        public string Town
        {
            get
            {
                return this.town;
            }
            set
            {
                if (this.town != value)
                {
                    this.town = value;
                    OnPropertyChanged("Town");
                }
            }
        }
        #endregion

        #region Producer
        private string producer;
        public string Producer
        {
            get
            {
                return this.producer;
            }
            set
            {
                if (this.producer != value)
                {
                    this.producer = value;
                    OnPropertyChanged("Producer");
                }
            }
        }
        #endregion

        #region Model
        private string model;
        public string Model
        {
            get
            {
                return this.model;
            }
            set
            {
                if (this.model != value)
                {
                    this.model = value;
                    OnPropertyChanged("Model");
                }
            }
        }
        #endregion

        #region Post Description
        private string pdescription;
        public string Pdescription
        {
            get
            {
                return this.pdescription;
            }
            set
            {
                if (this.pdescription != value)
                {
                    this.pdescription = value;
                    OnPropertyChanged("Pdescription");
                }
            }
        }
        #endregion

        #region Link
        private string link;
        public string Link
        {
            get
            {
                return this.link;
            }
            set
            {
                if (this.link != value)
                {
                    this.link = value;
                    OnPropertyChanged("Link");
                }
            }
        }
        #endregion

        #region Phone number
        private string phoneNumber;
        public string PhoneNumber
        {
            get
            {
                return this.phoneNumber;
            }
            set
            {
                if (this.phoneNumber != value)
                {
                    this.phoneNumber = value;
                    OnPropertyChanged("PhoneNumber");
                }
            }
        }
        #endregion

     

    }
}
