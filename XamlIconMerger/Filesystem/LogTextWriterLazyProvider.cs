using System.IO;
using XamlIconMerger.Messages;

namespace XamlIconMerger.Filesystem
{
    public class LogTextWriterLazyProvider : ILazyTextWriterProvider
    {
        private readonly ILoggingService loggingService;

        public LogTextWriterLazyProvider(ILoggingService loggingService)
        {
            this.loggingService = loggingService;
        }

        public TextWriter GetTextWriter()
        {
            return new ObservableStringWriter(loggingService);
        }

        private class ObservableStringWriter : StringWriter
        {
            private readonly ILoggingService loggingService;

            public ObservableStringWriter(ILoggingService loggingService)
            {
                this.loggingService = loggingService;
            }

            protected override void Dispose(bool disposing)
            {
                loggingService.LogInfo(GetStringBuilder().ToString());
                base.Dispose(disposing);
            }
        }
    }
}