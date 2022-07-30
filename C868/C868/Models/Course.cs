using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using C868.Views;
using SQLite;

using System.Linq;

namespace C868.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Title { get; set; }
        public string instName { get; set; }
        public string instPHone { get; set; }
        public string insteMail { get; set; }
        public DateTime Start { get; set; }
        public bool NotifyStart { get; set; }
        public DateTime End { get; set; }
        public bool NotifyEnd { get; set; }
        public string Status { get; set; }
        public int PAssessment { get; set; }
        public int OAssessment { get; set; }
        public int Term { get; set; }

        

        public Course() { }
        public Course(string name, string instname, string instphone, string instmail, DateTime start, bool snotify, DateTime end, bool enotify, string status, int term, int? pass = null, int? oass = null)
        {
            Title = name;
            instName = instname;
            instPHone = instphone;
            insteMail = instmail;
            Start = start;
            End = end;
            NotifyStart = snotify;
            NotifyEnd = enotify;            
            Status = status;
            Term = term;

            if(pass != null)
            {
                PAssessment = (int)pass;
            }
            else
            {
                PAssessment = 0;
            }

            if (oass != null)
            {
                OAssessment = (int)oass;
            }
            else
            {
                OAssessment = 0;
            }
        }
    }  
    
    public class DisplayCourse : Course
    {
        public List<Note> Notes { get; set; }
        public new Assessment PAssessment { get; set; }
        public new Assessment OAssessment { get; set; }

        public DisplayCourse(string name, string instname, string instphone, string instmail, DateTime start, bool snotify, DateTime end, bool enotify, string status, int term, Assessment pass = null, Assessment oass = null, List<Note> notes = null)
        {
            Title = name;
            instName = instname;
            instPHone = instphone;
            insteMail = instmail;
            Start = start;
            End = end;
            NotifyStart = snotify;
            NotifyEnd = enotify;            
            Status = status;
            Term = term;

            if(pass != null)
            {                
                PAssessment = pass;
            }
            if(oass != null)
            {
                OAssessment = oass;
            }
            
            if(notes.Count != 0)
            {
                Notes = notes;
            }
            else
            {
                Notes = new List<Note>();
            }
            
        }

        public DisplayCourse(Course course, Assessment pass = null, Assessment oass = null, List<Note> notes = null)
        {
            ID = course.ID;
            Title = course.Title;
            instName = course.instName;
            instPHone = course.instPHone;
            insteMail = course.insteMail;
            Start = course.Start;
            End = course.End;
            NotifyStart = course.NotifyStart;
            NotifyEnd = course.NotifyEnd;            
            Status = course.Status;
            Term = course.Term;

            if(int.TryParse(course.OAssessment.ToString(),out int dispose) && oass == null && dispose != 0)
            {
                OAssessment = App.assessList[App.assessList.ToList<Assessment>().FindIndex(i => i.ID == dispose)];
            }
            else if(oass != null)
            {
                OAssessment = oass;
            }
            else
            {
                OAssessment = null;
            }

            if (int.TryParse(course.PAssessment.ToString(), out int _dispose) && pass == null && _dispose != 0)
            {
                PAssessment = App.assessList[App.assessList.ToList<Assessment>().FindIndex(i => i.ID == _dispose)];
            }
            else if (oass != null)
            {
                PAssessment = pass;
            }
            else
            {
                PAssessment = null;
            }

            if (notes == null)
            {
                Notes = new List<Note>();
            }
        }

    }
}
