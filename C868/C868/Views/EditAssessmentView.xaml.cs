using C868.Models;
using C868.ViewModels;
using C868.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.LocalNotifications;

namespace C868.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAssessmentView : ContentPage
    {
        private int _ID,type,cid;

        public EditAssessmentView(int? ID, int Type, int courseID)
        {
            InitializeComponent();

            _ID = (int)ID;
            type = Type;
            cid = courseID;

            var tempModel = new EditAssessmentViewModel((int)ID, Type, courseID);

            this.BindingContext = tempModel;
        }

        protected override void OnDisappearing()
        {
            if (aName.Text == "" || aName.Text == null)
            {
                App.assessList.RemoveAt(App.assessList.ToList<Assessment>().FindIndex(i => i.ID == _ID));
            }

            base.OnDisappearing();
        }

        private void chkAENotify_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            App.assessList[(App.assessList.ToList<Assessment>()).FindIndex(i => i.ID == _ID)].NotifyEnd = chkAENotify.IsChecked;
            App.assessList[(App.assessList.ToList<Assessment>()).FindIndex(i => i.ID == _ID)].End = dpEndAssess.Date;
        }

        private void chkASNotify_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            App.assessList[(App.assessList.ToList<Assessment>()).FindIndex(i => i.ID == _ID)].NotifyStart = chkASNotify.IsChecked;
        }        

        private void chkObjNotify_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            App.assessList[(App.assessList.ToList<Assessment>()).FindIndex(i => i.ID == _ID)].NotifyEnd = chkObjNotify.IsChecked;
            if((string)fpDurationPicker.SelectedItem == "Hours")
            {
                App.assessList[(App.assessList.ToList<Assessment>()).FindIndex(i => i.ID == _ID)].End = DateTime.Parse(dpStartAssess.Date.ToString() + tpStartAssess.Time.ToString()).AddHours(int.Parse(Duration.Text));
            }
            else
            {
                App.assessList[(App.assessList.ToList<Assessment>()).FindIndex(i => i.ID == _ID)].End = DateTime.Parse(dpStartAssess.Date.ToString() + tpStartAssess.Time.ToString()).AddMinutes(int.Parse(Duration.Text));
            }
            
        }

        private async void ButtonSave_Clicked(object sender, EventArgs e)
        {
            var tempAssess = new Assessment(type, aName.Text, dpStartAssess.Date, chkASNotify.IsChecked, dpEndAssess.Date, chkAENotify.IsChecked, cid);

            if (type == 0)
            {
                tempAssess.Start = DateTime.Parse(dpStartAssess.Date.ToString());
                tempAssess.End = DateTime.Parse(dpEndAssess.Date.ToString());

                App.courseList[App.courseList.ToList<DisplayCourse>().FindIndex(i => i.ID == cid)].PAssessment = tempAssess;
            }
            else
            {                
                tempAssess.Start = DateTime.Parse(dpStartAssess.Date.ToString() + " " + tpStartAssess.Time.ToString());
                if((string)fpDurationPicker.SelectedItem == "Hours")
                {
                    tempAssess.End = dpEndAssess.Date.AddHours(int.Parse(Duration.Text));
                }
                else
                {
                    tempAssess.End = dpEndAssess.Date.AddMinutes(int.Parse(Duration.Text));
                }
                
                App.courseList[App.courseList.ToList<DisplayCourse>().FindIndex(i => i.ID == cid)].OAssessment = tempAssess;
            }

            App.assessList[(App.assessList.ToList<Assessment>()).FindIndex(i => i.ID == _ID)] = tempAssess;

            Course courseRef = (Course)App.courseList[App.courseList.ToList<DisplayCourse>().FindIndex(i => i.ID == cid)];

            await DatabaseService.UpdateAssessment(_ID, type, aName.Text, tempAssess.Start, chkASNotify.IsChecked, tempAssess.End, chkAENotify.IsChecked, cid);
            await DatabaseService.UpdateCourse(cid, courseRef.Title, courseRef.instName, courseRef.instPHone, courseRef.insteMail, courseRef.Start, courseRef.NotifyStart, courseRef.End, courseRef.NotifyEnd, courseRef.Status, courseRef.Term, courseRef.PAssessment, courseRef.OAssessment);            

            if (chkASNotify.IsChecked)
            {
                CrossLocalNotifications.Current.Show(App.assessList[(App.assessList.ToList<Assessment>()).FindIndex(i => i.ID == _ID)].Title, "The assessment " + App.assessList[(App.assessList.ToList<Assessment>()).FindIndex(i => i.ID == _ID)].Title + " is starting soon",3000 + _ID, App.assessList[App.assessList.ToList().FindIndex(i => i.ID == _ID)].Start.AddMinutes(-15));
            }
            if (chkAENotify.IsChecked)
            {
                CrossLocalNotifications.Current.Show(App.assessList[(App.assessList.ToList<Assessment>()).FindIndex(i => i.ID == _ID)].Title, "The assessment " + App.assessList[(App.assessList.ToList<Assessment>()).FindIndex(i => i.ID == _ID)].Title + " is ending soon", 3000 + _ID, App.assessList[App.assessList.ToList().FindIndex(i => i.ID == _ID)].End.AddMinutes(-15));
            }

            App.Current.MainPage.Navigation.PopModalAsync();
            await App.Current.MainPage.Navigation.PopModalAsync(false);
            await App.Current.MainPage.Navigation.PushModalAsync(new CourseView(App.courseList[App.courseList.ToList<DisplayCourse>().FindIndex(i => i.ID == cid)]));

        }

        private async void ButtonDelete_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Remove Assessment", "You are about to remove this assessment. Do you want to continue?", "Yes", "No");
            if (answer)
            {
                await DatabaseService.RemoveAssessment(_ID);
                if(type == 0)
                {
                    App.courseList[App.courseList.ToList<DisplayCourse>().FindIndex(i => i.ID == cid)].PAssessment = null;
                    var courseRef = App.courseList[App.courseList.ToList<DisplayCourse>().FindIndex(i => i.ID == cid)];
                    await DatabaseService.UpdateCourse(cid,courseRef.Title, courseRef.instName, courseRef.instPHone, courseRef.insteMail, courseRef.Start, courseRef.NotifyStart, courseRef.End, courseRef.NotifyEnd, courseRef.Status, courseRef.Term, 0, courseRef.OAssessment.ToInt());
                }
                else
                {
                    App.courseList[App.courseList.ToList<DisplayCourse>().FindIndex(i => i.ID == cid)].OAssessment = null;
                    var courseRef = App.courseList[App.courseList.ToList<DisplayCourse>().FindIndex(i => i.ID == cid)];
                    await DatabaseService.UpdateCourse(cid, courseRef.Title, courseRef.instName, courseRef.instPHone, courseRef.insteMail, courseRef.Start, courseRef.NotifyStart, courseRef.End, courseRef.NotifyEnd, courseRef.Status, courseRef.Term, courseRef.PAssessment.ToInt(),0);
                }

                App.Current.MainPage.Navigation.PopModalAsync();
                await App.Current.MainPage.Navigation.PopModalAsync(false);
                await App.Current.MainPage.Navigation.PushModalAsync(new CourseView(App.courseList[App.courseList.ToList<DisplayCourse>().FindIndex(i => i.ID == cid)]));
            }
        }
    }
}