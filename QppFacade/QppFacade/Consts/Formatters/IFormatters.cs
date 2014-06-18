using System;

namespace IHS.Phoenix.QPP.Formatters
{
    public interface IFormatters
    {
        Func<string, string> TypeFormatter { get; }
        Func<string, string, string> StringFormatter { get; }
        Func<string, string, string> StringArrayFormatter { get; }
        string Separator { get; }
    }
}