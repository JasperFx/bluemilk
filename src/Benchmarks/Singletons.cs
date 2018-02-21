using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    public class GetByType : ComparisonBenchmark
    {
        [Benchmark]
        public void Objects()
        {
            buildAll(objects);
        }
        
        
        /*
        [Benchmark]
        public void Singletons()
        {
            buildAll(singletons);
        }
        */
        
        /*
        [Benchmark]
        public void Scope()
        {
            buildAll(scoped);
        }
        
        
        [Benchmark]
        public void Transients()
        {
            buildAll(transients);
        }
        
        

        
                
        [Benchmark]
        public void Lambdas()
        {
            buildAll(lambdas);
        }
        
        [Benchmark]
        public void Internals()
        {
            buildAll(internals);
        }
        */
    }
}