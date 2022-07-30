using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.IO;
using Xamarin.Essentials;

using C868.Models;

namespace C868.Services
{
    public static class DatabaseService
    {
        private static SQLiteAsyncConnection _db;

        static async Task Init()
        {
            if (_db != null)
            {
                return;
            }
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "atm.db");
            _db = new SQLiteAsyncConnection(dbPath);
            await _db.CreateTableAsync<Term>();
            await _db.CreateTableAsync<Course>();
            await _db.CreateTableAsync<Assessment>();
            await _db.CreateTableAsync<Note>();
        }

        #region Add Classes
        public static async Task<int> AddTerm(string name, DateTime start, DateTime end)
        {
            await Init();
            var term = new Term
            {
                Name = name,
                Start = start,
                End = end
            };

            var id = await _db.InsertAsync(term);
            return term.ID;
        }

        public static async Task<int> AddTerm(Term term)
        {
            await Init();
            var id = await _db.InsertAsync(term);
            return term.ID;
        }

        public static async Task<int> AddCourse(string name, string instname, string instphone, string instmail, DateTime start, bool snotify, DateTime end, bool enotify, string status, int term, int? pass = null, int? oass = null)
        {
            await Init();
            var course = new Course
            {
                Title = name,
                instName = instname,
                instPHone = instphone,
                insteMail = instmail,
                Start = start,
                End = end,
                NotifyStart = snotify,
                NotifyEnd = enotify,
                PAssessment = (int)pass,
                OAssessment = (int)oass,
                Status = status,
                Term = term
            };

            var id = await _db.InsertAsync(course);
            return course.ID;
        }

        public static async Task<int> AddCourse(Course course)
        {
            await Init();
            var id = await _db.InsertAsync(course);

            return course.ID;
        }

        public static async Task<int> AddAssessment(Assessment assess)
        {
            await Init();
            var id = await _db.InsertAsync(assess);

            return assess.ID;
        }

        public static async Task<int> AddNote(Note note)
        {
            await Init();
            var id = await _db.InsertAsync(note);

            return note.ID;
        }
        #endregion

        #region Remove Classes
        public static async Task RemoveTerm(int id)
        {
            await Init();
            await _db.DeleteAsync<Term>(id);
        }

        public static async Task RemoveCourse(int id)
        {
            await Init();
            await _db.DeleteAsync<Course>(id);
        }

        public static async Task RemoveNote(int id)
        {
            await Init();
            await _db.DeleteAsync<Note>(id);
        }

        public static async Task RemoveAssessment(int id)
        {
            await Init();
            await _db.DeleteAsync<Assessment>(id);
        }

        #endregion

        #region Get Classes
        public static async Task<IEnumerable<Term>> GetTerms()
        {
            await Init();

            var terms = await _db.Table<Term>().ToListAsync();

            return terms;
        }

        
        public static async Task<IEnumerable<Course>> GetCourses()
        {
            await Init();

            var courses = await _db.Table<Course>().ToListAsync();

            return courses;
        }

        public static async Task<IEnumerable<Assessment>> GetAssessments()
        {
            await Init();

            var assessments = await _db.Table<Assessment>().ToListAsync();

            return assessments;
        }

        public static async Task<IEnumerable<Note>> GetNotes()
        {
            await Init();

            var notes = await _db.Table<Note>().ToListAsync();

            return notes;
        }
        #endregion

        #region Update Classes
        public static async Task UpdateTerm(int id, string name, DateTime start, DateTime end)
        {
            await Init();

            var termQuery = await _db.Table<Term>().Where(term => term.ID == id).FirstOrDefaultAsync();

            if (termQuery != null)
            {
                termQuery.Name = name;
                termQuery.Start = start;
                termQuery.End = end;

                await _db.UpdateAsync(termQuery);
            }
        }

        public static async Task UpdateCourse(int id, string name, string instname, string instphone, string instmail, DateTime start, bool snotify, DateTime end, bool enotify, string status, int term, int? pass = null, int? oass = null)
        {
            await Init();

            if(id == 0)
            {
                id++;
            }

            var courseQuery = await _db.Table<Course>().Where(course => course.ID == id).FirstOrDefaultAsync();

            if(courseQuery != null)
            {
                courseQuery.Title = name;
                courseQuery.instName = instname;
                courseQuery.instPHone = instphone;
                courseQuery.insteMail = instmail;
                courseQuery.Start = start;
                courseQuery.End = end;
                courseQuery.NotifyStart = snotify;
                courseQuery.NotifyEnd = enotify;                
                courseQuery.Status = status;
                courseQuery.Term = term;
                courseQuery.PAssessment = (int)pass;
                courseQuery.OAssessment = (int)oass;

                await _db.UpdateAsync(courseQuery);
            };
        }

