﻿namespace Fibrous.Benchmark
{
    using BenchmarkDotNet.Attributes;

    [MemoryDiagnoser]
    public class GC_Action
    {
        private IFiber _fiber;
        readonly IChannel<object> _channel = new Channel<object>();
        readonly object _msg = new object();

        [Benchmark]
        public void Publish()
        {
            _channel.Publish(_msg);
        }

        [GlobalSetup]
        public void Setup()
        {
            _fiber = PoolFiber_OLD.StartNew();
            _channel.Subscribe(_fiber, o => { });
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _fiber.Dispose();
        }
    }
}