using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuitarsAndMoreApp.Models;
using GuitarsAndMoreApp.ViewModels;
using GuitarsAndMoreApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GuitarsAndMoreApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Edit : ContentPage
    {
        public Edit(Post p)
        {
            EditViewModels evm = new EditViewModels(p);          
            evm.SetImageSourceEvent += SetImageSource;
            this.BindingContext = evm;
            InitializeComponent();
            SetImageSource(p.ImageUrl);
        }

        private void SetImageSource(ImageSource source)
        {
            PostImage.Source = source;
        }
    }
}