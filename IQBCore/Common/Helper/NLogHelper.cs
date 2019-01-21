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
        private static Logger _FileErrorLogger = LogManager.GetLogger("ErrorNLog");

        private static Logger _GameInfoLogger = LogManager.GetLogger("GameInfoLog");
        private static Logger _GameErrorLogger = LogManager.GetLogger("GameErrorLog");

        private static Logger _DbLogger = LogManager.GetLogger("DBLogger");


        public static void GameInfo(string txt)
        {
            try
            {
                _GameInfoLogger.Info(txt);
            }
            catch (Exception ex)
            {

            }
        }

        public static void GameError(string txt)
        {
            try
            {
                _GameErrorLogger.Info(txt);
            }
            catch (Exception ex)
            {

            }
        }

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

                _FileErrorLogger.Error(txt);
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
