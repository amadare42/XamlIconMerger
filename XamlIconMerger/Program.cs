using System;
using System.Diagnostics;
using CommandLine;
using XamlIconMerger.Messages;

namespace XamlIconMerger
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<RunArguments>(args)
                .WithParsed(RunApp);
        }

        public static void RunApp(RunArguments args)
        {
            ILoggingService loggingService = new ConsoleLoggingService();
            var factory = new GlobalFactory(loggingService);
            var fetcher = factory.CreateDefaultFileFetcher();
            var merger = factory.CreateDefaultTextMerger(args);

            try
            {
                var files = fetcher.GetFilesList(args);
                merger.Merge(files);
                Console.WriteLine("Press <Enter> to open results. Any key to exit.");
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Enter)
                {
                    Process.Start(args.OutFile);
                }
            }
            catch (Exception ex)
            {
                loggingService.Error($"Exception during merge:\r\n{ex}");
            }
        }
    }
}