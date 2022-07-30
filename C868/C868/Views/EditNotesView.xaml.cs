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

namespace C868.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditNotesView : ContentPage
    {
        private int _ID, _CID;
        public EditNotesView(int nid, string text, int cID)
        {
            InitializeComponent();

            var viewMod = new EditNotesViewModel(nid, text, cID);
            _ID = nid;
            _CID = cID;

            this.BindingContext = viewMod;
        }

        public async void SaveNote(object sender, EventArgs e)
        {
            string newNote = eText.Text;

            int noteNum = _ID;

            if (newNote != null && newNote != "")
            {
                App.notesList[(App.notesList.ToList<Note>()).FindIndex(i => i.ID == _ID)].noteText = newNote;
                await DatabaseService.UpdateNote(noteNum, newNote,_CID);
                await App.Current.MainPage.Navigation.PopModalAsync(false);
            }
            else
            {
                bool answer = await DisplayAlert("Remove Note", "You are about to remove this note. Do you want to continue?", "Yes", "No");
                if (answer)
                {
                    App.notesList.RemoveAt((App.notesList.ToList<Note>()).FindIndex(i => i.ID == _ID));
                    App.courseList[(App.courseList.ToList<Course>()).FindIndex(i => i.ID == _CID)].Notes.RemoveAt(App.courseList[(App.courseList.ToList<Course>()).FindIndex(i => i.ID == _CID)].Notes.FindIndex(i => i.ID == _ID));
                    await DatabaseService.RemoveNote(noteNum);
                    await App.Current.MainPage.Navigation.PopModalAsync(false);
                    await App.Current.MainPage.Navigation.PopModalAsync(false);
                    await App.Current.MainPage.Navigation.PushModalAsync(new CourseView(App.courseList[(App.courseList.ToList<Course>()).FindIndex(i => i.ID == _ID)]), false);
                }
            }
            
        }

        public async void CancelNote(object s, EventArgs e)
        {
            int noteNum = _ID;
            
            if(eText.Text == "" || eText.Text == null)
            {
                bool answer = await DisplayAlert("Remove Note", "You are about to remove this note. Do you want to continue?", "Yes", "No");
                if (answer)
                {
                    App.notesList.RemoveAt((App.notesList.ToList<Note>()).FindIndex(i => i.ID == _ID));
                    App.courseList[(App.courseList.ToList<Course>()).FindIndex(i => i.ID == _CID)].Notes.RemoveAt(App.courseList[(App.courseList.ToList<Course>()).FindIndex(i => i.ID == _CID)].Notes.FindIndex(i => i.ID == _ID));
                    await DatabaseService.RemoveNote(_ID);
                }
            }

            
            await App.Current.MainPage.Navigation.PopModalAsync(false);
            await App.Current.MainPage.Navigation.PopModalAsync(false);
            await App.Current.MainPage.Navigation.PushModalAsync(new CourseView(App.courseList[(App.courseList.ToList<DisplayCourse>()).FindIndex(i => i.ID == _CID)]), false);
            
        }
        
    }
}