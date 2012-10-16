using System;
using System.Diagnostics;

namespace Fibrous.Tests
{
    public class PerfTimer : IDisposable
    {
        private readonly int _count;
        private readonly Stopwatch _stopWatch;

        public PerfTimer(int count)
        {
            _count = count;
            _stopWatch = Stopwatch.StartNew();
        }

        #region IDisposable Members

        public void Dispose()
        {
            _stopWatch.Stop();
            long elapsed = _stopWatch.ElapsedMilliseconds;
            Console.WriteLine("Elapsed: " + elapsed + " Actions: " + _count);
            Console.WriteLine("actions/ms: " + (_count/elapsed));
        }

        #endregion
    }
}