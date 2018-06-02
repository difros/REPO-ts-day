using log4net;
using System;

namespace GQService.com.gq.log
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
        private static readonly ILog log = LogManager.GetLogger("GeminusQhom", "GeminusQhom");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(object message)
        {
            log.Debug(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Debug(object message, Exception exception)
        {
            log.Debug(message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        public static void Debug(object Class, string message)
        {
            Debug(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Debug(object Class, string message, Exception exception)
        {
            Debug(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Error(object message)
        {
            log.Error(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(object message, Exception exception)
        {
            log.Error(message + "\n" + (exception.StackTrace != null ? exception.StackTrace : ""), exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        public static void Error(object Class, string message)
        {
            Error(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(object Class, string message, Exception exception)
        {
            Error(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Fatal(object message)
        {
            log.Fatal(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Fatal(object message, Exception exception)
        {
            log.Fatal(message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        public static void Fatal(object Class, string message)
        {
            Fatal(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Fatal(object Class, string message, Exception exception)
        {
            Fatal(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Info(object message)
        {
            log.Info(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(object message, Exception exception)
        {
            log.Info(message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        public static void Info(object Class, string message)
        {
            Info(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(object Class, string message, Exception exception)
        {
            Info(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(object message)
        {
            log.Warn(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Warn(object message, Exception exception)
        {
            log.Warn(message, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        public static void Warn(object Class, string message)
        {
            Warn(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Warn(object Class, string message, Exception exception)
        {
            Warn(Class.GetType().Namespace + "." + Class.GetType().Name + " : " + message, exception);
        }

    }
}
