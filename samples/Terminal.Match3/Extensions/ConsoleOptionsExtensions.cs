using System;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace Terminal.Match3.Extensions
{
    public static class ConsoleOptionsExtensions
    {
        public static void SetCursorVisible(this ConsoleLifetimeOptions _, bool isVisible)
        {
            Console.CursorVisible = isVisible;
        }

        public static void SetOutputEncoding(this ConsoleLifetimeOptions _, Encoding encoding)
        {
            Console.OutputEncoding = encoding;
        }

        public static void SuppressStatusMessages(this ConsoleLifetimeOptions options)
        {
            options.SuppressStatusMessages = true;
        }
    }
}