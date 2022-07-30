using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C868.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlyoutContainer : FlyoutPage
    {

        public FlyoutContainer()
        {
            this.Title = "ATMan";
            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            this.Detail = new NavigationPage(new TermsManagerOverview());
            base.OnAppearing();
        }
    }
}