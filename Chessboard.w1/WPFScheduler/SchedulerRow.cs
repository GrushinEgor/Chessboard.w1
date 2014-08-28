using System;
using System.Collections.Generic;
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
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:SchedulerRow/>
    ///
    /// </summary>
    public class SchedulerRow : Control
    {
        private List<object> cells;
        public List<object> Cells
        {
            get { return cells; }
            set { cells = value; }
        }



        public int CellsCount
        {
            get { return (int)GetValue(CellsCountProperty); }
            set { SetValue(CellsCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CellsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CellsCountProperty =
            DependencyProperty.Register("CellsCount", typeof(int), typeof(SchedulerRow), new FrameworkPropertyMetadata(
                0, 
                FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnCellsCountChanged)));

        private static void OnCellsCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
                ((SchedulerRow)d).BuildCells();
        }



        public int Room
        {
            get { return (int)GetValue(RoomProperty); }
            set { SetValue(RoomProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InternalRoom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RoomProperty =
            DependencyProperty.Register("Room", typeof(int), typeof(SchedulerRow),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure));


        private void BuildCells()
        {
            var newCells = new List<object>();
            for (int i = 0; i < CellsCount; i++)
            {
                var binding = new Binding("DataContext.CurrentDate");
                binding.Converter = new DateBrushConverter();
                binding.ConverterParameter = i;
                binding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Scheduler), 1);
                var cell = new SchedulerCell();
                cell.SetBinding(SchedulerCell.BackgroundProperty, binding);
                newCells.Add(cell);
            }
            Cells = newCells;
        }
        static SchedulerRow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SchedulerRow), new FrameworkPropertyMetadata(typeof(SchedulerRow)));
        }

    }
}
