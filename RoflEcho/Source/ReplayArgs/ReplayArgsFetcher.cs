using System.Diagnostics;

namespace RoflEcho
{
    public enum ReplayArgsFetcherError
    {
        None,
        CouldNotGetCommandLineArguments,
    }

    public static class ReplayArgsFetcher
    {
        public static (ReplayArgs replayArgs, ReplayArgsFetcherError fetcherError, ReplayArgsParserError parserError) Fetch(Process clientProcess)
        {
            var replayArgsLine = clientProcess.GetCommandLine();
            if (replayArgsLine == null) return (null, ReplayArgsFetcherError.CouldNotGetCommandLineArguments, ReplayArgsParserError.None);

            var args = CommandLineParser.Parse(replayArgsLine);
            var (replayArgs, parserError) = ReplayArgsParser.Parse(args);
            return (replayArgs, ReplayArgsFetcherError.None, parserError);
        }
    }
}
