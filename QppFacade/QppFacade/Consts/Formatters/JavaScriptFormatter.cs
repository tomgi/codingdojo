using System;

namespace IHS.Phoenix.QPP.Formatters
{
    public class JavaScriptFormatter : IFormatters
    {
        public Func<string, string> TypeFormatter
        {
            get
            {
                return typeName => string.Format("{0}:{{", typeName);
            }
        }
        public Func<string, string, string> StringFormatter
        {
            get
            {
                return (key, value) => string.Format("{0} : \"{1}\"", key, value);
            }
        }
        public Func<string, string, string> StringArrayFormatter
        {
            get
            {
                return (key, value) => string.Format("{0} : [{1}]", key, value);
            }
        }
        public string Separator
        {
            get { return ","; }
        }
    }
}