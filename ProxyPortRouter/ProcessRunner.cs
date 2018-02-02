﻿using System.Diagnostics;

namespace ProxyPortRouter
{
    public static class ProcessRunner
    {
        public static string Run(string command, string arguments)
        {
            var process = new Process
            {
                StartInfo =
                {
                    //FileName = "cmd.exe",
                    //Arguments = string.Join(" ", "/c", command, arguments),
                    FileName = command,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            process.Start();
            process.WaitForExit();
            var result = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            return process.ExitCode == 0 || string.IsNullOrEmpty(error) ? result : error;
        }
    }
}