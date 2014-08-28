using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace WPFScheduler.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region Properties
        private DateTime minDate;
        public DateTime MinDate
        {
            get { return minDate; }
            set { minDate = value; OnPropertyChanged("MinDate"); }
        }
        private DateTime maxDate;
        public DateTime MaxDate
        {
            get { return maxDate; }
            set { maxDate = value; OnPropertyChanged("MaxDate");}
        }

        /// <summary>
        /// Height of headers for each column
        /// </summary>
        private int columnHeadersHeight;
        public int ColumnHeadersHeight
        {
            get { return columnHeadersHeight; }
            set { columnHeadersHeight = value; OnPropertyChanged("ColumnHeadersHeight"); }
        }
        /// <summary>
        /// Height of each row
        /// </summary>
        private int rowHeight;
        public int RowHeight
        {
            get { return rowHeight; }
            set { rowHeight = value; OnPropertyChanged("RowHeight"); }
        }
        /// <summary>
        /// Width of each row
        /// </summary>
        private double rowWidth;
        public double RowWidth
        {
            get { return rowWidth; }
            set { rowWidth = value; OnPropertyChanged("RowWidth");  }
        }
        
        /// <summary>
        /// Width of each row headers
        /// </summary>
        private int rowHeadersWidth;
        public int RowHeadersWidth
        {
            get { return rowHeadersWidth; }
            set { rowHeadersWidth = value; OnPropertyChanged("RowHeadersWidth"); }
        }
        
        /// <summary>
        /// Width of each column
        /// </summary>
        private int columnWidth;
        public int ColumnWidth
        {
            get { return columnWidth; }
            set { columnWidth = value; OnPropertyChanged("ColumnWidth"); }
        }
        /// <summary>
        /// Width of scheduler
        /// </summary>
        private double width;
        public double Width
        {
            get { return width; }
            set { width = value;  OnPropertyChanged("Width"); }
        }
        /// <summary>
        /// Height of scheduler
        /// </summary>
        private double height;
        public double Height
        {
            get { return height; }
            set { height = value;  OnPropertyChanged("Height"); }
        }
        
        


        private ObservableCollection<ISchedulerItemData> selectedData;

        public ObservableCollection<ISchedulerItemData> SelectedData
        {
            get { return selectedData; }
            set { selectedData = value; OnPropertyChanged("SelectedData"); }
        }
        
        


        private DateTime currentDate;
        public DateTime CurrentDate
        {
            get { return currentDate; }
            set
            {
                SelectedData = Loader.Instance.FilterData(SelectedData, currentDate, value, Range);
                currentDate = value;
                OnPropertyChanged("CurrentDate");
            }
        }
        private int range;

        public int Range
        {
            get { return range; }
            set { range = value; OnPropertyChanged("Range"); CreateHeaders(); }
        }
        private string msg;

        public string Msg
        {
            get { return msg; }
            set { msg = value; OnPropertyChanged("Msg"); }
        }
        
        private List<ISchedulerItemData> items;

        public List<ISchedulerItemData> Items
        {
            get { return items; }
            set
            {
                items = value;
                items.Sort();
                SelectedData = Loader.Instance.FirstLoadData(value, CurrentDate, Range);
                OnPropertyChanged("Items");
            }
        }
        private ObservableCollection<ISchedulerRoomData> rows;

        public ObservableCollection<ISchedulerRoomData> Rows
        {
            get { return rows; }
            set { rows = value; OnPropertyChanged("Rows"); }
        }



        private List<HeaderItem> headers;

        public List<HeaderItem> Headers
        {
            get { return headers; }
            set { headers = value; OnPropertyChanged("Headers"); }
        }
        #endregion
        #region Commands
        #region IncreaseDate
        private RelayCommand increaseDateCommand;
        public ICommand IncreaseDateCommand
        {
            get
            {
                if (increaseDateCommand == null)
                    increaseDateCommand = new RelayCommand(IncreaseDate);
                return increaseDateCommand;
            }
        }

        private void IncreaseDate(object obj)
        {
            CurrentDate = CurrentDate.AddDays(1);
        }
        #endregion
        #region DecreaseDate
        private RelayCommand decreaseDateCommand;
        public ICommand DecreaseDateCommand
        {
            get
            {
                if (decreaseDateCommand == null)
                    decreaseDateCommand = new RelayCommand(DecreaseDate);
                return decreaseDateCommand;
            }
        }

        private void DecreaseDate(object obj)
        {
            CurrentDate = CurrentDate.AddDays(-1);
        }
        #endregion
        #endregion
        #region Methods
        private void CreateHeaders()
        {
            var newHeaders = new List<HeaderItem>();
            for (int i = 0; i < Range; i++)
            {
                var header = new HeaderItem();
                var datebinding = new Binding("CurrentDate");
                header.DataContext = this;
                datebinding.Converter = new DateShiftConverter();
                datebinding.ConverterParameter = i;
                header.SetBinding(HeaderItem.HeaderProperty, datebinding);
                var brushBinding = new Binding("CurrentDate");
                brushBinding.Converter = new DateHeaderBrushConverter();
                brushBinding.ConverterParameter = i;
                header.SetBinding(HeaderItem.BackgroundProperty, brushBinding);

                newHeaders.Add(header);
            }
            Headers = newHeaders;
        }
        public Size Measure(Size constraint)
        {
            var desiredSize = new Size();
            desiredSize.Height = constraint.Height;
            ColumnWidth = (int)((constraint.Width - RowHeadersWidth - 18) / Range);
            desiredSize.Width = RowHeadersWidth + ColumnWidth * Range + 18;
            RowWidth = desiredSize.Width;
            Msg = string.Format("{0}", ColumnWidth.ToString());
            return desiredSize;
        }

        private void AlertAboutMeasure(Size size)
        {
            Msg = string.Format("{0}", size.ToString());
        }
        #endregion
        #region Singleton

        private static readonly ViewModelBase instance = new ViewModelBase();

        public static ViewModelBase Instance
        {
            get { return instance; }
        }

        protected ViewModelBase() 
        {
            ColumnHeadersHeight = 18;
            RowHeight = 20;
            RowHeadersWidth = 100;
            Width = double.PositiveInfinity;
            Height = double.PositiveInfinity;
        }

        #endregion
        #region INotifyProperty
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
