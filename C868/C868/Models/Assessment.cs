using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C868.Models
{
    public enum AssessmentType { Performance, Objective };

    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public AssessmentType Type {get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public bool NotifyStart { get; set; }
        public DateTime End { get; set; }
        public bool NotifyEnd { get; set; }
        public int Course { get; set; }

        public Assessment() { }

        public Assessment(int type,string title,DateTime start, bool snotify,DateTime end, bool enotify, int iCourse = 0)
        {
            Type = (AssessmentType)type;
            Title = title;
            Start = start;
            NotifyStart = snotify;
            End = end;
            NotifyEnd = enotify;
            Course = iCourse;
        }

        public int ToInt()
        {
            if(ID != null)
            {
                return ID;
            }
            else
            {
                return 0;
            }
        }

    }    
}
