using System;
using System.Collections.Generic;
using System.Text;

using C868.Services;
using C868.Views;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace C868.ViewModels
{
    class ReportsViewModel : INotifyPropertyChanged
    {
        private List<string[]> _reportData;
        private string[] _headerText;

        public List<string[]> reportData
        {
            get
            {
                return _reportData;
            }
            set
            {
                if (_reportData != value)
                {
                    _reportData = value;
                    OnPropertyChanged("reportData");
                }
            }
        }

        public string[] headerText
        {
            get
            {
                return _headerText;
            }
            set
            {
                if (_headerText != value)
                {
                    _headerText = value;
                    OnPropertyChanged("headerText");
                }
            }
        }

        public ReportsViewModel()
        {

        }

        public ICommand CourseEndClickCommand => new Command<Frame>((buttonFrame) =>
        {
            CourseEndClick(buttonFrame, null);
        });


        public async void CourseEndClick(object s, EventArgs e)
        {
            await AnimateButton(s);

            ContentPage pageRef = (ContentPage)(s as Frame).Parent.Parent.Parent.Parent;

            reportData = await DatabaseService.CourseEndReport((pageRef.Content.FindByName("dpEnd") as DatePicker).Date.ToString());
            headerText = new string[] { "Title", "Status", "Start", "End" };
        }

        public ICommand CoursesCompletedClickCommand => new Command<Frame>((buttonFrame) =>
        {
            CoursesCompletedClick(buttonFrame, null);
        });


        public async void CoursesCompletedClick(object s, EventArgs e)
        {
            await AnimateButton(s);

            ContentPage pageRef = (ContentPage)(s as Frame).Parent.Parent.Parent.Parent;

            reportData = await DatabaseService.CoursesCompletedReport();
            headerText = new string[] { "Title", "Start", "End" };
        }

        public ICommand AllCoursesClickCommand => new Command<Frame>((buttonFrame) =>
        {
            AllCoursesClick(buttonFrame, null);
        });


        public async void AllCoursesClick(object s, EventArgs e)
        {
            await AnimateButton(s);

            ContentPage pageRef = (ContentPage)(s as Frame).Parent.Parent.Parent.Parent;

            reportData = await DatabaseService.AllCoursesReport();
            headerText = new string[] { "Title", "Status", "Start", "End" };
        }

        public ICommand AssessmentEndClickCommand => new Command<Frame>((buttonFrame) =>
        {
            AssessmentEndClick(buttonFrame, null);
        });


        public async void AssessmentEndClick(object s, EventArgs e)
        {
            await AnimateButton(s);

            ContentPage pageRef = (ContentPage)(s as Frame).Parent.Parent.Parent.Parent;

            reportData = await DatabaseService.AssessmentEndReport((pageRef.Content.FindByName("dpEnd") as DatePicker).Date.ToString());
            headerText = new string[] { "Title", "Start", "End" };
        }

        protected async Task AnimateButton(object s)
        {
            var scaleDownAnimationTask = (s as Frame).ScaleTo(.8, 65);
            var fadeOutAnimationTask = (s as Frame).FadeTo(0, 65);

            await Task.WhenAll(scaleDownAnimationTask, fadeOutAnimationTask);

            var scaleUpAnimationTask = (s as Frame).ScaleTo(1, 65);
            var fadeInAnimationTask = (s as Frame).FadeTo(1, 65);

            await Task.WhenAll(scaleUpAnimationTask, fadeInAnimationTask);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected async virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

    }    
}
