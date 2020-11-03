using System.Diagnostics;

namespace RoflEcho
{
    public static class ReplayArgsFetcher
    {
        public static ReplayArgs Fetch(Process clientProcess)
        {
            var replayArgsLine = clientProcess.GetCommandLine();
            if (replayArgsLine == null) return null;

            var args = CommandLineParser.Parse(replayArgsLine);
            return ReplayArgsParser.Parse(args);
        }
    }
}
