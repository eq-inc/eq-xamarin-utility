namespace Eq.Utility
{
    public delegate void CsRunnable();
    public delegate void CsRunnableWithParam(object[] paramArray);

    public interface IThreadUtil
    {
        void RunOnMainThread(CsRunnable runnable);

        void RunOnMainThread(CsRunnableWithParam runnable, params object[] paramArray);

        void RunOnMainThreadDelayed(CsRunnable runnable, long delayMillis);

        void RunOnMainThreadDelayed(CsRunnableWithParam runnable, long delayMillis, params object[] paramArray);

        void RunOnWorkThread(CsRunnable runnable);

        void RunOnWorkThread(CsRunnableWithParam runnable, params object[] paramArray);

        void RunOnWorkThreadDelayed(CsRunnable runnable, long delayMillis);

        void RunOnWorkThreadDelayed(CsRunnableWithParam runnable, long delayMillis, params object[] paramArray);
    }
}
