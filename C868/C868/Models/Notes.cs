using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using C868.Views;
using SQLite;
using System.Linq;

namespace C868.Models
{
    public class Note : INotifyPropertyChanged
    {
        private string _text;
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string noteText
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    onPropertyChanged("noteText");
                }
            }
        }
        public int classID { get; set; }

        public Note()
        {

        }

        public Note(string text, int courseID)
        {
            noteText = text;
            classID = courseID;
        }

        public ICommand ShareNoteCommand => new Command<string>((note) =>
        {
            ShareNote(note);
        });

        public async void ShareNote(string noteText)
        {
            await Share.RequestAsync(new ShareTextRequest { Text = noteText, Title = "Share Note" });
        }

        public ICommand EditNoteCommand => new Command<int>((num) =>
        {
            EditNote(num);
        });

        public async void EditNote(int noteNum)
        {
            noteNum -= 2;

            await App.Current.MainPage.Navigation.PushModalAsync(new EditNotesView(noteNum, App.notesList[(App.notesList.ToList<Note>()).FindIndex(i => i.ID == ID)].noteText, App.notesList[(App.notesList.ToList<Note>()).FindIndex(i => i.ID == ID)].classID));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
