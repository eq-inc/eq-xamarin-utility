using Eq.Utility.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(RequestWorkerBuilder))]
namespace Eq.Utility.iOS
{
    public class RequestWorkerBuilder : IRequestWorkerBuilder
    {
        public IRequestWorker Create(RequestHandler requestHander)
        {
            return new RequestWorker(requestHander);
        }

        public IRequestWorker Create(RequestHandler requestHander, ThreadType threadType)
        {
            return new RequestWorker(requestHander, threadType);
        }
    }

    public class RequestWorker : AbstractRequestWorker
    {
        public RequestWorker(RequestHandler handler) : this(handler, ThreadType.Main)
        {
        }

        public RequestWorker(RequestHandler handler, ThreadType threadType) : base(handler, threadType)
        {
        }

        protected override IThreadUtil GetThreadUtil()
        {
            return new ThreadUtil_iOS();
        }
    }
}
