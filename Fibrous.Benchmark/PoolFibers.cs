﻿using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Fibrous.Experimental;

namespace Fibrous.Benchmark
{
    [MemoryDiagnoser]
    public class PoolFibers
    {
        private const int OperationsPerInvoke = 10000000;
        private readonly AutoResetEvent _wait = new AutoResetEvent(false);
        private IAsyncFiber _async;
        private IFiber _fiber;
        private int i;

        private void Handler()
        {
            i++;
            if (i == OperationsPerInvoke)
                _wait.Set();
        }

        private Task AsyncHandler()
        {
            i++;
            if (i == OperationsPerInvoke)
                _wait.Set();
            return Task.CompletedTask;
        }

        public void Run(IFiber fiber)
        {
            i = 0;
            for (var j = 0; j < OperationsPerInvoke; j++) fiber.Enqueue(Handler);
            WaitHandle.WaitAny(new WaitHandle[] {_wait});
        }

        public void Run(IAsyncFiber fiber)
        {
            i = 0;
            for (var j = 0; j < OperationsPerInvoke; j++) fiber.Enqueue(AsyncHandler);
            WaitHandle.WaitAny(new WaitHandle[] {_wait});
        }

        [Benchmark(OperationsPerInvoke = OperationsPerInvoke)]
        public void Fiber()
        {
            Run(_fiber);
        }

        [Benchmark(OperationsPerInvoke = OperationsPerInvoke)]
        public void Async()
        {
            Run(_async);
        }

        [IterationSetup]
        public void Setup()
        {
            _async = new AsyncFiber();
            _fiber = new Fiber();
        }

        [IterationCleanup]
        public void Cleanup()
        {
            _fiber.Dispose();
            _async.Dispose();
        }
    }
}