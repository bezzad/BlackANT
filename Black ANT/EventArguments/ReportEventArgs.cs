using System;

namespace EventArguments
{
    public enum ReportCodes
    {
        AddExtension,
        Allowed,
        Canceled,
        CancelRequested,
        DataChanged,
        DataDeleted,
        DataLoaded,
        DataStored,
        DiskProbingFinished,
        DiskProbingStarted,
        Disposed,
        Enter,
        ExceptionHandled,
        Exit,
        ExtractCompleted,
        ExtractStart,
        GUIClosed,
        GUIRunning,
        IdentifyMassStorage,
        IdentityVerificationMassStorage,
        KeyListenerStarted,
        KeyListenerStopped,
        MassStorageInserted,
        MassStorageRejected,
        NotAllowed,
        Other,
        PasswordCorrect,
        PasswordInCorrect,
        RemoveExtension,
        RequestedForJob,
        RunAsAdministrator,
        RunAsCurrentUser,
        RunFromCorrectPath,
        RunFromInCorrectPath,
        Running,
        Searching,
        TheSearchBeginning,
        TheSearchCompleted,
        WaitCompleted,
        WaitStartup,
        Waitting,
        ZipOperateCompleted,
        ZipOperateStart,
        Zipped,
        Zipping,
        PackagingStarted,
        PackagingDone
    }

    /// <summary>
    /// Reporter Event Arguments
    /// </summary>
    public class ReportEventArgs : EventArgs
    {
        private Exception exception;
        private string message;
        private DateTime occurrenceUtcTime;
        private ReportCodes reportCode;
        private string source;
        public ReportEventArgs(string sourceName, ReportCodes code, string messageData)
        {
            occurrenceUtcTime = DateTime.UtcNow;
            message = messageData;
            source = sourceName;
            reportCode = code;
        }

        public ReportEventArgs(string sourceName, ReportCodes code, string format, params object[] args)
        {
            occurrenceUtcTime = DateTime.UtcNow;
            message = string.Format(format, args);
            source = sourceName;
            reportCode = code;
        }

        public ReportEventArgs(string sourceName, Exception exp)
        {
            this.occurrenceUtcTime = DateTime.UtcNow;
            this.message = exp.Message;
            this.source = sourceName;
            this.exception = exp;
            this.reportCode = ReportCodes.ExceptionHandled;
        }

        /// <summary>
        /// Properties of message variable
        /// </summary>
        public string Message
        {
            get { return message; }
        }

        /// <summary>
        /// Get Occurrence Time in Iran Standard Time format's
        /// </summary>
        public DateTime OccurrenceTime_IST
        {
            get
            {
                try
                {
                    TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Iran Standard Time");
                    DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(OccurrenceUtcTime, cstZone);
                    return cstTime;
                }
                catch (TimeZoneNotFoundException)
                {
                    throw new TimeZoneNotFoundException("The registry does not define the Central Standard Time zone.");
                }
                catch (InvalidTimeZoneException)
                {
                    throw new InvalidTimeZoneException("Registry data on the Central STandard Time zone has been corrupted.");
                }
            }
        }

        /// <summary>
        /// Properties of occurrenceTime variable
        /// </summary>
        public DateTime OccurrenceUtcTime
        {
            get { return occurrenceUtcTime; }
        }

        /// <summary>
        /// Properties of reportCode variable
        /// </summary>
        public ReportCodes ReportCode
        { get { return reportCode; } }

        /// <summary>
        /// Properties of exception variable
        /// </summary>
        public Exception ReportException
        {
            get { return exception; }
        }

        /// <summary>
        /// Properties of source variable
        /// </summary>
        public string Source
        {
            get { return source; }
        }
    }
}
