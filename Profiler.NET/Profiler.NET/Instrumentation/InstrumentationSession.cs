using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Profiler.NET.Instrumentation
{
    public sealed class InstrumentationSession : ISession
    {
        private readonly Stopwatch SessionStopwatch;
        private readonly List<InstrumentationEntry> SessionInstrumentationEntries;
        private readonly List<InstrumentationLogEntry> SessionInstrumentationLogs;

        public string ProfilerName { get; }

        public string InstrumentorName { get; }

        public DateTime StartTime { get; }

        public bool IsRunning { get; private set; }

        public TimeSpan Elapsed => this.SessionStopwatch.Elapsed;

        public ImmutableArray<InstrumentationEntry> InstrumentationEntries => this.SessionInstrumentationEntries.ToImmutableArray();

        internal InstrumentationSession(string profilerName, string instrumentorName)
        {
            this.ProfilerName = profilerName;
            this.InstrumentorName = instrumentorName;

            this.SessionInstrumentationEntries = new List<InstrumentationEntry>();
            this.SessionInstrumentationLogs = new List<InstrumentationLogEntry>();

            this.StartTime = DateTime.Now;
            this.SessionStopwatch = Stopwatch.StartNew();
            this.IsRunning = true;
        }

        private string GetFunctionName(MethodInfo method)
        {
            return string.Format("{0} {1}::{2}::{3}({4})",
                method.ReturnType.Name,
                method.DeclaringType.Assembly.FullName,
                method.DeclaringType.FullName,
                method.Name,
                string.Join(", ", method.GetParameters().OrderBy(p => p.Position).Select(p => $"{p.ParameterType} {p.Name}{(p.HasDefaultValue ? $" = {p.DefaultValue}" : "")}"))
            );
        }

        internal InstrumentationEntry StartInstrumentMethod(string label, int processId, int threadId, MethodInfo method)
        {
            var entry = new InstrumentationEntry(label, this.GetFunctionName(method), processId, threadId, DateTime.Now);

            var logStart = new InstrumentationLogEntry(entry, InstrumentationLogEntry.LogEntryTypes.MethodBeginInvoke);
            entry.AddLog(logStart);

            this.SessionInstrumentationEntries.Add(entry);
            this.SessionInstrumentationLogs.Add(logStart);

            return entry;
        }

        internal void StopInstrumentMethod(InstrumentationEntry entry, TimeSpan elapsed)
        {
            entry.SetElapsed(elapsed);

            var logEnd = new InstrumentationLogEntry(entry, InstrumentationLogEntry.LogEntryTypes.MethodEndInvoke);
            entry.AddLog(logEnd);

            this.SessionInstrumentationLogs.Add(logEnd);
        }

        public ISession Stop()
        {
            if(!this.IsRunning)
                return this;

            this.SessionStopwatch.Stop();
            this.IsRunning = false;
            return this;
        }
    }
}
