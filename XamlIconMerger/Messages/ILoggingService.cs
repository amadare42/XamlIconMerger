using System;

namespace XamlIconMerger.Messages
{
    public interface ILoggingService
    {
        void LogInfo(string text);
        void Error(string text);
        void LogException(Exception ex);

        IMessagesBlock NewBlock(string blockText);
    }
}