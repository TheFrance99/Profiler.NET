namespace Profiler.NET.Instrumentation
{
    public interface IInstrumentor : IProfilerComponent
    {
        /// <summary>
        /// Gets the current Instrumentor Session
        /// </summary>
        InstrumentationSession Session { get; }

        /// <summary>
        /// Start the Instrumentation Session
        /// </summary>
        /// <returns>the current instance of the Instrumentor</returns>
        IInstrumentor Start();

        /// <summary>
        /// Stop the Instrumentation Session
        /// </summary>
        /// <returns>the current instance of the Instrumentor</returns>
        IInstrumentor Stop();
    }
}
