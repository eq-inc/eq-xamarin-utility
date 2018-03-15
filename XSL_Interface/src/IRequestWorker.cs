namespace Eq.Utility
{
    public delegate void RequestHandler(Request request);

    public enum ThreadType
    {
        Main,
        Work,
    }

    public interface IRequestWorkerBuilder
    {
        IRequestWorker Create(RequestHandler requestHander);

        IRequestWorker Create(RequestHandler requestHander, ThreadType threadType);
    }

    public interface IRequestWorker
    {
        void SendRequest(int what);

        void SendRequest(int what, object obj);

        void SendRequest(int what, int arg1, int arg2);

        void SendRequest(int what, int arg1, int arg2, object obj);

        void SendRequestDelayed(int what, long delayMillis);

        void SendRequestDelayed(int what, object obj, long delayMillis);

        void SendRequestDelayed(int what, int arg1, int arg2, long delayMillis);

        void SendRequestDelayed(int what, int arg1, int arg2, object obj, long delayMillis);
    }

    public class Request
    {
        public int what;
        public int arg1;
        public int arg2;
        public object obj;
    }
}
