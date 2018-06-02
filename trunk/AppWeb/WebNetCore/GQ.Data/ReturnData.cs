using System;

namespace GQ.Data
{

    /// <summary>
    /// 
    /// </summary>
    public class ReturnData
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual object data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool isError { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual long ticks { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool isSecurity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ReturnData()
        {
            data = null;
            isError = false;
            isSecurity = false;
            ticks = DateTime.Now.Ticks;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class ReturnData<T> : ReturnData
    {
        /// <summary>
        /// 
        /// </summary>
        public new T data
        {
            get
            {
                T result;

                try
                {
                    result = (T)base.data;
                }
                catch
                {
                    result = default(T);
                }

                return result;
            }
            set
            {
                base.data = value;
            }
        }

    }
}