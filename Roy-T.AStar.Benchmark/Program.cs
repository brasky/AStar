using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Roy_T.AStar.Benchmark
{
    public class Program
    {
        static void Main(string[] _)
        {
            var config = DefaultConfig.Instance.AddJob(Job.ShortRun);
            BenchmarkRunner.Run<AStarBenchmark>(config);
            Console.ReadLine();
        }
    }
}
