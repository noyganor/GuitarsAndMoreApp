using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GuitarsAndMoreApp.ViewModels;

namespace GuitarsAndMoreApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTab : TabbedPage
    {
        public MainTab()
        {
            InitializeComponent();
            App app = (App)Application.Current;
            app.TheMainTab = this;
        }

        public void SwitchToHomeTab()
        {
            CurrentPage = Children[0];
        }

        public void OnLogin()
        {
            HomePage home = (HomePage)Children[0];
            HomePageViewModels vm = (HomePageViewModels) home.BindingContext;
            vm.IsVisible = true;
        }
    }
}