using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using C868.ViewModels;
using C868.Views;
using C868.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using SQLite;
using C868.Services;
using System.Linq;
using System.Threading;
using Plugin.LocalNotifications;

namespace C868
{
    public partial class App : Application
    {
        public static ObservableCollection<DisplayCourse> courseList;
        public static ObservableCollection<DisplayTerm> termList;
        public static ObservableCollection<Assessment> assessList;
        public static ObservableCollection<Note> notesList;

        public App()
        {
            termList = new ObservableCollection<DisplayTerm>();
            courseList = new ObservableCollection<DisplayCourse>();
            assessList = new ObservableCollection<Assessment>();
            notesList = new ObservableCollection<Note>();

            App.Current.MainPage = new FlyoutContainer();

            InitializeComponent();            

            LoadData();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        protected async void LoadData()
        {
            var tempTermlist = await DatabaseService.GetTerms();
            List<DisplayTerm> dTerms = new List<DisplayTerm>();
            foreach(Term term in tempTermlist)
            {
                DisplayTerm temp = new DisplayTerm(term);
                temp.Courses.Capacity = 6;
                dTerms.Add(temp);
            }
            termList = new ObservableCollection<DisplayTerm>(dTerms);

            var tempAssesslist = await DatabaseService.GetAssessments();
            List<Assessment> dassesss = new List<Assessment>();
            foreach (Assessment assess in tempAssesslist)
            {
                dassesss.Add(assess);
            }
            assessList = new ObservableCollection<Assessment>(dassesss);

            var tempCourselist = await DatabaseService.GetCourses();
            List<DisplayCourse> dCourses = new List<DisplayCourse>();
            foreach (Course Course in tempCourselist)
            {
                DisplayCourse temp = new DisplayCourse(Course);
                dCourses.Add(temp);
            }
            courseList = new ObservableCollection<DisplayCourse>(dCourses);            

            var tempNotelist = await DatabaseService.GetNotes();
            List<Note> dNotes = new List<Note>();
            foreach (Note note in tempNotelist)
            {
                dNotes.Add(note);
            }
            notesList = new ObservableCollection<Note>(dNotes);

            if (termList.Count == 0 && courseList.Count == 0 && assessList.Count == 0 && notesList.Count == 0)
            {
                dTerms.Add(new DisplayTerm("Initial Test Term", DateTime.Parse("06/22/22"), DateTime.Parse("07/12/22")));
                dTerms[0].ID = await DatabaseService.AddTerm(dTerms[0].Name, dTerms[0].Start, dTerms[0].End);
                termList = new ObservableCollection<DisplayTerm>(dTerms);
            
                dassesss.Add(new Assessment(0, "Test Performance Assessment", DateTime.Parse("02/02/2022"), false, DateTime.Parse("02/03/2022"), false));
                dassesss.Add(new Assessment(1, "Test Objective Assessment", DateTime.Parse("02/02/2022 09:00 PM"), false, DateTime.Parse("02/03/2022 09:00PM"), false));

                dassesss[0].ID = await DatabaseService.AddAssessment(dassesss[0]);
                dassesss[1].ID = await DatabaseService.AddAssessment(dassesss[1]);

                assessList = new ObservableCollection<Assessment>(dassesss);

                dCourses.Add(new DisplayCourse(new Course("Test Course", "Michael LaBruyere", @"(573)200-1855", "mlabruy@wgu.edu", DateTime.Parse("02/02/22"), false, DateTime.Now, false, CourseStatus.Status.Waiting.ToString(), dTerms[0].ID, 1, 2), null, null, new List<Note>()));
                dCourses[0].ID = await DatabaseService.AddCourse(dCourses[0].Title, dCourses[0].instName, dCourses[0].instPHone, dCourses[0].insteMail, dCourses[0].Start, dCourses[0].NotifyStart, dCourses[0].End, dCourses[0].NotifyEnd, dCourses[0].Status, 1, dCourses[0].PAssessment.ToInt(), dCourses[0].OAssessment.ToInt());

                dCourses.Add(new DisplayCourse(new Course("Completed Test Course", "Michael LaBruyere", @"(573)200-1855", "mlabruy@wgu.edu", DateTime.Parse("02/02/22"), false, DateTime.Now, false, CourseStatus.Status.Completed.ToString(), dTerms[0].ID, 1, 2), null, null, new List<Note>()));
                dCourses[1].ID = await DatabaseService.AddCourse(dCourses[1].Title, dCourses[1].instName, dCourses[1].instPHone, dCourses[1].insteMail, dCourses[1].Start, dCourses[1].NotifyStart, dCourses[1].End, dCourses[1].NotifyEnd, dCourses[1].Status, 1, dCourses[1].PAssessment.ToInt(), dCourses[1].OAssessment.ToInt());

                courseList = new ObservableCollection<DisplayCourse>(dCourses);

                dNotes.Add(new Note(@"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut pellentesque velit non vulputate rutrum. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Nulla quis justo in dolor facilisis venenatis id eget purus. Duis dignissim dapibus ultricies. Morbi hendrerit dictum velit, ut mollis ligula semper faucibus. Aliquam tristique arcu ac scelerisque auctor. Nam faucibus nunc eu lorem placerat lacinia. Ut ligula velit, condimentum nec faucibus at, placerat in metus. Pellentesque hendrerit nulla viverra, aliquam est id, aliquam sem. Proin pulvinar laoreet fringilla.

Aliquam velit neque, ullamcorper ut finibus a, venenatis in augue.In quis efficitur erat, eu rutrum sem.Maecenas id sollicitudin nulla.Nunc tempus purus et eros lacinia viverra vel id sapien.Quisque ornare quam sit amet nulla posuere ornare.Quisque posuere consectetur orci, sit amet tempus ligula pulvinar in.Nam eget viverra augue.Vestibulum eget pulvinar tortor, a auctor urna.Mauris non nisi aliquam, aliquam lacus viverra, aliquam nisi.Donec aliquam, nisi at porttitor malesuada, velit ex maximus urna, fermentum sagittis ipsum nibh vitae diam.Donec accumsan mattis ipsum.

Aenean id finibus nisi, ac pulvinar nunc.Cras dapibus nibh a purus venenatis aliquam.Vestibulum eros nisi, convallis ut magna sed, volutpat tincidunt est.Aliquam porta urna ac neque convallis, et fermentum neque dapibus.Suspendisse nec malesuada diam, rutrum sollicitudin purus.Curabitur velit mi, posuere eu neque nec, maximus rutrum diam.Donec efficitur lorem a vehicula dignissim.Integer facilisis sapien scelerisque eros varius, eget rutrum est tempor.Sed ut interdum lectus.

Maecenas porttitor, tortor sit amet imperdiet sollicitudin, nisl enim commodo sem, in volutpat mi lectus sed urna.Sed id volutpat tellus.Nullam finibus in mi iaculis interdum.Aenean vitae metus nec neque convallis efficitur.Etiam euismod justo dolor, sed placerat lacus blandit nec.Duis pellentesque ligula eu felis consectetur blandit.Cras viverra, metus in sagittis iaculis, odio lorem ultrices purus, non varius erat est id urna.Sed finibus metus at feugiat congue.Nam consequat eget lacus eget blandit.

Proin ac libero massa.Lorem ipsum dolor sit amet, consectetur adipiscing elit.Praesent vestibulum neque id efficitur egestas.Aliquam mollis sapien sem, sit amet interdum nisi ullamcorper et.Curabitur libero urna, laoreet sit amet condimentum in, sagittis in nunc.In pretium est augue, eu egestas tellus feugiat in.Mauris commodo, lacus sit amet molestie aliquet, massa nisi maximus risus, eu accumsan mi arcu quis arcu.Quisque quam nunc, maximus eu leo eu, laoreet vestibulum lorem.Mauris ullamcorper turpis odio, et posuere ipsum rhoncus ut.Aliquam vitae elit in eros hendrerit iaculis eu id metus.Maecenas varius auctor sodales.", 1));

                dNotes[0].ID = await DatabaseService.AddNote(dNotes[0]);
                notesList = new ObservableCollection<Note>(dNotes);
            }

            foreach(DisplayTerm term in termList)
            {
                term.Courses = (List<DisplayCourse>)courseList.Where(Course => Course.Term == term.ID).ToList();
            }

            foreach (DisplayCourse course in courseList)
            {
                course.Notes = (List<Note>)notesList.Where(Note => Note.classID == course.ID).ToList();

                if (course.NotifyStart)
                {
                    CrossLocalNotifications.Current.Show(course.Title, "The course " + course.Title + " is starting soon", 2000 + course.ID, course.Start.AddMinutes(-15));
                }

                if (course.NotifyEnd)
                {
                    CrossLocalNotifications.Current.Show(course.Title, "The course " + course.Title + " is ending soon", 2000 + course.ID, course.End.AddMinutes(-15));
                }
            }

            foreach(Assessment assess in assessList)
            {
                if (assess.NotifyStart)
                {
                    CrossLocalNotifications.Current.Show(assess.Title, "The course " + assess.Title + " is starting soon", 3000 + assess.ID, assess.Start.AddMinutes(-15));
                }

                if (assess.NotifyEnd)
                {
                    CrossLocalNotifications.Current.Show(assess.Title, "The course " + assess.Title + " is ending soon", 3000 + assess.ID, assess.End.AddMinutes(-15));
                }
            }

            App.Current.MainPage = new FlyoutContainer();
        }
    }
}
