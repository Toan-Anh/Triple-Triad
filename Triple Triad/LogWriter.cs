using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Triple_Triad
{
    /// <summary>
    /// A Logging class implementing the Singleton pattern and an internal Queue to be flushed perdiodically
    /// From http://blog.bondigeek.com/2011/09/08/a-simple-c-thread-safe-logging-class/
    /// </summary>
    public class LogWriter
    {
        private static LogWriter _Instance;
        private static Queue<Log> _LogQueue;
        private static string logDir = "E:/";
        private static string logFile = "TripleTriadLog.txt";
        private static int maxLogAge = int.Parse("1");
        private static int queueSize = int.Parse("10");
        private static DateTime _LastFlushed = DateTime.Now;

        /// <summary>
        /// Private constructor to prevent instance creation
        /// </summary>
        private LogWriter()
        {
        }

        ~LogWriter()
        {
            FlushLog();
        }

        /// <summary>
        /// An LogWriter instance that exposes a single instance
        /// </summary>
        public static LogWriter Instance
        {
            get
            {
                // If the instance is null then create one and init the Queue
                if (_Instance == null)
                {
                    _Instance = new LogWriter();
                    _LogQueue = new Queue<Log>();
                }
                return _Instance;
            }
        }

        /// <summary>
        /// The single instance method that writes to the log file
        /// </summary>
        /// <param name="message">The message to write to the log</param>
        public void WriteToLog(string message)
        {
            // Lock the queue while writing to prevent contention for the log file
            lock (_LogQueue)
            {
                // Create the entry and push to the Queue
                Log logEntry = new Log(message);
                _LogQueue.Enqueue(logEntry);

                // If we have reached the Queue Size then flush the Queue
                if (_LogQueue.Count >= queueSize || DoPeriodicFlush())
                {
                    FlushLog();
                }
            }
        }

        private bool DoPeriodicFlush()
        {
            TimeSpan logAge = DateTime.Now - _LastFlushed;
            if (logAge.TotalSeconds >= maxLogAge)
            {
                _LastFlushed = DateTime.Now;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Flushes the Queue to the physical log file
        /// </summary>
        private void FlushLog()
        {
            while (_LogQueue.Count > 0)
            {
                Log entry = _LogQueue.Dequeue();
                string logPath = logDir + entry.LogDate + "_" + logFile;

                // This could be optimised to prevent opening and closing the file for each write
                using (FileStream fs = File.Open(logPath, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter log = new StreamWriter(fs))
                    {
                        log.WriteLine(string.Format("{0}:\t{1}", entry.LogTime, entry.Message));
                    }
                }
            }
        }
    }


    /// <summary>
    /// A Log class to store the message and the Date and Time the log entry was created
    /// </summary>  
    public class Log
    {
        public string Message { get; set; }
        public string LogTime { get; set; }
        public string LogDate { get; set; }


        public Log(string message)
        {
            Message = message;
            LogDate = DateTime.Now.ToString("yyyy-MM-dd");
            LogTime = DateTime.Now.ToString("hh:mm:ss:fff tt");
        }
    }
}
