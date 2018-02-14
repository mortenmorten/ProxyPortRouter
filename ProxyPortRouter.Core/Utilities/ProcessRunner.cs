namespace ProxyPortRouter.Core.Utilities
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    using Serilog;

    [UsedImplicitly]
    public class ProcessRunner : IProcessRunnerAsync
    {
        public Task<string> RunAsync(string command, string arguments)
        {
            Log.Information("Running command: {Command} {Arguments}", command, arguments);
            var tcs = new TaskCompletionSource<string>();

            var process = new Process
                              {
                                  StartInfo =
                                      {
                                          FileName = command,
                                          Arguments = arguments,
                                          UseShellExecute = false,
                                          RedirectStandardOutput = true,
                                          RedirectStandardError = true,
                                          CreateNoWindow = true,
                                      },
                                  EnableRaisingEvents = true
                              };

            process.Exited += (sender, args) =>
                {
                    var result = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();
                    tcs.SetResult(process.ExitCode == 0 || string.IsNullOrEmpty(error) ? result : error);
                };

            process.Start();

            return tcs.Task;
        }
    }
}