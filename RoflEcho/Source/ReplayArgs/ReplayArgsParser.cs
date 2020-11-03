using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RoflEcho
{
    public enum ReplayArgsParserError
    {
        None,
        LackOfArguments,
        FirstArgumentIsNotExe,
        SecondArgumentIsNotRofl,
    }

    public static class ReplayArgsParser
    {
        public static (ReplayArgs replayArgs, ReplayArgsParserError error) Parse(string commandLine)
        {
            return Parse(CommandLineParser.Parse(commandLine));
        }

        public static (ReplayArgs replayArgs, ReplayArgsParserError error) Parse(IList<string> args)
        {
            if (args.Count < 2) return (null, ReplayArgsParserError.LackOfArguments);
            if (!args[0].ToLower().EndsWith(".exe")) return (null, ReplayArgsParserError.FirstArgumentIsNotExe);
            if (!args[1].ToLower().EndsWith(".rofl")) return (null, ReplayArgsParserError.SecondArgumentIsNotRofl);

            return (new ReplayArgs
            {
                Executable = new FileInfo(args[0]),
                ReplayFile = new FileInfo(args[1]),
                ExtraArgs = args.Skip(2).ToArray(),
            }, ReplayArgsParserError.None);
        }
    }
}
