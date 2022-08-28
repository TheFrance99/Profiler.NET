using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiler.NET.Instrumentation.Emitters
{
    /// <summary>
    /// Emitter for the Google Chrome integrated instrumentor visualizer "chrome://tracing/" (only works on chrome)
    /// It serialize the instrumentation session to the Json format of the tool and writes to the provided stream
    /// Docs for the tool can be found here: https://docs.google.com/document/d/1CvAClvFfyA5R-PhYUmn5OOQtYMH4h6I0nSsKchNAySU/preview
    /// </summary>
    public class GoogleChromeInstrumentorJsonFormatSessionEmitter : IInstrumentationSessionEmitter
    {
        public Stream DataStream { get; }

        public GoogleChromeInstrumentorJsonFormatSessionEmitter(Stream dataStream)
        {
            this.DataStream = dataStream;
        }

        public void Emit(InstrumentationSession session)
        {
            const long TicksPerMicrosecond = 10;

            var traceData = new Google.Chrome.Tracing.TraceData();
            traceData.otherData.version = session.InstrumentorName;

            foreach(var entry in session.InstrumentationEntries)
            {
                var traceEvent = new Google.Chrome.Tracing.TraceEvent();
                traceEvent.name = this.GetName(entry);
                traceEvent.cat = Google.Chrome.Tracing.TraceEvent.Categories.Function;
                traceEvent.ph = Google.Chrome.Tracing.TraceEvent.Phases.CompleteEvents.CompleteEvent;
                traceEvent.ts = entry.StartTime.Ticks / TicksPerMicrosecond;
                traceEvent.dur = entry.Elapsed.Ticks / TicksPerMicrosecond;
                traceEvent.pid = entry.ProcessId;
                traceEvent.tid = entry.ThreadId;

                traceData.traceEvents.Add(traceEvent);
            }

            System.Text.Json.JsonSerializer.Serialize(this.DataStream, traceData);
        }

        private string GetName(InstrumentationEntry entry)
        {
            if (string.IsNullOrEmpty(entry.Label))
                return entry.FunctionName;

            return $"{entry.Label} -> {entry.FunctionName}";
        }

        public void Dispose()
        {
        }
    }
}

namespace Google.Chrome.Tracing
{
    public class TraceData
    {
        public TraceData()
        {
            this.traceEvents = new List<TraceEvent>();
            this.otherData = new TraceOtherData();
        }

        public List<TraceEvent> traceEvents { get; set; }

        public string displayTimeUnit { get; set; }

        public string systemTraceEvents { get; set; }

        public TraceOtherData otherData { get; set; }
    }

    public class TraceEvent
    {
        /// <summary>
        /// The name of the event, as displayed in Trace Viewer
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// The event categories. 
        /// This is a comma separated list of categories for the event. 
        /// The categories can be used to hide events in the Trace Viewer UI
        /// </summary>
        public string cat { get; set; }

        /// <summary>
        ///  The event type. 
        ///  This is a single character which changes depending on the type of event being output. 
        ///  The valid values are listed in the table below. 
        ///  We will discuss each phase type below.
        /// </summary>
        public string ph { get; set; }

        /// <summary>
        /// The tracing clock timestamp of the event. 
        /// The timestamps are provided at microsecond granularity.
        /// </summary>
        public long ts { get; set; }

        /// <summary>
        /// The duration of the event. 
        /// The durationss are provided at microsecond granularity.
        /// </summary>
        public long dur { get; set; }

        /// <summary>
        /// The process ID for the process that output this event.
        /// </summary>
        public int pid { get; set; }

        /// <summary>
        /// The thread ID for the thread that output this event.
        /// </summary>
        public int tid { get; set; }

        public class Categories
        {
            public const string Function = "function";
        }

        public class Phases
        {
            public class DurationEvents
            {
                public const string Begin = "B";
                public const string End = "E";
            }

            public class CompleteEvents
            {
                public const string CompleteEvent = "X";
            }

            public class InstantEvents
            {
                public const string InstantEvent = "i";
            }

            public class CounterEvents
            {
                public const string CounterEvent = "C";
            }

            public class AsyncEvents
            {
                public const string NestableStart = "b";
                public const string NestableInstant = "n";
                public const string NestableEnd = "e";
            }

            public class FlowEvents
            {
                public const string Start = "s";
                public const string Stop = "t";
                public const string End = "f";
            }

            public class SampleEvents
            {
                public const string SampleEvent = "P";
            }

            public class ObjectEvents
            {
                public const string Created = "N";
                public const string Snapshot = "O";
                public const string Destroyed = "D";
            }

            public class MetadataEvents
            {
                public const string MetadataEvent = "M";
            }

            public class MemoryDumoEvents
            {
                public const string Global = "V";
                public const string Process = "v";
            }

            public class MarkEvents
            {
                public const string MarkEvent = "R";
            }

            public class ClockSyncEvents
            {
                public const string ClockSyncEvent = "c";
            }
        }
    }

    public class TraceOtherData
    {
        public string version { get; set; }
    }
}
