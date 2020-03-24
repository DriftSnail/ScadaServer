using System;
using System.Collections.Generic;
using System.IO;
using log4net;


namespace SXLibrary
{
    public  static class WriteLog
    {


        private static void SetConfigFile()
        {
            if (!File.Exists(AppDomain.CurrentDomain.SetupInformation.ApplicationBase+"Log4net.config"))
            {
                FileObj.WriteFile(
                    AppDomain.CurrentDomain.SetupInformation.ApplicationBase+"Log4net.config",
                    Properties.Resources.log4netconfig, false);
            } 
        }

        /// <summary>
        /// 写入调试的日志内容
        /// </summary>
        /// <param name="msg"> 写入的内容</param>
        /// <param name="className"> 写日志发生在哪个类中</param>
        public static void Write(string msg,Type className=null)
        {
            SetConfigFile();
            if (className == null)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("null");
                log.Debug(msg);
            }
            else
            {
                log4net.ILog log = log4net.LogManager.GetLogger(className);
                log.Debug(msg);
            }
          
          
        }
        /// <summary>
        /// 写入调试的日志内容并Throw
        /// </summary>
        /// <param name="msg"> 写入的内容</param>
        /// <param name="className"> 写日志发生在哪个类中</param>
        public static void WriteAndThrow(string msg, Type className = null)
        {
            SetConfigFile();
            if (className == null)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("null");
                log.Debug(msg);
            }
            else
            {
                log4net.ILog log = log4net.LogManager.GetLogger(className);
                log.Debug(msg);
            }
            throw new Exception(msg);
        }

        /// <summary>
        /// 写入正常的内容
        /// </summary>
        /// <param name="msg"> 写入的内容</param>
        /// <param name="className"> 写日志发生在哪个类中</param>
        public static void WriteNormalLog(string msg, Type className=null)
        {
            SetConfigFile();
            if (className == null)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("null");
                log.Info(msg);
            }
            else
            {
                log4net.ILog log = log4net.LogManager.GetLogger(className);
                log.Info(msg);
            }
          
          
        }

        /// <summary>
        /// 写入错误的内容
        /// </summary>
        /// <param name="msg"> 写入的内容</param>
        /// <param name="className"> 写日志发生在哪个类中</param>
        public static void WriteExLog(string msg, Type className=null)
        {
            SetConfigFile();
            if (className == null)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("null");
                log.Error(msg);
            }
            else
            {
                log4net.ILog log = log4net.LogManager.GetLogger(className);
                log.Error(msg);
            }
          
        }
 
      
    }
}
