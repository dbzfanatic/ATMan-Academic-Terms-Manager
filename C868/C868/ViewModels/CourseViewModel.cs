using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using C868.Models;
using C868.Views;
using C868.Services;

using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Web;
using System.Text.RegularExpressions;
using System.Globalization;

namespace C868.ViewModels
{
    public class CourseViewModel : INotifyPropertyChanged
    {
        private DisplayCourse _course;

        public CourseViewModel(DisplayCourse course)
        {
            _course = course;
        }

        public int ID
        {
            get { return _course.ID; }
        }

        public string Title
        {
            get { return _course.Title; }
            set 
            {
                if (Title != value)
                {
                    _course.Title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        public string InstName
        {
            get { return _course.instName; }
            set
            {
                if (InstName != value)
                {
                    _course.instName = value;
                    OnPropertyChanged("InstName");
                }
            }
        }

        public string InstPhone
        {
            get { return _course.instPHone; }
            set
            {
                if (InstPhone != value)
                {
                    _course.instPHone = value;
                    OnPropertyChanged("InstPhone");
                }
            }
        }

        public string InstEMail
        {
            get { return _course.insteMail; }
            set
            {
                if (InstEMail != value)
                {
                    _course.insteMail = value;
                    OnPropertyChanged("InstEMail");
                }
            }
        }

        public DateTime Start
        {
            get { return _course.Start; }
        }

        public DateTime End
        {
            get { return _course.End; }
        }
        public string Status
        {
            get => _course.Status.ToString();
        }

        public bool StartNotify
        {
            get { return _course.NotifyStart; }
        }

        public bool EndNotify
        {
            get { return _course.NotifyEnd; }
        }

        public Assessment PerfAssess
        {
            get { return _course.PAssessment; }
        }

        public Assessment ObjAssess
        {
            get { return _course.OAssessment; }
        }

        public Course Course
        {
            get => _course;
        }

        public List<Note> Notes
        {
            get
            {
                return _course.Notes;
            }
            set
            {
                if (Notes != value)
                {
                    _course.Notes = value;
                    OnPropertyChanged("Notes");
                }
            }
        }

        public ICommand EmailClickCommand => new Command<string>((url) =>
        {
            Launcher.OpenAsync(new System.Uri("mailto:" + url)); //open email app with instructor email
        });

        public ICommand PhoneClickCommand => new Command<string>((num) =>
        {
            Launcher.OpenAsync(new System.Uri("tel:" + num)); //open phone dialer with instructor phone #
        });

        public ICommand EditCourseTitleDT => new Command<int>((ID) =>
        {
            EditCourseTitle(ID);
        });

        public async void EditCourseTitle(int sArgs)
        {
            string newTitle = await App.Current.MainPage.DisplayPromptAsync("Title Change", "New Course Title");
            if (newTitle != null && newTitle != "")
            {
                Title = newTitle;
            }
            else if(newTitle == "")
            {
                await App.Current.MainPage.DisplayAlert("No Entry", "Your entry of ' ' (literally nothing) is not valid","Ok");
            }
        }

        public ICommand AddNoteCommand => new Command<object>((frame) =>
        {
            AddNote(frame,null);
        });

        public async void AddNote(object s,EventArgs e)
        {           
            var scaleDownAnimationTask = (s as Frame).ScaleTo(.8, 65);
            var fadeOutAnimationTask = (s as Frame).FadeTo(0, 65);

            await Task.WhenAll(scaleDownAnimationTask, fadeOutAnimationTask);

            var scaleUpAnimationTask = (s as Frame).ScaleTo(1, 65);
            var fadeInAnimationTask = (s as Frame).FadeTo(1, 65);

            await Task.WhenAll(scaleUpAnimationTask, fadeInAnimationTask);

            int tempNum = App.notesList.Count;
            Note tempNote = new Note("", ID);
            App.courseList[(App.courseList.ToList<DisplayCourse>()).FindIndex(i => i.ID == _course.ID)].Notes.Add(tempNote);
            App.notesList.Add(tempNote);
            tempNote.ID = await DatabaseService.AddNote(tempNote);
            await App.Current.MainPage.Navigation.PushModalAsync(new EditNotesView(tempNote.ID,tempNote.noteText,tempNote.classID));
            List<Note> tempList = App.notesList.Where(Note => Note.classID == ID).ToList();
            Notes = (List<Note>)tempList;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected async virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public ICommand EditCourseInstDT => new Command<int>((ID) =>
        {
            EditCourseInst(ID);
        });

        public async void EditCourseInst(int sArgs)
        {
            string newName = await App.Current.MainPage.DisplayPromptAsync("Instructor Name Change", "New Instructor Name");
            if (newName != null && newName != "")
            {
                InstName = newName;

            }
            else if (newName == "")
            {
                await App.Current.MainPage.DisplayAlert("No Entry", "Your entry of ' ' (literally nothing) is not valid", "Ok");
            }
        }

        public ICommand EditCoursePhoneDT => new Command<int>((ID) =>
        {
            EditCoursePhone(ID);
        });

        public async void EditCoursePhone(int sArgs)
        {

            string newPhone = await App.Current.MainPage.DisplayPromptAsync("Instructor Phone Change", "New Instructor Phone");
            if (newPhone != null && newPhone != "")
            {
                if (PhoneValid(newPhone))
                {
                    InstPhone = newPhone;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Invalid Entry", "The listed phone number is not in a valid format. Please try again.", "Ok");
                    EditCoursePhone(sArgs);
                }

            }
            else if (newPhone == "")
            {
                await App.Current.MainPage.DisplayAlert("No Entry", "Your entry of ' ' (literally nothing) is not valid", "Ok");
                EditCoursePhone(sArgs);
            }
        }

        public ICommand EditCourseEmailDT => new Command<int>((ID) =>
        {
            EditCourseEmail(ID);
        });

        public async void EditCourseEmail(int sArgs)
        {
            string newEmail = await App.Current.MainPage.DisplayPromptAsync("Instructor Email Change", "New Instructor Email");
            if (newEmail != null && newEmail != "")
            {
                if (EmailValid(newEmail))
                {
                    InstEMail = newEmail;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Invalid Entry", "The listed email is not in a valid format. Please try again.", "Ok");
                    EditCourseEmail(sArgs);
                }
                

            }
            else if (newEmail == "")
            {
                await App.Current.MainPage.DisplayAlert("No Entry", "Your entry of ' ' (literally nothing) is not valid", "Ok");
                EditCourseEmail(sArgs);
            }
        }

        public ICommand EditObjAssessment => new Command<int?>((oaID) =>
        {
            EditOA(oaID);
        });

        public async void EditOA(int? oaID)
        {
            if(oaID == null || oaID == 0)
            {
                Assessment tempOA = new Assessment(1, "New Objective Assessment", DateTime.Now, false, DateTime.Now, false,ID);                
                App.assessList.Add(tempOA);
                tempOA.ID = await DatabaseService.AddAssessment(tempOA);
                oaID = tempOA.ID;
                _course.OAssessment = tempOA;
            }

            await App.Current.MainPage.Navigation.PushModalAsync(new EditAssessmentView(oaID,1,ID));
        }

        public ICommand EditPerfAssessment => new Command<int?>((paID) =>
        {
            EditPA(paID);
        });

        public async void EditPA(int? paID)
        {
            if (paID == null || paID == 0)
            {
                Assessment tempPA = new Assessment(0, "New Performance Assessment", DateTime.Now, false, DateTime.Now, false, ID);
                App.assessList.Add(tempPA);
                tempPA.ID = await DatabaseService.AddAssessment(tempPA);
                paID = tempPA.ID;
                _course.PAssessment = tempPA;
            }

            await App.Current.MainPage.Navigation.PushModalAsync(new EditAssessmentView(paID, 0,ID));
        }

        private bool EmailValid(string checkEmail)
        {
            try
            {
                return Regex.IsMatch(checkEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch
            {
                return false;
            }
        }

        private bool PhoneValid(string checkPhone)
        {
            try
            {
                return Regex.IsMatch(checkPhone, @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", RegexOptions.None, TimeSpan.FromMilliseconds(250));
            }
            catch
            {
                return false;
            }
        }
        
    }

    public class BindingTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }
    }
}
