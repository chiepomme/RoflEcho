using System.Data;
using System.Diagnostics;
using System.Linq;

namespace RoflEcho
{
    public static class ClientProcessFinder
    {
        public static Process Find()
            => Process
                .GetProcesses()
                .Where(p => p.ProcessName == "League of Legends")
                .Where(p => p.MainWindowTitle == "League of Legends (TM) Client")
                .SingleOrDefault();
    }
}
