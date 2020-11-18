namespace Boxed.DotnetNewTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// <see cref="Process"/> extension methods.
    /// </summary>
    public static partial class ProcessExtensions
    {
        /// <summary>
        /// Starts the specified <see cref="Process"/> and then waits for exit asynchronously for the process to exit.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns>A task representing the operation.</returns>
        public static Task StartAndWaitForExitAsync(this Process process)
        {
            if (process is null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            var taskCompletionSource = new TaskCompletionSource<object>();

            process.EnableRaisingEvents = true;

            process.Exited += (s, a) =>
            {
                taskCompletionSource.SetResult(null);

                process.Dispose();
            };

            _ = process.Start();

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Waits asynchronously for the process to exit.
        /// </summary>
        /// <param name="process">The process to wait for cancellation.</param>
        /// <param name="cancellationToken">A cancellation token. If invoked, the task will return
        /// immediately as cancelled.</param>
        /// <returns>A Task representing waiting for the process to end.</returns>
        public static Task WaitForExitAsync(
            this Process process,
            CancellationToken cancellationToken = default)
        {
            if (process is null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            var taskCompletionSource = new TaskCompletionSource<object>();

            process.EnableRaisingEvents = true;

            process.Exited += OnExited;
            if (cancellationToken != default)
            {
                cancellationToken.Register(
                    () =>
                    {
                        process.Exited -= OnExited;
                        taskCompletionSource.TrySetCanceled();
                    });
            }

            return taskCompletionSource.Task;

            void OnExited(object sender, EventArgs e)
            {
                process.Exited -= OnExited;
                taskCompletionSource.TrySetResult(null);
            }
        }

        private static void GetAllChildIdsUnix(int parentId, ISet<int> children, TimeSpan timeout)
        {
            var exitCode = RunProcessAndWaitForExit(
                "pgrep",
                $"-P {parentId}",
                timeout,
                out var stdout);

            if (exitCode == 0 && !string.IsNullOrEmpty(stdout))
            {
                using (var reader = new StringReader(stdout))
                {
                    while (true)
                    {
                        var text = reader.ReadLine();
                        if (text is null)
                        {
                            return;
                        }

                        if (int.TryParse(text, out var id))
                        {
                            children.Add(id);

                            // Recursively get the children
                            GetAllChildIdsUnix(id, children, timeout);
                        }
                    }
                }
            }
        }

        private static int RunProcessAndWaitForExit(string fileName, string arguments, TimeSpan timeout, out string stdout)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            };

            var process = Process.Start(startInfo);

            stdout = null;
            if (process.WaitForExit((int)timeout.TotalMilliseconds))
            {
                stdout = process.StandardOutput.ReadToEnd();
            }
            else
            {
                process.Kill();
            }

            return process.ExitCode;
        }
    }
}
