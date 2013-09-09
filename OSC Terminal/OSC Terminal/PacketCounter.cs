﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSC_Terminal
{
    /// <summary>
    /// Packet counter. Tracks number of packets received and packet rate.
    /// </summary>
    class PacketCounter
    {
        /// <summary>
        /// Timer to calculate packet rate.
        /// </summary>
        protected System.Windows.Forms.Timer timer;

        /// <summary>
        /// Number of packets received.
        /// </summary>
        public int PacketsReceived { get; protected set; }

        /// <summary>
        /// Packet receive rate as packets per second.
        /// </summary>
        public int PacketRate { get; protected set; }

        /// <summary>
        /// Used to calculate packet rate.
        /// </summary>
        protected DateTime prevTime;

        /// <summary>
        /// Used to calculate packet rate.
        /// </summary>
        protected int prevPacketsReceived;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PacketCounter()
        {
            // Initialise variables
            prevPacketsReceived = 0;
            PacketsReceived = 0;

            // Setup timer
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        /// <summary>
        /// Increments packet counter.
        /// </summary>
        public void Increment()
        {
            PacketsReceived++;
        }

        // Zeros packet counter.
        public void Reset()
        {
            prevPacketsReceived = 0;
            PacketsReceived = 0;
            PacketRate = 0;
        }

        /// <summary>
        /// timer Tick event to calculate packet rate.
        /// </summary>
        public void timer_Tick(object sender, EventArgs e)
        {
            DateTime nowTime = DateTime.Now;
            TimeSpan t = nowTime - prevTime;
            prevTime = nowTime;
            //avoid deviding by zero
            if (t.Seconds == 0 && t.Milliseconds == 0)
            {
                PacketRate = 0;
            }
            else
            {
                PacketRate = (int)((float)(PacketsReceived - prevPacketsReceived) / ((float)t.Seconds + (float)t.Milliseconds * 0.001f));
            }
            prevPacketsReceived = PacketsReceived;
        }
    }
}