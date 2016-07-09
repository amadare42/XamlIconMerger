using System;

namespace XamlIconMerger.Messages
{
    public interface IMessagesBlock : IDisposable
    {
        void Log(string text);
        void LogNewLine(string text);
    }
}