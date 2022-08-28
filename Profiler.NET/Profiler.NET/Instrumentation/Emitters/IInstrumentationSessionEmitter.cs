using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiler.NET.Instrumentation.Emitters
{
    public interface IInstrumentationSessionEmitter : IDisposable
    {
        /// <summary>
        /// Enable to emit a session to an output format
        /// </summary>
        /// <param name="session">The session to emit</param>
        void Emit(InstrumentationSession session);
    }
}
