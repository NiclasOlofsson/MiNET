using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;

namespace MiNET
{
    [TestClass]
    public class BenchmarkEventHandlerGenerators
    {
        private const int Iterations = 250000;

        [TestMethod]
        public void BenchmarkReflectionEventHandlerGenerator()
        {
            Benchmark(() => new ReflectionPackageHandlerGenerator());
        }

        [TestMethod]
        public void BenchmarkEmittedEventHandlerGenerator()
        {
            Benchmark(() => new EmittedPackageHandlerGenerator());
        }

        private static void Benchmark(Func<IPackageEventHandlerGenerator> supplier)
        {
            var instance = new DummyHandler();
            var stopwatch = new Stopwatch();
            var package = new McpeText();
            var generator = supplier();

            MethodInfo site = instance.GetType().GetMethod("Handle", new[] { typeof(McpeText) });
            IPackageEventHandler handler = generator.Generate(instance, site, typeof(McpeText));

            // warm up
            for (int i = 0; i < Iterations / 10; i++)
            {
                handler.Handle(package, null);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            stopwatch.Start();
            for (int i = 0; i < Iterations; i++)
            {
                handler.Handle(package, null);
            }
            stopwatch.Stop();
            Trace.WriteLine(string.Format("Iterations: {0}", Iterations));
            Trace.WriteLine(string.Format("{0}: {1} ticks", generator.GetType().FullName, stopwatch.Elapsed.Ticks));
        }

        public class DummyHandler
        {
            [PacketHandler]
            public void Handle(McpeText package)
            {

            }
        }
    }
}
