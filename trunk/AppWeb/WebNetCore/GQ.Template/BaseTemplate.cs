using System.Collections.Generic;

namespace GQ.Template
{
    public class BaseTemplate
    {
        public string TextTemplate { get; set; }

        public List<object> Data { get; set; }

        public virtual string Rendere()
        {
            return "";
        }
    }
}
