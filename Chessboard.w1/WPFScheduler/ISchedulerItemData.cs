using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScheduler
{
    public interface ISchedulerItemData : IComparable
    {
        DateTime Date { get; set; }
        int Duration { get; set; }
        int Room { get; set; }
    }
}
