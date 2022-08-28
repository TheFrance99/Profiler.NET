using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Profiler.NET.Instrumentation
{
    public sealed class Instrumentor : IInstrumentor
    {
        public string ProfilerName { get; }

        public string Name { get; }

        public InstrumentationSession Session { get; private set; }

        public bool IsRunning { get; private set; }

        public Instrumentor(string profilerName, string name)
        {
            this.ProfilerName = profilerName;
            this.Name = name;

            this.IsRunning = false;
        }

        public static IInstrumentor StartNew(string name)
        {
            return new Instrumentor("", name).Start();
        }

        public T ProfileFunc<T>(Expression<Func<T>> func)
        {
            return this.ProfileFunc("", func);
        }

        public T ProfileFunc<T>(string label, Expression<Func<T>> func)
        {
            var method = ((MethodCallExpression)func.Body).Method;

            var entry = this.Session.StartInstrumentMethod(label, Process.GetCurrentProcess().Id, Thread.CurrentThread.ManagedThreadId, method);

            var stopwatch = Stopwatch.StartNew();
            var res = func.Compile().Invoke();
            stopwatch.Stop();

            this.Session.StopInstrumentMethod(entry, stopwatch.Elapsed);

            return res;
        }

        public void ProfileAction(Expression<Action> action)
        {
            this.ProfileAction("", action);
        }

        public void ProfileAction(string label, Expression<Action> action)
        {
            var method = ((MethodCallExpression)action.Body).Method;

            var entry = this.Session.StartInstrumentMethod(label, Process.GetCurrentProcess().Id, Thread.CurrentThread.ManagedThreadId, method);

            var stopwatch = Stopwatch.StartNew();
            action.Compile().Invoke();
            stopwatch.Stop();

            this.Session.StopInstrumentMethod(entry, stopwatch.Elapsed);
        }

        public IInstrumentor Start()
        {
            if (this.IsRunning)
                this.Stop();

            this.Session = new InstrumentationSession(this.ProfilerName, this.Name);
            this.IsRunning = true;
            return this;
        }

        public IInstrumentor Stop()
        {
            if (!this.IsRunning)
                return this;

            this.Session.Stop();
            this.IsRunning = false;
            return this;
        }

        public void Dispose()
        {
        }
    }
}
