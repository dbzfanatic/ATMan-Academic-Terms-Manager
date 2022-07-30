using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using C868.ViewModels;
using C868.Models;
using C868.Services;
using Plugin.LocalNotifications;

namespace C868.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseView : ContentPage
    {
        private int _ID;
        private DisplayCourse _Course;
        public CourseView(DisplayCourse course, int ID = 0)
        {
            InitializeComponent();

            _Course = course;

            if (ID != 0)
            {
                _ID = ID;
            }
            else
            {
                _ID = _Course.ID;
            }
            
            this.BindingContext = new CourseViewModel(_Course);
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {

            if (chkStartNotify.IsChecked)
            {
                CrossLocalNotifications.Current.Show(App.courseList[(App.courseList.ToList<DisplayCourse>()).FindIndex(i => i.ID == _ID)].Title, "The course " + App.courseList[(App.courseList.ToList<DisplayCourse>()).FindIndex(i => i.ID == _ID)].Title + " is starting soon", 2000 + _ID, App.courseList[(App.courseList.ToList<DisplayCourse>()).FindIndex(i => i.ID == _ID)].Start.AddMinutes(-15));
            }
            if (chkEndNotify.IsChecked)
            {
                CrossLocalNotifications.Current.Show(App.courseList[(App.courseList.ToList<DisplayCourse>()).FindIndex(i => i.ID == _ID)].Title, "The course " + App.courseList[(App.courseList.ToList<DisplayCourse>()).FindIndex(i => i.ID == _ID)].Title + " is ending soon", 2000 + _ID, App.courseList[(App.courseList.ToList<DisplayCourse>()).FindIndex(i => i.ID == _ID)].End.AddMinutes(-15));
            }

            App.courseList[(App.courseList.ToList<DisplayCourse>()).FindIndex(i => i.ID == _Course.ID)] = new DisplayCourse(_Course.Title, _Course.instName, _Course.instPHone, _Course.insteMail, _Course.Start, _Course.NotifyStart, _Course.End, _Course.NotifyEnd, _Course.Status, _Course.Term, _Course.PAssessment, _Course.OAssessment, _Course.Notes);
            if (_Course.PAssessment != null && _Course.OAssessment != null)
            {
                await DatabaseService.UpdateCourse(_Course.ID, _Course.Title, _Course.instName, _Course.instPHone, _Course.insteMail, _Course.Start, _Course.NotifyStart, _Course.End, _Course.NotifyEnd, _Course.Status, _Course.Term, _Course.PAssessment.ID, _Course.OAssessment.ID);
            }
            else if (_Course.PAssessment != null)
            {
                await DatabaseService.UpdateCourse(_Course.ID, _Course.Title, _Course.instName, _Course.instPHone, _Course.insteMail, _Course.Start, _Course.NotifyStart, _Course.End, _Course.NotifyEnd, _Course.Status, _Course.Term, _Course.PAssessment.ID, 0);
            }
            else if (_Course.OAssessment != null)
            {
                await DatabaseService.UpdateCourse(_Course.ID, _Course.Title, _Course.instName, _Course.instPHone, _Course.insteMail, _Course.Start, _Course.NotifyStart, _Course.End, _Course.NotifyEnd, _Course.Status, _Course.Term, 0, _Course.OAssessment.ID);
            }
            else
            {
                await DatabaseService.UpdateCourse(_Course.ID, _Course.Title, _Course.instName, _Course.instPHone, _Course.insteMail, _Course.Start, _Course.NotifyStart, _Course.End, _Course.NotifyEnd, _Course.Status, _Course.Term, 0, 0);
            }            
            await App.Current.MainPage.Navigation.PopModalAsync(false);
        }

        protected override async void OnDisappearing()
        {            

            if (App.Current.MainPage.Navigation.ModalStack.Count > 0 && App.Current.MainPage.Navigation.ModalStack[0].GetType() == typeof(EditTermView))
            {
                App.Current.MainPage.Navigation.PopModalAsync(false);
                App.Current.MainPage.Navigation.PopModalAsync(false);
                App.Current.MainPage.Navigation.PushModalAsync(new EditTermView(_ID),false);
            }
            else if (App.Current.MainPage.Navigation.ModalStack.Count == 0)
            {
                
            }
            
            base.OnDisappearing();
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Remove Course", "You are about to remove this course. Do you want to continue?", "Yes", "No");
            if (answer)
            {
                App.termList[App.termList.ToList<DisplayTerm>().FindIndex(i => i.ID == _Course.Term)].Courses.RemoveAt(App.termList[App.termList.ToList<DisplayTerm>().FindIndex(i => i.ID == _Course.Term)].Courses.FindIndex(i => i.ID == _Course.ID));
                App.courseList.RemoveAt(App.courseList.ToList<DisplayCourse>().FindIndex(i => i.ID == _ID));
                await DatabaseService.RemoveCourse(_ID);
                await App.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        private void chkStartNotify_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            App.courseList[App.courseList.ToList<DisplayCourse>().FindIndex(i => i.ID == _ID)].NotifyStart = chkStartNotify.IsChecked;
        }

        private void chkEndNotify_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            App.courseList[App.courseList.ToList<DisplayCourse>().FindIndex(i => i.ID == _ID)].NotifyEnd = chkEndNotify.IsChecked;
        }
    }
}