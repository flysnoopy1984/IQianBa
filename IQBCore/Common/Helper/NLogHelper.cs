using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.Common.Helper
{
    public static class NLogHelper
    {
        private static Logger _FileLogger = LogManager.GetLogger("FileNLog");

        private static Logger _DbLogger = LogManager.GetLogger("DBLogger");

        public static void InfoTxt(string txt)
        {
            try
            {
                _FileLogger.Info(txt);
            }
            catch(Exception ex)
            {
                
            }

           
        }

        public static void ErrorTxt(string txt)
        {
            try
            {

                _FileLogger.Error(txt);
            }
            catch (Exception ex)
            {

            }
        }

        public static void InfoDb(string msg)
        {
            try
            {
                _DbLogger.Info(msg);
            }
            catch (Exception ex)
            {
              //  throw ex;
            }
        }

        public static void ErrorDb(string msg)
        {
            try
            {

                _DbLogger.Error(msg);
            }
            catch (Exception ex)
            {

            }
        }

    }
}