        public static async Task UpdateNote(int id, string text, int courseID)
        {
            await Init();

            var noteQuery = await _db.Table<Note>().Where(note => note.ID == id).FirstOrDefaultAsync();

            if(noteQuery != null)
            {
                noteQuery.noteText = text;
                noteQuery.classID = courseID;

                await _db.UpdateAsync(noteQuery);
            }
        }

        public static async Task UpdateAssessment(int id, int assType, string title, DateTime start, bool nstart, DateTime end, bool nend, int courseID)
        {
            await Init();

            var assessmentQuery = await _db.Table<Assessment>().Where(Assessment => Assessment.ID == id).FirstOrDefaultAsync();

            if (assessmentQuery != null)
            {
                assessmentQuery.Type = (AssessmentType)assType;
                assessmentQuery.Title = title;
                assessmentQuery.Start = start;
                assessmentQuery.NotifyStart = nstart;
                assessmentQuery.End = end;
                assessmentQuery.NotifyEnd = nend;
                assessmentQuery.Course = courseID;

                await _db.UpdateAsync(assessmentQuery);
            }
        }
        #endregion

        #region AdminData
        public static async Task<List<string[]>> SearchDB(string searchTerm,bool term, bool course, bool assess)
        {
            await Init();

            List<string[]> results = new List<string[]>();
            if (term)
            {
                var termRes = await _db.QueryAsync<Term>($"SELECT * FROM Term WHERE Name LIKE '%{searchTerm}%'");
                foreach (Term termy in termRes)
                {
                    results.Add(new string[] { termy.Name, termy.Start.ToString("MM/dd/yy"), termy.End.ToString(),"" });
                }
            }
            if (course)
            {
                var courseRes = await _db.QueryAsync<Course>($"Select * from Course Where title like '%{searchTerm}%'");
                foreach (Course courses in courseRes)
                {
                    results.Add(new string[] { courses.Title, courses.Start.ToString("MM/dd/yy"), courses.End.ToString(), courses.Status });
                }
            }
            if (assess)
            {
                var assessRes = await _db.QueryAsync<Assessment>($"Select * from Assessment Where title like '%{searchTerm}%'");
                foreach(Assessment asses in assessRes)
                {
                    results.Add(new string[] { asses.Title, asses.Start.ToString("MM/dd/yy"), asses.End.ToString(),"" });
                }
            }
            return results;
        }

        public static async Task<List<string[]>> CourseEndReport(String endFilter)
        {
            await Init();

            List<string[]> results = new List<string[]>();

            var endCourseRes = await _db.QueryAsync<Course>($"SELECT * FROM Course WHERE end <= '{endFilter}'");
            foreach(Course res in endCourseRes)
            {
                results.Add(new string[] { res.Title, res.Status, res.Start.ToString("MM/dd/yy"), res.End.ToString() });
            }

            return results;
        }

        public static async Task<List<string[]>> CoursesCompletedReport()
        {
            await Init();

            List<string[]> results = new List<string[]>();

            var compCourseRes = await _db.QueryAsync<Course>("SELECT * FROM Course WHERE status == 'Completed'");

            foreach (Course res in compCourseRes)
            {
                results.Add(new string[] { res.Title, res.Start.ToString("MM/dd/yy"), res.End.ToString() });
            }

            return results;
        }

        public static async Task<List<string[]>> AllCoursesReport()
        {
            await Init();

            List<string[]> results = new List<string[]>();

            var allCourseRes = await GetCourses();

            foreach (Course res in allCourseRes)
            {
                results.Add(new string[] { res.Title, res.Status, res.Start.ToString("MM/dd/yy"), res.End.ToString() });
            }

            return results;
        }

        public static async Task<List<string[]>> AssessmentEndReport(String endFilter)
        {
            await Init();

            List<string[]> results = new List<string[]>();

            var endAssessRes = await _db.QueryAsync<Assessment>($"SELECT * FROM Assessment WHERE end <= '{endFilter}'");
            foreach (Assessment assess in endAssessRes)
            {
                results.Add(new string[] { assess.Title, assess.Start.ToString("MM/dd/yy"), assess.End.ToString() });
            }

            return results;
        }

        #endregion
    }
}
