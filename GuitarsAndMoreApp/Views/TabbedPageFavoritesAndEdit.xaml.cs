using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GuitarsAndMoreApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPageFavoritesAndEdit : TabbedPage
    {
        public TabbedPageFavoritesAndEdit(int chosenTab = 0)
        {
            InitializeComponent();

            this.CurrentPage = this.Children[chosenTab];
        }
    }
}