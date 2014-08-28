using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using WPFScheduler;

namespace Chessboard.w1
{
    public class MyClass : ISchedulerItemData, INotifyPropertyChanged
    {
        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged("Date");}
        }
        

        public int Duration { get; set; }

        public string Name { get; set; }

        private int room;

        public int Room
        {
            get { return room; }
            set { room = value; OnPropertyChanged("Room"); }
        }
        

        public int CompareTo(object obj)
        {
            return this.Date.CompareTo(((MyClass)obj).Date);
        }
        public override string ToString()
        {
            //return string.Format("Date = {0}, Duration = {1}, Room = {2}", Date.ToShortDateString(), Duration, Room);
            return string.Format("{0}. Room = {1}", Name, Room);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
