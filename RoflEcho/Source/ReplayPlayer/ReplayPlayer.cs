using System.Diagnostics;

namespace RoflEcho
{
    public static class ReplayPlayer
    {
        public static void Play(ReplayArgs replayArgs)
        {
            using var _ = Process.Start(new ProcessStartInfo
            {
                FileName = replayArgs.Executable.FullName,
                Arguments = ReplayArgsStringifier.Stringify(replayArgs, withExecutable: false),
                WorkingDirectory = replayArgs.Executable.DirectoryName,
            });
        }
    }
}
