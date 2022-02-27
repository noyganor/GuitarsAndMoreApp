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
    public partial class EditMyPosts : ContentPage
    {
        public EditMyPosts()
        {
            InitializeComponent();
            this.BindingContext = new EditMyPostsViewModels();
        }
    }
}