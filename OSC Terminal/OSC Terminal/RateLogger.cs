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
    class RateLogger : PacketCounter
    {
        /******************************************************
         *                    Properties
         ******************************************************/
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
        public class logNotRunningException : System.Exception
        {
            public logNotRunningException() : base() { }
            public logNotRunningException(string message) : base(message) { }
            public logNotRunningException(string message, System.Exception inner) : base(message, inner) { }

            // A constructor is needed for serialization when an 
            // exception propagates from a remoting server to the client.  
            protected logNotRunningException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
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

        /// <summary>
        /// Retrieves the current data rate in bit/s
        /// </summary>
        /// <returns></returns>
        public float DataRate()
        {
            return PacketRate * 800;
        }
        
        /// <summary>
        /// Sets up the log file ready to run. 
        /// Throws a logAlreadyRunningException if the logger is already running
        /// </summary>
        /// <param name="logInterval">Time between log entries</param>
        /// <param name="filePath">Location of log file</param>
        public void startLog(int logInterval, string filePath){
            if(logState == logStatus.log_stopped){
                //setup file to write to
                if (!File.Exists(@filePath)){//file does not already exist
                    logWriter = File.CreateText(filePath);
                }else{//file does already exist
                    logWriter = new StreamWriter(@filePath);
                }
                //intialise log clock
                logTrigger = new System.Windows.Forms.Timer();
                logTrigger.Interval = logInterval * 1000;
                logTrigger.Tick += new EventHandler(logEvent);
                logTrigger.Start();

                logState = logStatus.log_running;
            }else{
                throw new logAlreadyRunningException();
            }
        }

        /// <summary>
        /// Resumes logging operation after being suspended with pause operation
        /// </summary>
        public void resumeLog()
        {
            //check log is paused
            switch(logState){
                case logStatus.log_running:
                    throw new logAlreadyRunningException();
                case logStatus.log_stopped:
                    throw new logNotRunningException();
                case logStatus.log_paused:
                    logTrigger.Start();
                    logState = logStatus.log_running;
                    break;
            }
        }

        /// <summary>
        /// temporarily suspend logging operation
        /// </summary>
        public void pauseLog()
        {
            if (logState == logStatus.log_running)
            {
                logTrigger.Stop();
                logState = logStatus.log_paused;
            }
            else
            {
                throw new logNotRunningException();
            }
        }

        /// <summary>
        /// end the log operation and close the file
        /// </summary>
        public void stopLog()
        {
            logTrigger.Stop();
            logWriter.Close();
            logState = logStatus.log_stopped;
        }

        /// <summary>
        /// Add another entry to the log
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logEvent(object sender, EventArgs e)
        {
            logWriter.WriteLine(DateTime.Now + "\t" + PacketRate + "\t" + DataRate());
        }
    }
}
