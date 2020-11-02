using System;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace RoflEcho
{
    public static class ReplayArgsFetcher
    {
        public static (string, Exception) Fetch()
        {
            var clientProcess = Process
                                .GetProcesses()
                                .Where(p => p.ProcessName == "League of Legends" && p.MainWindowTitle == "League of Legends (TM) Client")
                                .SingleOrDefault();

            if (clientProcess == null) return (null, new ClientNotFoundException());

            var args = clientProcess.GetCommandLine();
            if (args == null) return (null, new ClientCantGetArgumentsException());

            return (args, null);
        }
    }
}
