using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiler.NET
{
    public interface ISession
    {
        /// <summary>
        /// Session Start Time
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// Returns whether the session is running or not
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Returns the elapsed time since the start of the session
        /// </summary>
        TimeSpan Elapsed { get; }

        /// <summary>
        /// Stop the Session
        /// </summary>
        /// <returns>the current instance of the Session</returns>
        ISession Stop();
    }
}
