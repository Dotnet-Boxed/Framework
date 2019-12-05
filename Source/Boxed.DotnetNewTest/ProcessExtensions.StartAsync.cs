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
        /// Starts the a <see cref="Process"/> asynchronously.
        /// </summary>
        /// <param name="workingDirectory">The working directory.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result and console output from executing the process.</returns>
        public static async Task<(ProcessResult processResult, string message)> StartAsync(
            string workingDirectory,
            string fileName,
            string arguments,
            CancellationToken cancellationToken)
        {
            TestLogger.WriteLine($"Executing {fileName} {arguments} from {workingDirectory}");

            var output = new StringBuilder();
            var error = new StringBuilder();
            using (var outputStringWriter = new StringWriter(output))
            using (var errorStringWriter = new StringWriter(error))
            {
                ProcessResult processResult;
                try
                {
                    var exitCode = await StartProcessAsync(
                        fileName,
                        arguments,
                        workingDirectory,
                        outputStringWriter,
                        errorStringWriter,
                        cancellationToken).ConfigureAwait(false);
                    processResult = exitCode == 0 ? ProcessResult.Succeeded : ProcessResult.Failed;
                }
                catch (TaskCanceledException)
                {
                    TestLogger.WriteLine($"Timed Out {fileName} {arguments} from {workingDirectory}");
                    processResult = ProcessResult.TimedOut;
                }

                var standardOutput = output.ToString();
                var standardError = error.ToString();

                var message = GetAndWriteMessage(
                    processResult,
                    standardOutput,
                    standardError);
                return (processResult, message);
            }
        }

        private static string GetAndWriteMessage(
            ProcessResult result,
            string standardOutput,
            string standardError)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Result: {result}");
            TestLogger.Write("Result: ");
            TestLogger.WriteLine(result.ToString(), result == ProcessResult.Succeeded ? ConsoleColor.Green : ConsoleColor.Red);

            if (!string.IsNullOrEmpty(standardError))
            {
                stringBuilder
                    .AppendLine()
                    .AppendLine($"StandardError: {standardError}");
                TestLogger.WriteLine("StandardError: ");
                TestLogger.WriteLine(standardError, ConsoleColor.Red);
                TestLogger.WriteLine();
            }

            if (!string.IsNullOrEmpty(standardOutput))
            {
                stringBuilder
                    .AppendLine()
                    .AppendLine($"StandardOutput: {standardOutput}");
                TestLogger.WriteLine();
                TestLogger.WriteLine($"StandardOutput: {standardOutput}");
            }

            return stringBuilder.ToString();
        }

        private static async Task<int> StartProcessAsync(
            string filename,
            string arguments,
            string workingDirectory = null,
            TextWriter outputTextWriter = null,
            TextWriter errorTextWriter = null,
            CancellationToken cancellationToken = default)
        {
            var processStartInfo = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                Arguments = arguments,
                FileName = filename,
                RedirectStandardOutput = outputTextWriter != null,
                RedirectStandardError = errorTextWriter != null,
                UseShellExecute = false,
                WorkingDirectory = workingDirectory,
            };

            try
            {
                using (var process = new Process() { StartInfo = processStartInfo })
                {
                    process.Start();

                    var tasks = new List<Task>(3) { process.WaitForExitAsync(cancellationToken) };
                    if (outputTextWriter != null)
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

                    if (errorTextWriter != null)
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
#if NETCOREAPP3_0 || NETCOREAPP3_1
                            process.Kill(true);
#else
                            process.KillTree();
#endif
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
            var taskCompletionSource = new TaskCompletionSource<object>();

            DataReceivedEventHandler handler = null;
            handler = new DataReceivedEventHandler(
                (sender, e) =>
                {
                    if (e.Data is null)
                    {
                        removeHandler(handler);
                        taskCompletionSource.TrySetResult(null);
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
