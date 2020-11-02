using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace RoflEcho
{
    public static class ProcessExtension
    {
        // thx https://stackoverflow.com/questions/2633628/can-i-get-command-line-arguments-of-other-processes-from-net-c
        public static string GetCommandLine(this Process process)
        {
            using var searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id);
            using var objects = searcher.Get();
            return objects.Cast<ManagementBaseObject>().SingleOrDefault()?["CommandLine"]?.ToString();
        }
    }
}
