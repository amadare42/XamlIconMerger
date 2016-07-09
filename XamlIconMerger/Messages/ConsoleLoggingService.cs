using System;

namespace XamlIconMerger.Messages
{
    public class ConsoleLoggingService : ILoggingService
    {
        public void Error(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.ReadLine();
        }

        public void LogException(Exception ex)
        {
            this.Error(ex.ToString());
        }

        public IMessagesBlock NewBlock(string blockText)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{blockText}:");
            return new ConsoleMessageBlock();
        }

        public void LogInfo(string text)
        {
            Console.WriteLine(text);
        }

        private class ConsoleMessageBlock : IMessagesBlock
        {
            public ConsoleMessageBlock()
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }

            public void Dispose()
            {
                Console.Write("\r\n\r\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            public void Log(string text)
            {
                Console.Write(" " + text);
            }

            public void LogNewLine(string text)
            {
                Console.WriteLine("\r\n\t" + text);
            }
        }
    }
}