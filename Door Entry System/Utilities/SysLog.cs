using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Door_Entry_System.Utilities
{
    public class SysLog
    {
        private bool FileExists;
        private string file = "log.txt";
        //ctor for when a new logger is created creates the date
        //using the default of log.txt if there isn't a passed in file name
        //this is for testing the logger and passing in a test file location
        public SysLog(): this("log.txt")
        {

        }
        public SysLog(string FILE)
        {
            file = FILE;
            if (!File.Exists(file))
                File.Create(file);
            //new session log line
            string newSessionMessage = "[NEW SESSION] AT " + dateString();
            logMessage(newSessionMessage);

        }
        //property for returning the current date time 
        public DateTime logDate
        {
            get { return DateTime.Now; }
        }
        //function for building the attempt log message line
        public void attempt(List<int> code, bool success)
        {
            int[] enteredCode = code.ToArray();
            string message = "";
            //if the bool failed or succeded
            string failed = success ? "SUCCESS" : "FAILIURE";
            //Adding the date and time to the log message
            message = "[LOCK ATTEMPT] AT "+ dateString();
            //adding the code enetered to the log
            message += "[CODE ENTERED]("+ string.Join("",enteredCode)+ ")";
            //adding wether or not the code failed or succeded.
            message += "[CODE WORKED](" + failed +")";
            logMessage(message);
        }
        //function for building the disabled log message line
        public void disabled()
        {
            string message = "[LOCK DISABLED] AT " + dateString();
            logMessage(message);

        }
        //function for returning the current date time as a log friendly string
        public string dateString()
        {
            return  "[DATE AND TIME]("+ logDate.ToString()+")";
        }
        //function for writing the log messgae to the file
        private void logMessage(string MessageLine)
        {
            //using the streamWriter to write a new line in the log file
            using (StreamWriter writer = new StreamWriter(file, true))
            {
                writer.WriteLine(MessageLine);   
            }

        }
    }
}
