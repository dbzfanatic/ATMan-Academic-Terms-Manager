using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using C868.Models;

namespace C868.ViewModels
{   

    class EditTermViewModel : INotifyPropertyChanged
    {

        private int _ID;
        private string _Name;
        private DateTime _Start, _End;
        private List<DisplayCourse> _courseList;

        public int tID {
            get 
            {
                return _ID;
            }
            private set 
            {
                if(_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }
        public string tName
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged("tName");
                }

            }
        }

        public DateTime tStart
        {
            get
            {
                return _Start;
            }
            set
            {
                if (_Start != value)
                {
                    _Start = value;
                    OnPropertyChanged("tStart");
                }

            }
        }

        public DateTime tEnd
        {
            get
            {
                return _End;
            }
            set
            {
                if (_End != value)
                {
                    _End = value;
                    OnPropertyChanged("tEnd");
                }
            }
        }

        public List<DisplayCourse> tCourseList {
            get
            {
                return _courseList;
            }
            set
            {
                if (_courseList != value)
                {
                    _courseList = value;
                    OnPropertyChanged("tCourseList");
                }
                
            } 
        }

        public EditTermViewModel(int ID)
        {
            tID = ID;

            tName = App.termList[App.termList.ToList<Term>().FindIndex(i => i.ID == tID)].Name;
            tStart = App.termList[App.termList.ToList<Term>().FindIndex(i => i.ID == tID)].Start;
            tEnd = App.termList[App.termList.ToList<Term>().FindIndex(i => i.ID == tID)].End;
            List<DisplayCourse> tempList = App.courseList.Where(Course => Course.Term == ID).ToList();
            tCourseList = tempList;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected async virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

    }
}
