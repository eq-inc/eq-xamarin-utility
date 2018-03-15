using Android.OS;
using Eq.Utility.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(ThreadUtil_Android))]
namespace Eq.Utility.Droid
{
    public class ThreadUtil_Android : IThreadUtil
    {
        private static readonly string TAG = typeof(ThreadUtil_Android).Name;

        public bool IsMainThread()
        {
            return Java.Lang.Thread.CurrentThread().Equals(Looper.MainLooper.Thread);
        }

        public void RunOnMainThread(CsRunnable runnable)
        {
            new Handler(Looper.MainLooper).Post(new LocalRunnable(runnable, 0));
        }

        public void RunOnMainThread(CsRunnableWithParam runnable, params object[] paramArray)
        {
            new Handler(Looper.MainLooper).Post(new LocalRunnable(runnable, 0, paramArray));
        }

        public void RunOnMainThreadDelayed(CsRunnable runnable, long delayMillis)
        {
            new Handler(Looper.MainLooper).PostDelayed(new LocalRunnable(runnable, 0), delayMillis);
        }

        public void RunOnMainThreadDelayed(CsRunnableWithParam runnable, long delayMillis, params object[] paramArray)
        {
            new Handler(Looper.MainLooper).PostDelayed(new LocalRunnable(runnable, 0, paramArray), delayMillis);
        }

        public void RunOnWorkThread(CsRunnable runnable)
        {
            new Java.Lang.Thread(new LocalRunnable(runnable, 0)).Start();
        }

        public void RunOnWorkThread(CsRunnableWithParam runnable, params object[] paramArray)
        {
            new Java.Lang.Thread(new LocalRunnable(runnable, 0, paramArray)).Start();
        }

        public void RunOnWorkThreadDelayed(CsRunnable runnable, long delayMillis)
        {
            new Java.Lang.Thread(new LocalRunnable(runnable, delayMillis)).Start();
        }

        public void RunOnWorkThreadDelayed(CsRunnableWithParam runnable, long delayMillis, params object[] paramArray)
        {
            new Java.Lang.Thread(new LocalRunnable(runnable, delayMillis, paramArray)).Start();
        }

        class LocalRunnable : Java.Lang.Object, Java.Lang.IRunnable
        {
            private CsRunnable mRunnable;
            private CsRunnableWithParam mRunnableWithParam;
            private object[] mParamArray;
            private long mDelayMillis = 0;

            public LocalRunnable(CsRunnable runnable, long delayMillis)
            {
                mRunnable = runnable ?? throw new System.NullReferenceException("runnable == null");
                mDelayMillis = delayMillis;
            }

            public LocalRunnable(CsRunnableWithParam runnable, long delayMillis, params object[] paramArray)
            {
                mRunnableWithParam = runnable ?? throw new System.NullReferenceException("runnable == null");
                mDelayMillis = delayMillis;
                mParamArray = paramArray;
            }

            public void Run()
            {
                if(mDelayMillis > 0)
                {
                    Java.Lang.Thread.Sleep(mDelayMillis);
                }

                mRunnable?.Invoke();
                mRunnableWithParam?.Invoke(mParamArray);
            }
        }
    }
}
