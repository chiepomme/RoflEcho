using System.Collections.Generic;
using System.Text;

namespace RoflEcho
{
    public static class CommandLineParser
    {
        /// <summary>
        /// Split a command line text by spaces then remove quotes
        /// </summary>
        public static List<string> Parse(string commandLine)
        {
            var args = new List<string>();
            var sb = new StringBuilder();
            var innerQuote = false;

            foreach (var c in commandLine)
            {
                switch (c)
                {
                    case ' ':
                    {
                        if (innerQuote)
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
                        innerQuote = !innerQuote;
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
    }
}
