using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiler.NET.Instrumentation
{
    public class InstrumentationEntry
    {
        internal InstrumentationEntry(string label, string functionName, int processId, int threadId, DateTime startTime)
            : this(label, functionName, processId, threadId, startTime, TimeSpan.Zero)
        {
        }

        internal InstrumentationEntry(string label, string functionName, int processId, int threadId, DateTime startTime, TimeSpan elapsed) 
        {
            this.Label = label;
            this.FunctionName = functionName;
            this.ProcessId = processId;
            this.ThreadId = threadId;
            this.StartTime = startTime;
            this.EntryElapsed = elapsed;

            this.EntryLogs = new List<InstrumentationLogEntry>();
        }

        private TimeSpan EntryElapsed;
        private readonly List<InstrumentationLogEntry> EntryLogs;

        internal void AddLog(InstrumentationLogEntry log)
        {
            this.EntryLogs.Add(log);
        }

        internal void SetElapsed(TimeSpan elapsed)
        {
            this.EntryElapsed = elapsed;
        }

        public string Label { get; }

        public string FunctionName { get; }

        public int ProcessId { get; }
        
        public int ThreadId { get; }

        public DateTime StartTime { get; }

        public TimeSpan Elapsed => this.EntryElapsed;

        public ImmutableArray<InstrumentationLogEntry> Logs => this.EntryLogs.ToImmutableArray();
    }
}
