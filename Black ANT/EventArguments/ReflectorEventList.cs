using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace EventArguments
{
    /// <summary>
    /// If occurred Reporter event then reflect by ReflectReports.
    /// All occurrence event saved in this class List.
    /// You can Add or AddRange many ReportEventArgs programmatic to this class objects.
    /// </summary>
    public static class EventReflector
    {
        public static ActionBlock<ReportEventArgs> CallReport =
            new ActionBlock<ReportEventArgs>(e => { Reports.Add(e);  ReflectedReporter("Reflector", e); });

        public static List<ReportEventArgs> Reports = new List<ReportEventArgs>();

        public static event EventHandler<ReportEventArgs> ReflectedReporter = delegate { };

    }
}
