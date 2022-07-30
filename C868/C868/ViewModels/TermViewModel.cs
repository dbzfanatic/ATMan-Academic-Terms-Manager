using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using C868.Models;
using C868.Services;

using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;

namespace C868.ViewModels
{
    public class TermViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Course> _Courses = new ObservableCollection<Course>();
        private ObservableCollection<DisplayTerm> _Term = new ObservableCollection<DisplayTerm>();


        public TermViewModel(ObservableCollection<DisplayTerm> terms)
        {
            _Term = terms;
        }

        public TermViewModel() { }

        public ObservableCollection<DisplayTerm> Term
        {
            get
            {
                return _Term;
            }
            set
            {
                if (Term != value)
                {
                    _Term = value;
                    OnPropertyChanged("Term");
                }
            }
        }

        public ObservableCollection<Course> Courses
        {
            get
            {
                return _Courses;
            }
            set
            {
                if(Courses != value)
                {
                    _Courses = value;
                    OnPropertyChanged("Courses");
                }
            }
        }

        public string StateIcon { get; set; }        

        public event PropertyChangedEventHandler PropertyChanged;

        protected async virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        }

        public ICommand AddTermCommand => new Command<object>((frame) =>
        {
            AddTerm(frame, null);
        });

        public async void AddTerm(object s, EventArgs e)
        {
            var scaleDownAnimationTask = (s as Frame).ScaleTo(.8, 65);
            var fadeOutAnimationTask = (s as Frame).FadeTo(0, 65);

            await Task.WhenAll(scaleDownAnimationTask, fadeOutAnimationTask);

            var scaleUpAnimationTask = (s as Frame).ScaleTo(1, 65);
            var fadeInAnimationTask = (s as Frame).FadeTo(1, 65);

            await Task.WhenAll(scaleUpAnimationTask, fadeInAnimationTask);

            Term tempTerm = new Term("New Term", DateTime.Now,DateTime.Now);
            App.termList.Add(new DisplayTerm(tempTerm));
            tempTerm.ID = await DatabaseService.AddTerm(tempTerm);
            App.termList[(App.termList.ToList<DisplayTerm>()).FindIndex(i => i.Name == tempTerm.Name)].ID = tempTerm.ID;
            await App.Current.MainPage.Navigation.PushModalAsync(new Views.EditTermView(tempTerm.ID));
        }

        public ICommand OpenCourse => new Command<int>((ID) =>
        {
            App.Current.MainPage.Navigation.PushModalAsync(new Views.CourseView(App.courseList[ID-1]));
        });

        public ICommand EditTermCommand => new Command<int>((ID) =>
        {
            App.Current.MainPage.Navigation.PushModalAsync(new Views.EditTermView(ID));
        });
    }
}
