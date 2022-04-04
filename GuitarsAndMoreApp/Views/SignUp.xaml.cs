using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuitarsAndMoreApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GuitarsAndMoreApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUp : ContentPage
    {
        public SignUp()
        {
            SignUpViewModels vm = new SignUpViewModels();
            vm.SetImageSourceEvent += SetImageSource;
            this.BindingContext = vm;
            InitializeComponent();
       
        }

        private void SetImageSource(ImageSource source)
        {
            UserImage.Source = source;
        }
    }
}