﻿using System;

namespace GQ.Data.exception
{
    /// <summary>
    /// 
    /// </summary>
    public class GenericError
    {
        /// <summary>
        /// 
        /// </summary>
        public GenericError()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public GenericError(Exception e)
        {
            ExceptionType = e.GetType().Name;
            Message = e.Message;
            Source = e.Source;
            StackTrace = e.StackTrace;
            Data = e.Data;

            if (e != null)
            {
                if (e.InnerException != null)
                    InnerException = e.InnerException.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual System.String ExceptionType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual System.String Message { get; set; }

        public static GenericError Create(Exception e)
        {
            return new GenericError(e);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual System.String Source { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual System.String StackTrace { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Collections.IDictionary Data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual System.String InnerException { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual Boolean isLogin { get; set; }

    }
}
