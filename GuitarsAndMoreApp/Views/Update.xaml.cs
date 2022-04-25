using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuitarsAndMoreApp.ViewModels;
using GuitarsAndMoreApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GuitarsAndMoreApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Update : ContentPage
    {
        public Update()
        {
            UpdateViewModels u = new UpdateViewModels();

            //fix
            App app = (App)App.Current;
            SetImageSource(app.CurrentUser.ImageUrl);
            u.SetImageSourceEvent += SetImageSource;
            this.BindingContext = u;
            InitializeComponent();
        }

        private void SetImageSource(ImageSource source)
        {
            UserImage.Source = source;
        }
    }
}