using CoreFoundation;
using Eq.Utility.iOS;
using Foundation;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(ThreadUtil_iOS))]
namespace Eq.Utility.iOS
{
    public class ThreadUtil_iOS : IThreadUtil
    {
        private static readonly string TAG = typeof(ThreadUtil_iOS).Name;

        public void RunOnMainThread(CsRunnable runnable)
        {
            NSOperationQueue.MainQueue.AddOperation(delegate ()
            {
                runnable();
            });
        }

        public void RunOnMainThread(CsRunnableWithParam runnable, params object[] paramArray)
        {
            NSOperationQueue.MainQueue.AddOperation(delegate ()
            {
                runnable(paramArray);
            });
        }

        public void RunOnMainThreadDelayed(CsRunnable runnable, long delayMillis)
        {
            DispatchQueue.MainQueue.DispatchAfter(new DispatchTime(((ulong)(DateTimeOffset.Now.ToUnixTimeMilliseconds() + delayMillis)) * 1000000), delegate ()
            {
                runnable();
            });
        }

        public void RunOnMainThreadDelayed(CsRunnableWithParam runnable, long delayMillis, params object[] paramArray)
        {
            DispatchQueue.MainQueue.DispatchAfter(new DispatchTime(((ulong)(DateTimeOffset.Now.ToUnixTimeMilliseconds() + delayMillis)) * 1000000), delegate ()
            {
                runnable(paramArray);
            });
        }

        public void RunOnWorkThread(CsRunnable runnable)
        {
            new NSOperationQueue().AddOperation(delegate () {
                runnable();
            });
        }

        public void RunOnWorkThread(CsRunnableWithParam runnable, params object[] paramArray)
        {
            new NSOperationQueue().AddOperation(delegate () {
                runnable(paramArray);
            });
        }

        public void RunOnWorkThreadDelayed(CsRunnable runnable, long delayMillis)
        {
            DispatchQueue.DefaultGlobalQueue.DispatchAfter(new DispatchTime(((ulong)(DateTimeOffset.Now.ToUnixTimeMilliseconds() + delayMillis)) * 1000000), delegate()
            {
                runnable();
            });
        }

        public void RunOnWorkThreadDelayed(CsRunnableWithParam runnable, long delayMillis, params object[] paramArray)
        {
            DispatchQueue.DefaultGlobalQueue.DispatchAfter(new DispatchTime(((ulong)(DateTimeOffset.Now.ToUnixTimeMilliseconds() + delayMillis)) * 1000000), delegate ()
            {
                runnable(paramArray);
            });
        }
    }
}
