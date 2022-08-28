using Profiler.NET.Instrumentation;
using Profiler.NET.Instrumentation.Emitters;
using System.IO;
using Xunit;

namespace Profiler.NET.Tests
{
    public class InstrumentorTest
    {
        [Fact]
        public void TestProfileFunc()
        {
            using (var instrumentor = Instrumentor.StartNew("test"))
            {
                var test = "Lorem Ipsum";
                instrumentor.ProfileFunc(() => test.ToUpper());
                instrumentor.ProfileFunc("with label", () => test.ToUpper());
                instrumentor.Stop();
            }
        }

        [Fact]
        public void TestProfileAction()
        {
            using (var instrumentor = Instrumentor.StartNew("test"))
            {
                var test = "Lorem Ipsum";
                instrumentor.ProfileAction(() => test.ToUpper());
                instrumentor.ProfileAction("with label", () => test.ToUpper());
                instrumentor.Stop();
            }
        }

        [Fact]
        public void TestProfileFuncGoogleChromeEmitter()
        {
            using (var outStream = new MemoryStream())
            {
                using (var instrumentor = Instrumentor.StartNew("test"))
                {
                    var test = "Lorem Ipsum";
                    instrumentor.ProfileFunc(() => test.ToUpper());
                    instrumentor.ProfileFunc("with label", () => test.ToUpper());
                    instrumentor.Stop();

                    using (var emitter = new GoogleChromeInstrumentorJsonFormatSessionEmitter(outStream))
                    {
                        emitter.Emit(instrumentor.Session);
                    }
                }
            }
        }

        [Fact]
        public void TestProfileActionGoogleChromeEmitter()
        {
            using (var outStream = new MemoryStream())
            {
                using (var instrumentor = Instrumentor.StartNew("test"))
                {
                    var test = "Lorem Ipsum";
                    instrumentor.ProfileAction(() => test.ToUpper());
                    instrumentor.ProfileAction("with label", () => test.ToUpper());
                    instrumentor.Stop();

                    using (var emitter = new GoogleChromeInstrumentorJsonFormatSessionEmitter(outStream))
                    {
                        emitter.Emit(instrumentor.Session);
                    }
                }
            }
        }

    }
}