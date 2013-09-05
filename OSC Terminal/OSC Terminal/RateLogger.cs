using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSC_Terminal
{
    //Uses packet arrival data and size to produce a log of data rate which may be saved to file

    class RateLogger
    {
        List<int> packetSize = new List<int>();//used to store the size of recieved packets
        List<logEntry> log = new List<logEntry>();//contents of log
        int packetsRecieved;
        int logFrequency;

        public void RateLogger()
        {
            //default 1Hz log frequency
            logFrequency = 1;
        }

        public void RateLogger(int frequency)
        {
            //user chooses log rate
            logFrequency = frequency;
        }

        struct logEntry{
            DateTime timestamp; //timestamp of log entry
            int throughput;     //throughput for that log entry
        }

        //handles one clock tick
        void timer_tick(object sender, EventArgs e)
        {
            
        }
    }
}
