using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C868.Models
{
    public class Term
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        

        public Term() {  }
        public Term(string name, DateTime start, DateTime end)
        {
            Name = name;
            Start = start;
            End = end;
        }
    }

    public class DisplayTerm : Term
    {
        public List<DisplayCourse> Courses { get; set; }

        public DisplayTerm(string name, DateTime start, DateTime end, List<DisplayCourse> courses = null)
        {
            Name = name;
            Start = start;
            End = end;
            if(courses == null)
            {
                Courses = new List<DisplayCourse>();
            }
            else
            {
                Courses = courses;
            }
        }

        public DisplayTerm(Term term)
        {
            ID = term.ID;
            Name = term.Name;
            Start = term.Start;
            End = term.End;
            Courses = new List<DisplayCourse>();
        }
    }


}
