using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKHOSTING.Core
{
    public class Timer
    {
        public TimeSpan Interval { get; set; }
        public bool IsRunning { get; set; }
        public event EventHandler Tick;

        protected virtual void OnTick()
        {
            if (Tick != null)
            {
                Tick(this, new EventArgs());
            }
        }

        public async Task Start()
        {
            int sec
        }
    }
}
