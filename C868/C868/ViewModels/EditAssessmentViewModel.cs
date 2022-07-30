using C868.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace C868.ViewModels
{
    class EditAssessmentViewModel : INotifyPropertyChanged
    {
        private int _ID, _cID;
        private string _Name;
        private DateTime _Start, _End;
        private bool _sNotify, _eNotify;
        private AssessmentType _type;
        

        public int ID {
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
        public string Name {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public DateTime aStart {
            get
            {
                return _Start;
            }
            set
            {
                if (_Start != value)
                {
                    _Start = value;
                    OnPropertyChanged("aStart");
                }
            }
        }
        public DateTime aEnd{
            get
            {
                return _End;
            }
            set
            {
                if (_End != value)
                {
                    _End = value;
                    OnPropertyChanged("aEnd");
                }
            }
        }
        public bool sNotify {
            get
            {
                return _sNotify;
            }
            set
            {
                if (_sNotify != value)
                {
                    _sNotify = value;
                    OnPropertyChanged("sNotify");
                }
            }
        }
        public bool eNotify {
            get
            {
                return _eNotify;
            }
            set
            {
                if (_eNotify != value)
                {
                    _eNotify = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public AssessmentType type {
            get
            {
                return _type;
            }
            private set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        public int CID
        {
            get
            {
                return _cID;
            }
            set
            {
                if(_cID != value)
                {
                    _cID = value;
                    OnPropertyChanged("CID");
                }
            }
        }

        public Course Course { get; private set; }

        public EditAssessmentViewModel(int aID, int aType, int cID)
        {
            ID = aID;
            type = (AssessmentType)aType;

            Name = App.assessList[(App.assessList.ToList<Assessment>()).FindIndex(i => i.ID == ID)].Title;
            aStart = App.assessList[(App.assessList.ToList<Assessment>()).FindIndex(i => i.ID == ID)].Start;
            aEnd = App.assessList[(App.assessList.ToList<Assessment>()).FindIndex(i => i.ID == ID)].End;
            sNotify = App.assessList[(App.assessList.ToList<Assessment>()).FindIndex(i => i.ID == ID)].NotifyStart;
            eNotify = App.assessList[(App.assessList.ToList<Assessment>()).FindIndex(i => i.ID == ID)].NotifyEnd;

            Course = App.courseList[(App.courseList.ToList<DisplayCourse>()).FindIndex(i => i.ID == cID)];
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected async virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        }
    }

    public class ObjEnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (AssessmentType)value == AssessmentType.Performance;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0;
        }
    }

    public class ShowOAConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (AssessmentType)value == AssessmentType.Objective;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 0 : 1;
        }
    }
}
