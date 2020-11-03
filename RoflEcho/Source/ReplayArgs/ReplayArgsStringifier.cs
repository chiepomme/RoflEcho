using System.Linq;

namespace RoflEcho
{
    public static class ReplayArgsStringifier
    {
        public static string Stringify(ReplayArgs args, bool withExecutable)
        {
            if (withExecutable)
            {
                return string.Join(' ', Enumerable
                                            .Empty<string>()
                                            .Append(args.Executable.FullName.Quote())
                                            .Append(args.ReplayFile.FullName.Quote())
                                            .Concat(args.ExtraArgs.Select(a => a.Quote())));
            }
            else
            {
                return string.Join(' ', Enumerable
                                            .Empty<string>()
                                            .Append(args.ReplayFile.FullName.Quote())
                                            .Concat(args.ExtraArgs.Select(a => a.Quote())));
            }
        }
    }
}
