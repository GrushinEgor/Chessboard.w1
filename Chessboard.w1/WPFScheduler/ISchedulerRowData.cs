using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScheduler
{
    public interface ISchedulerRoomData : IComparable
    {
        int Number { get; set; }
    }
}
