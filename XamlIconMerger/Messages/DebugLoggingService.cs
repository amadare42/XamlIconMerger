using System;
using System.Diagnostics;

namespace XamlIconMerger.Messages
{
    public class DebugLoggingService : ILoggingService
    {
        public void LogInfo(string text)
        {
            Debug.WriteLine("---------INFO:");
            Debug.Indent();
            Debug.WriteLine(text);
            Debug.Unindent();
        }

        public void Error(string text)
        {
            Debug.WriteLine("---------ERROR:");
            Debug.Indent();
            Debug.WriteLine(text);
            Debug.Unindent();
        }

        public void LogException(Exception ex)
        {
            Debug.WriteLine("---------EXCEPTION:");
            Debug.Indent();
            Debug.WriteLine(ex.ToString());
            Debug.Unindent();
        }

        public IMessagesBlock NewBlock(string blockText)
        {
            Debug.WriteLine($"------ {blockText}:");
            Debug.Indent();
            return new DebugMessageBlock();
        }

        private class DebugMessageBlock : IMessagesBlock
        {
            public void Dispose()
            {
                Debug.Unindent();
            }

            public void Log(string text)
            {
                Debug.Write(text + "\t");
            }

            public void LogNewLine(string text)
            {
                Debug.Write("\r\n" + text + "\t");
            }
        }
    }
}