using System;

namespace IHS.Phoenix.QPP.Formatters
    {
    public class CsharpFormatter : IFormatters
    {
        public Func<string,string> TypeFormatter {
            get
            {
                return typeName => string.Format("public static class {0}{{", typeName);
            }
        }
        public Func<string,string,string> StringFormatter {
            get
            {
                return (key, value) => string.Format("public const string {0} = \"{1}\";", key, value);
            }
        }
        public Func<string,string,string> StringArrayFormatter {
            get
            {
                return (key, value) => string.Format("public static string[] {0} = {{{1}}};", key, value);
            }
        }
        public string Separator {
            get { return string.Empty; }
        }
    }
}