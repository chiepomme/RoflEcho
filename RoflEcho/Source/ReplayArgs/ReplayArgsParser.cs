using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RoflEcho
{
    public static class ReplayArgsParser
    {
        public static ReplayArgs Parse(string commandLine)
        {
            return Parse(CommandLineParser.Parse(commandLine));
        }

        public static ReplayArgs Parse(IList<string> args)
        {
            if (args.Count < 2) return null;
            if (!args[0].ToLower().EndsWith(".exe")) return null;
            if (!args[1].ToLower().EndsWith(".rofl")) return null;

            return new ReplayArgs
            {
                Executable = new FileInfo(args[0]),
                ReplayFile = new FileInfo(args[1]),
                ExtraArgs = args.Skip(2).ToArray(),
            };
        }
    }
}
