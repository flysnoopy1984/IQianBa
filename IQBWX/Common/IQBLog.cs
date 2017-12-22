using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace IQBWX.Common
{
    public class IQBLog
    {
        private  string logFile;
        private StreamWriter writer;
        private FileStream fileStream = null;

        public IQBLog(string fileName)
        {
            logFile = fileName;
            CreateDirectory(logFile);
        }

        public IQBLog()
        {
            logFile = ConfigurationManager.AppSettings["logPath"];
            CreateDirectory(logFile);
        }

        public void  log(string info)
        {

            try
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(logFile);
                if (!fileInfo.Exists)
                {
                    fileStream = fileInfo.Create();
                    writer = new StreamWriter(fileStream);
                }
                else
                {
                    fileStream = fileInfo.Open(FileMode.Append, FileAccess.Write);
                    writer = new StreamWriter(fileStream);
                }
               

            }
            catch
            {

            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
        }

        public void CreateDirectory(string infoPath)
        {
            DirectoryInfo directoryInfo = Directory.GetParent(infoPath);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
        }
    }
}