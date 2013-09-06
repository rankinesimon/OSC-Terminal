using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OSC_Terminal
{
    /// <summary>
    /// Uses packet arrival data and size to produce a log of data rate which may be saved to file. Extends the packet counter class
    /// </summary>
    public class RateLogger : PacketCounter
    {
        /******************************************************
         *                    Properties
         ******************************************************/
        private List<int> packetSize = new List<int>();//used to store the size of recieved packets
        private List<logEntry> log = new List<logEntry>();//contents of log
        private float logInterval;//time between logs in s
        public logStatus logState {get; protected set;}//current status of log
        public String logFile { get; protected set; }//file to use for log
        protected System.Windows.Forms.Timer logTrigger;
        protected StreamWriter logWriter;


        /******************************************************
         *                   Enumerations
         ******************************************************/

        /// <summary>
        /// Used to describe current status of logging operations
        /// </summary>
        public enum logStatus {log_running, log_stopped, log_paused };

        /******************************************************
         *                   Exceptions
         ******************************************************/
        [Serializable()]
        public class logAlreadyRunningException : System.Exception
        {
            public logAlreadyRunningException() : base() { }
            public logAlreadyRunningException(string message) : base(message) { }
            public logAlreadyRunningException(string message, System.Exception inner) : base(message, inner) { }

            // A constructor is needed for serialization when an 
            // exception propagates from a remoting server to the client.  
            protected logAlreadyRunningException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
        }

        /******************************************************
         *                    Strctures
         ******************************************************/

        /// <summary>
        /// Represents one log entry
        /// </summary>
        private struct logEntry
        {
            DateTime timestamp; //timestamp of log entry
            int throughput;     //throughput for that log entry
        }

        /******************************************************
         *                      Methods
         ******************************************************/

        public RateLogger()
        {
            // Initialise variables
            prevPacketsReceived = 0;
            PacketsReceived = 0;
            logState = logStatus.log_stopped;

            // Setup timer
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        public float DataRate()
        {
            return PacketRate * 800;
        }
        
        public void startLog(int logInterval, string filePath){
            if (logState == logStatus.log_stopped)
            {
                //setup file to write to
                if (!File.Exists(@filePath))
                {
                    logWriter = File.CreateText(filePath);
                }
                else
                {
                    logWriter = new StreamWriter(@filePath);
                }
                logTrigger = new System.Windows.Forms.Timer();
                logTrigger.Interval = logInterval * 1000;
                logTrigger.Tick += new EventHandler(logEvent);
                logTrigger.Start();

                logState = logStatus.log_running;
            }
            else
            {
                throw new logAlreadyRunningException();
            }
        }

        public void pauseLog()
        {
            logTrigger.Stop();
            logState = logStatus.log_stopped;
        }

        public void stopLog()
        {
            logTrigger.Stop();
            
        }

        private void logEvent(object sender, EventArgs e)
        {

        }
    }
}
