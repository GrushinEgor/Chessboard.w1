using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFScheduler.ViewModels;
namespace WPFScheduler
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPFScheduler"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPFScheduler;assembly=WPFScheduler"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class Scheduler : Control
    {

        #region Properties
        private ViewModelBase viewModel = ViewModelBase.Instance;
        public int Range
        {
            get { return viewModel.Range; }
            set
            {
                if (value != viewModel.Range)
                    viewModel.Range = value;
            }
        }

        public List<ISchedulerItemData> Items
        {
            get { return viewModel.Items; }
            set
            {
                if (value != viewModel.Items)
                    viewModel.Items = value;
            }
        }
        public ObservableCollection<ISchedulerRoomData> Rows
        {
            get { return viewModel.Rows; }
            set
            {
                if (value != viewModel.Rows)
                    viewModel.Rows = value;
            }
        }
        public DateTime CurrentDate
        {
            get { return viewModel.CurrentDate; }
            set
            {
                if (value != viewModel.CurrentDate)
                    viewModel.CurrentDate = value;
            }
        }
        public DateTime MinDate
        {
            get { return viewModel.MinDate; }
            set
            {
                if (value != viewModel.MinDate)
                    viewModel.MinDate = value;
            }
        }
        public DateTime MaxDate
        {
            get { return viewModel.MaxDate; }
            set
            {
                if (value != viewModel.MaxDate)
                    viewModel.MaxDate = value;
            }
        }
        
        

        #endregion
        #region Constructors
        static Scheduler()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Scheduler), new FrameworkPropertyMetadata(typeof(Scheduler)));
        }

        public Scheduler()
        {
            CurrentDate = DateTime.Now;
            MaxDate = DateTime.MaxValue;
            MinDate = DateTime.MinValue;
            Range = 30;
            this.DataContext = viewModel;
            SizeChanged += Scheduler_SizeChanged;
        }

        void Scheduler_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize != e.NewSize)
                viewModel.Measure(e.NewSize);
        }
        #endregion
        
    }
}
