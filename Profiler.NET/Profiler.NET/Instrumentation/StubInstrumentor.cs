using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Profiler.NET.Instrumentation
{
    public sealed class StubInstrumentor : IInstrumentor
    {
        public InstrumentationSession Session => null;

        public bool IsRunning => false;

        public T ProfileFunc<T>(Expression<Func<T>> func)
        {
            return this.ProfileFunc("", func);
        }

        public T ProfileFunc<T>(string label, Expression<Func<T>> func)
        {
            return func.Compile().Invoke();
        }

        public void ProfileAction(Expression<Action> action)
        {
            this.ProfileAction("", action);
        }

        public void ProfileAction(string label, Expression<Action> action)
        {
            action.Compile().Invoke();
        }

        public IInstrumentor Start()
        {
            return this;
        }

        public IInstrumentor Stop()
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
}
