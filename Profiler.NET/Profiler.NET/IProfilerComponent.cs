using System.Linq.Expressions;

namespace Profiler.NET
{
    public interface IProfilerComponent : IDisposable
    {
        /// <summary>
        /// Return whether the component is running or not
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Used to analyze a method that return a Type different than void
        /// </summary>
        /// <typeparam name="T">The returned Type of the analyzed method</typeparam>
        /// <param name="func">The Method that you want to analyze</param>
        /// <returns></returns>
        T ProfileFunc<T>(Expression<Func<T>> func);

        /// <summary>
        /// Used to analyze a method that return a Type different than void
        /// </summary>
        /// <typeparam name="T">The returned Type of the analyzed method</typeparam>
        /// <param name="label">A label that can be used to add additional info</param>
        /// <param name="func">The Method that you want to analyze</param>
        /// <returns></returns>
        T ProfileFunc<T>(string label, Expression<Func<T>> func);

        /// <summary>
        /// Used to analyze a method that return void
        /// </summary>
        /// <param name="action">The Method that you want to analyze</param>
        void ProfileAction(Expression<Action> action);

        /// <summary>
        /// Used to analyze a method that return void
        /// </summary>
        /// <param name="label">A label that can be used to add additional info</param>
        /// <param name="action">The Method that you want to analyze</param>
        void ProfileAction(string label, Expression<Action> action);
    }
}