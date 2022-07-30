using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using C868.Services;

namespace C868.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        public ICommand OpenSearchCommand => new Command<object>((frame) =>
        {
            OpenSearch(frame, null);
            return;
        });

        public async void OpenSearch(object s, EventArgs e)
        {
            var scaleDownAnimationTask = (s as Frame).ScaleTo(.8, 65);
            var fadeOutAnimationTask = (s as Frame).FadeTo(0, 65);

            await Task.WhenAll(scaleDownAnimationTask, fadeOutAnimationTask);

            var scaleUpAnimationTask = (s as Frame).ScaleTo(1, 65);
            var fadeInAnimationTask = (s as Frame).FadeTo(1, 65);

            await Task.WhenAll(scaleUpAnimationTask, fadeInAnimationTask);

            await App.Current.MainPage.Navigation.PushModalAsync(new Search());
        }

        public ICommand OpenReportsCommand => new Command<object>((frame) =>
        {
            OpenReports(frame, null);
            return;
        });

        public async void OpenReports(object s, EventArgs e)
        {
            var scaleDownAnimationTask = (s as Frame).ScaleTo(.8, 65);
            var fadeOutAnimationTask = (s as Frame).FadeTo(0, 65);

            await Task.WhenAll(scaleDownAnimationTask, fadeOutAnimationTask);

            var scaleUpAnimationTask = (s as Frame).ScaleTo(1, 65);
            var fadeInAnimationTask = (s as Frame).FadeTo(1, 65);

            await Task.WhenAll(scaleUpAnimationTask, fadeInAnimationTask);

            await App.Current.MainPage.Navigation.PushModalAsync(new ReportsView());
        }
    }
}