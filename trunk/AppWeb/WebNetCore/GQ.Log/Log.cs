using System;
using log4net;
using System.Collections.Generic;
using GQ.Core.service;
using log4net.Repository;

namespace GQ.Log
{
    /// <summary>
    /// Registra log de la aplicacion
    /// Agregasr en el web.config
    /// <code>
    ///     <logger name="GeminusQhom">
    ///         <level value="DEBUG"/>
    ///     </logger>
    /// </code>
    /// </summary>
    public class Log
    {

        public const string DefaultLog = "GeminusQhom";
        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<string, Log> Logs = new Dictionary<string, Log>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LogName"></param>
        /// <returns></returns>
        public static Log GetLog(string LogName = DefaultLog)
        {
            try
            {
                if (!Logs.ContainsKey(LogName))
                {
                    new Log(LogName);
                }
                return Logs[LogName];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void StartLog()
        {
            var repository = log4net.LogManager.CreateRepository(DefaultLog);
            StartLog(repository);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public static void StartLog(ILoggerRepository repository)
        {
            log4net.Config.XmlConfigurator.Configure(repository, new System.IO.FileInfo(ServicesContainer.ContentRootPath() + "/log4netConfig.xml"));

            Log.GetLog().Info("****************************************************************************************");
            Log.GetLog().Info("****************************************************************************************");
            Log.GetLog().Info("************************************   Startup  ****************************************");
            Log.GetLog().Info("****************************************************************************************");
            Log.GetLog().Info("****************************************************************************************");
        }


        /// <summary>
        /// 
        /// </summary>
        private readonly ILog log = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Repositorio"></param>
        /// <param name="LogName"></param>
        public Log(string LogName = DefaultLog)
        {
            log = LogManager.GetLogger(LogName, LogName);

            if (Logs.ContainsKey(LogName))
            {
                Logs[LogName] = this;
            }
            else
            {
                Logs.Add(LogName, this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Debug(object message)
        {
            log.Debug(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Debug(object message, Exception exception)
        {
            log.Debug(message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        public void Debug(object Class, string message)
        {
            Debug(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Debug(object Class, string message, Exception exception)
        {
            Debug(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Error(object message)
        {
            log.Error(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Error(object message, Exception exception)
        {
            log.Error(message + "\n" + (exception.StackTrace != null ? exception.StackTrace : ""), exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        public void Error(object Class, string message)
        {
            Error(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Error(object Class, string message, Exception exception)
        {
            Error(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Fatal(object message)
        {
            log.Fatal(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Fatal(object message, Exception exception)
        {
            log.Fatal(message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        public void Fatal(object Class, string message)
        {
            Fatal(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Fatal(object Class, string message, Exception exception)
        {
            Fatal(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Info(object message)
        {
            log.Info(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Info(object message, Exception exception)
        {
            log.Info(message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        public void Info(object Class, string message)
        {
            Info(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Info(object Class, string message, Exception exception)
        {
            Info(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Warn(object message)
        {
            log.Warn(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Warn(object message, Exception exception)
        {
            log.Warn(message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        public void Warn(object Class, string message)
        {
            Warn(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Warn(object Class, string message, Exception exception)
        {
            Warn(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message, exception);
        }

    }
}
