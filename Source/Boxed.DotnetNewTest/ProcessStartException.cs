namespace Boxed.DotnetNewTest
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    /// <summary>
    /// A process failed to start exception.
    /// </summary>
    /// <seealso cref="Exception" />
    [Serializable]
    public class ProcessStartException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessStartException"/> class.
        /// </summary>
        public ProcessStartException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessStartException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ProcessStartException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessStartException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        public ProcessStartException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessStartException"/> class.
        /// </summary>
        /// <param name="processStartInfo">The process start information.</param>
        public ProcessStartException(ProcessStartInfo processStartInfo)
            : base(GetMessage(processStartInfo))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessStartException"/> class.
        /// </summary>
        /// <param name="processStartInfo">The process start information.</param>
        /// <param name="inner">The inner.</param>
        public ProcessStartException(ProcessStartInfo processStartInfo, Exception inner)
            : base(GetMessage(processStartInfo), inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessStartException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"></see> that holds the serialized object data about
        /// the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"></see> that contains contextual information about
        /// the source or destination.</param>
        protected ProcessStartException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }

        private static string GetMessage(ProcessStartInfo processStartInfo)
        {
            if (processStartInfo == null)
            {
                throw new ArgumentNullException(nameof(processStartInfo));
            }

            return $"Process start failed. Filename:<{processStartInfo.FileName}> Arguments:<{processStartInfo.Arguments}> WorkingDirectory:<{processStartInfo.WorkingDirectory}>.";
        }
    }
}
