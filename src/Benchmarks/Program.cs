using BenchmarkDotNet.Running;

namespace Benchmarks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<BuildAspNetBenchmark>();
        }
    }
}