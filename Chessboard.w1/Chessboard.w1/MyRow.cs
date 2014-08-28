using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFScheduler;

namespace Chessboard.w1
{
    class MyRow : ISchedulerRoomData
    {
        public int Number { get; set; }

        public int CompareTo(object obj)
        {
            return Number.CompareTo(((MyRow)obj).Number);
        }
    }
}
