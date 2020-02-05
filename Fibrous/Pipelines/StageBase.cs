﻿using System;
using System.Threading.Tasks;

namespace Fibrous.Pipelines
{
    public abstract class StageBase<TIn, TOut> : IStage<TIn, TOut>
    {
        protected readonly IChannel<TIn> In = new Channel<TIn>();
        protected readonly IChannel<TOut> Out = new Channel<TOut>();

        public void Publish(TIn msg)
        {
            In.Publish(msg);
        }

        public IDisposable Subscribe(IFiber fiber, Action<TOut> receive)
        {
            return Out.Subscribe(fiber, receive);
        }

        public IDisposable Subscribe(IAsyncFiber fiber, Func<TOut, Task> receive)
        {
            return Out.Subscribe(fiber, receive);
        }

        public IDisposable Subscribe(Action<TOut> receive)
        {
            return Out.Subscribe(receive);
        }

        public abstract void Dispose();
    }
}