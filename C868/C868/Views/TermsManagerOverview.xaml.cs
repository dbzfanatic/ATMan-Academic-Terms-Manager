using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using C868.ViewModels;

namespace C868.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermsManagerOverview : ContentPage
    {
        public TermsManagerOverview()
        {
            InitializeComponent();

            var termView = new TermViewModel(App.termList);

            this.BindingContext = termView;
        }
    }
}