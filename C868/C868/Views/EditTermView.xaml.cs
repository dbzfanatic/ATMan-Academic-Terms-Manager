using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using C868.Models;
using C868.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C868.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditTermView : ContentPage
    {
        private int _ID;
        private List<DisplayCourse> _cList;

        public EditTermView(int ID)
        {
            _ID = ID;
            InitializeComponent();

            _cList = (List<DisplayCourse>)App.courseList.Where(Course => Course.Term == ID).ToList();

            var editTerm = new ViewModels.EditTermViewModel(_ID);

            this.BindingContext = editTerm;
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            if (await checkRemoved())
            {
                App.termList.RemoveAt((App.termList.ToList<DisplayTerm>()).FindIndex(i => i.ID == _ID));
                await DatabaseService.RemoveTerm(_ID);
            }
            else
            {
                App.termList[(App.termList.ToList<DisplayTerm>()).FindIndex(i => i.ID == _ID)].Name = eTermname.Text;
                App.termList[(App.termList.ToList<DisplayTerm>()).FindIndex(i => i.ID == _ID)].Start = dpStartTerm.Date;
                App.termList[(App.termList.ToList<DisplayTerm>()).FindIndex(i => i.ID == _ID)].End = dpEndTerm.Date;

                await DatabaseService.UpdateTerm(_ID, eTermname.Text, dpStartTerm.Date, dpEndTerm.Date);
            }

            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void AddCourse_Clicked(object sender, EventArgs e)
        {
            if (App.termList[(App.termList.ToList<DisplayTerm>()).FindIndex(i => i.ID == _ID)].Courses.Count < 6)
            {
                Course tempCourse = new Course("New Course", "Instructor Name", "Instructor Phone #", "Instructor Email", DateTime.Now, false, DateTime.Now, false, CourseStatus.Status.Waiting.ToString(), _ID);
                tempCourse.ID = await DatabaseService.AddCourse(tempCourse);

                DisplayCourse tempDPC = new DisplayCourse(tempCourse);

                List<DisplayCourse> tempList = _cList;
                tempList.Add(tempDPC);

                App.courseList.Add(tempDPC);
                App.termList[(App.termList.ToList<DisplayTerm>()).FindIndex(i => i.ID == _ID)].Courses.Add(tempDPC);

                await App.Current.MainPage.Navigation.PushModalAsync(new CourseView(App.courseList[(App.courseList.ToList<DisplayCourse>()).FindIndex(i => i.ID == tempCourse.ID)], _ID));
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Course Limit", "You have reached the maximum allowed courses per term. Please remove one or create a new term.","Ok");
            }
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            if (await checkRemoved())
            {                
                App.termList.RemoveAt((App.termList.ToList<DisplayTerm>()).FindIndex(i => i.ID == _ID));
                await DatabaseService.RemoveTerm(_ID);
            }

            await App.Current.MainPage.Navigation.PopModalAsync();

        }

        private async Task<bool> checkRemoved()
        {
            if (eTermname.Text == "" || eTermname.Text == null)
            {
                bool answer = await DisplayAlert("Remove Term", "You are about to remove this term. Do you want to continue?", "Yes", "No");
                if (answer)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}