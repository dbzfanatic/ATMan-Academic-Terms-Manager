using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

using C868.Services;
using C868.Views;
using System.Threading.Tasks;

namespace C868.ViewModels
{
    class SearchViewModel : INotifyPropertyChanged
    {
        private List<string[]> _searchData;

        public SearchViewModel()
        {

        }

        public List<string[]> searchData
        {
            get
            {
                return _searchData;
            }
            set
            {
                if(_searchData != value)
                {
                    _searchData = value;
                    OnPropertyChanged("searchData");
                }
            }
        }

        public ICommand SearchClickCommand => new Command<Frame>((buttonFrame) =>
        {
            SearchClick(buttonFrame,null);
        });


        public async void SearchClick(object s, EventArgs e)
        {
            var scaleDownAnimationTask = (s as Frame).ScaleTo(.8, 65);
            var fadeOutAnimationTask = (s as Frame).FadeTo(0, 65);

            await Task.WhenAll(scaleDownAnimationTask, fadeOutAnimationTask);

            var scaleUpAnimationTask = (s as Frame).ScaleTo(1, 65);
            var fadeInAnimationTask = (s as Frame).FadeTo(1, 65);

            await Task.WhenAll(scaleUpAnimationTask, fadeInAnimationTask);

            ContentPage pageRef = (ContentPage)(s as Frame).Parent.Parent.Parent;

            searchData = await DatabaseService.SearchDB((pageRef.Content.FindByName("txtSearch") as Entry).Text,(pageRef.Content.FindByName("chkTerm") as CheckBox).IsChecked, (pageRef.Content.FindByName("chkCourse") as CheckBox).IsChecked, (pageRef.Content.FindByName("chkAssessment") as CheckBox).IsChecked);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected async virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
