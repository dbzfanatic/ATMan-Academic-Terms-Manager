using System;
using System.Collections.Generic;
using System.Text;

namespace C868.ViewModels
{
    public class EditNotesViewModel
    {
        private int _nID;

        public int ID {
            get
            {
                return _nID;
            }
            set
            {
                if(_nID != value)
                {
                    _nID = value;
                }
            }
        }

        public string nText { get; set; }
        public int classID { get; set; }

        public EditNotesViewModel(int nid, string text, int cID){
            ID = nid;
            nText = text;
            classID = cID;
        }    
    }
}
