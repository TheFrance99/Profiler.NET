using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiler.NET.Instrumentation
{
    public class InstrumentationLogEntry
    {
        public enum LogEntryTypes
        {
            MethodBeginInvoke,
            MethodEndInvoke
        }

        internal InstrumentationLogEntry(InstrumentationEntry entry, LogEntryTypes logEntryType)
        {
            this.Entry = entry;
            this.LogEntryType = logEntryType;
            this.LogTime = DateTime.Now;
        }

        public InstrumentationEntry Entry { get; }

        public LogEntryTypes LogEntryType { get; }

        public DateTime LogTime { get; }
    }
}
