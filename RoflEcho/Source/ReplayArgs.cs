using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace RoflEcho
{
    public class ReplayArgs
    {
        public FileInfo ExecutablePath;
        public FileInfo ReplayPath;
        public string[] ExtraArgs;

        public ReplayArgs(string line)
        {
            var args = Parse(line);
            ExecutablePath = new FileInfo(args[0]);
            ReplayPath = new FileInfo(args[1]);
            ExtraArgs = args.Skip(2).ToArray();
        }

        List<string> Parse(string line)
        {
            var args = new List<string>();
            var sb = new StringBuilder();
            var quoted = false;

            foreach (var c in line)
            {
                switch (c)
                {
                    case ' ':
                    {
                        if (quoted)
                        {
                            sb.Append(c);
                            break;
                        }
                        else
                        {
                            args.Add(sb.ToString());
                            sb.Clear();
                            continue;
                        }
                    }
                    case '"':
                    {
                        quoted = !quoted;
                        break;
                    }
                    default:
                    {
                        sb.Append(c);
                        break;
                    }
                }
            }

            args.Add(sb.ToString());

            return args;
        }

        public void ExecuteReplay()
        {
            var args = Enumerable.Empty<string>()
                                 .Append(ReplayPath.FullName.Quote())
                                 .Concat(ExtraArgs.Select(a => a.Quote()));

            using var p = Process.Start(new ProcessStartInfo
            {
                FileName = ExecutablePath.FullName,
                Arguments = string.Join(' ', args),
                WorkingDirectory = ExecutablePath.DirectoryName,
            });
        }
    }
}
