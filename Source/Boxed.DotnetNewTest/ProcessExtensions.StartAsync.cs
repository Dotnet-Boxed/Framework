namespace Boxed.DotnetNewTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// <see cref="Process"/> extension methods.
    /// </summary>
    public static partial class ProcessExtensions
    {
        /// <summary>
        /// Starts the a <see cref="Process" /> asynchronously.
        /// </summary>
        /// <param name="workingDirectory">The working directory.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="showShellWindow">if set to <c>true</c> show a shell window instead of logging.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The result and console output from executing the process.
        /// </returns>
        public static async Task<(ProcessResult ProcessResult, string Message)> StartAsync(
            string workingDirectory,
            string fileName,
            string arguments,
            bool showShellWindow = false,
            CancellationToken cancellationToken = default)
        {
            TestLogger.WriteLine($"Executing {fileName} {arguments} from {workingDirectory}");

            var stringBuilder = new StringBuilder();
            using (var stringWriter = new StringWriter(stringBuilder))
            {
                ProcessResult processResult;
                try
                {
                    var exitCode = await StartProcessAsync(
                        fileName,
                        arguments,
                        workingDirectory,
                        stringWriter,
                        stringWriter,
                        showShellWindow,
                        cancellationToken).ConfigureAwait(false);
                    processResult = exitCode == 0 ? ProcessResult.Succeeded : ProcessResult.Failed;
                }
                catch (TaskCanceledException)
                {
                    await stringWriter.FlushAsync().ConfigureAwait(false);
                    TestLogger.WriteLine($"Timed Out {fileName} {arguments} from {workingDirectory}");
                    processResult = ProcessResult.TimedOut;
                }

                var message = GetAndWriteMessage(processResult, stringBuilder.ToString());
                return (processResult, message);
            }
        }

        private static string GetAndWriteMessage(ProcessResult result, string output)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Result: {result}");
            TestLogger.Write("Result: ");
            TestLogger.WriteLine(result.ToString(), result == ProcessResult.Succeeded ? ConsoleColor.Green : ConsoleColor.Red);

            stringBuilder
                .AppendLine()
                .AppendLine($"Output: {output}");
            TestLogger.WriteLine();
            TestLogger.WriteLine($"Output: {output}");

            return stringBuilder.ToString();
        }

        private static async Task<int> StartProcessAsync(
            string filename,
            string arguments,
            string workingDirectory,
            TextWriter outputTextWriter,
            TextWriter errorTextWriter,
            bool showShellWindow,
            CancellationToken cancellationToken)
        {
            var processStartInfo = new ProcessStartInfo()
            {
                CreateNoWindow = showShellWindow,
                Arguments = arguments,
                FileName = filename,
                RedirectStandardOutput = !showShellWindow && outputTextWriter is not null,
                RedirectStandardError = !showShellWindow && errorTextWriter is not null,
                UseShellExecute = showShellWindow,
                WorkingDirectory = workingDirectory,
            };

            try
            {
                using (var process = new Process() { StartInfo = processStartInfo })
                {
                    process.Start();

                    var tasks = new List<Task>(3) { process.WaitForExitAsync(cancellationToken) };
                    if (!showShellWindow && outputTextWriter is not null)
                    {
                        tasks.Add(ReadAsync(
                            x =>
                            {
                                process.OutputDataReceived += x;
                                process.BeginOutputReadLine();
                            },
                            x => process.OutputDataReceived -= x,
                            outputTextWriter,
                            cancellationToken));
                    }

                    if (!showShellWindow && errorTextWriter is not null)
                    {
                        tasks.Add(ReadAsync(
                            x =>
                            {
                                process.ErrorDataReceived += x;
                                process.BeginErrorReadLine();
                            },
                            x => process.ErrorDataReceived -= x,
                            errorTextWriter,
                            cancellationToken));
                    }

                    try
                    {
                        await Task.WhenAll(tasks).ConfigureAwait(false);
                    }
                    catch
                    {
                        if (!process.HasExited)
                        {
                            process.Kill(true);
                        }

                        throw;
                    }

                    return process.ExitCode;
                }
            }
            catch (Exception exception)
            {
                throw new ProcessStartException(processStartInfo, exception);
            }
        }

        /// <summary>
        /// Reads the data from the specified data received event and writes it to the
        /// <paramref name="textWriter"/>.
        /// </summary>
        /// <param name="addHandler">Adds the event handler.</param>
        /// <param name="removeHandler">Removes the event handler.</param>
        /// <param name="textWriter">The text writer.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private static Task ReadAsync(
            this Action<DataReceivedEventHandler> addHandler,
            Action<DataReceivedEventHandler> removeHandler,
            TextWriter textWriter,
            CancellationToken cancellationToken = default)
        {
            var taskCompletionSource = new TaskCompletionSource();

            DataReceivedEventHandler handler = null!;
            handler = new DataReceivedEventHandler(
                (sender, e) =>
                {
                    if (e.Data is null)
                    {
                        removeHandler(handler);
                        taskCompletionSource.SetResult();
                    }
                    else
                    {
                        textWriter.WriteLine(e.Data);
                    }
                });

            addHandler(handler);

            if (cancellationToken != default)
            {
                cancellationToken.Register(
                    () =>
                    {
                        removeHandler(handler);
                        taskCompletionSource.TrySetCanceled();
                    });
            }

            return taskCompletionSource.Task;
        }
    }
}
