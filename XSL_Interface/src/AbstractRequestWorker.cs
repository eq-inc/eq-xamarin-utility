using System;
using System.Collections.Generic;

namespace Eq.Utility
{
    public abstract class AbstractRequestWorker : IRequestWorker
    {
        protected RequestHandler mHandler;
        protected ThreadType mThreadType = ThreadType.Main;
        protected IThreadUtil mThreadUtil;
        protected Queue<Request> mRequestQueue;

        protected abstract IThreadUtil GetThreadUtil();

        public AbstractRequestWorker(RequestHandler handler) : this(handler, ThreadType.Main)
        {
        }

        public AbstractRequestWorker(RequestHandler handler, ThreadType threadType)
        {
            mHandler = handler ?? throw new ArgumentNullException("handler == nunll");
            mThreadType = threadType;
            if(threadType == ThreadType.Work)
            {
                mRequestQueue = new Queue<Request>();
            }
            mThreadUtil = GetThreadUtil();
        }

        public void SendRequest(int what)
        {
            SendRequest(what, 0, 0, null);
        }

        public void SendRequest(int what, object obj)
        {
            SendRequest(what, 0, 0, obj);
        }

        public void SendRequest(int what, int arg1, int arg2)
        {
            SendRequest(what, arg1, arg2, null);
        }

        public void SendRequest(int what, int arg1, int arg2, object obj)
        {
            Request request = new Request
            {
                what = what,
                arg1 = arg1,
                arg2 = arg2,
                obj = obj,
            };

            switch (mThreadType)
            {
                case ThreadType.Main:
                    mThreadUtil.RunOnMainThread(delegate ()
                    {
                        mHandler(request);
                    });
                    break;
                case ThreadType.Work:
                    lock (mRequestQueue)
                    {
                        mRequestQueue.Enqueue(request);
                        mThreadUtil.RunOnWorkThread(delegate ()
                        {
                            lock (mRequestQueue)
                            {
                                try
                                {
                                    mHandler(mRequestQueue.Dequeue());
                                }
                                catch (InvalidOperationException)
                                {

                                }
                            }
                        });
                    }
                    break;
            }
        }

        public void SendRequestDelayed(int what, long delayMillis)
        {
            SendRequestDelayed(what, 0, 0, null, delayMillis);
        }

        public void SendRequestDelayed(int what, object obj, long delayMillis)
        {
            SendRequestDelayed(what, 0, 0, obj, delayMillis);
        }

        public void SendRequestDelayed(int what, int arg1, int arg2, long delayMillis)
        {
            SendRequestDelayed(what, arg1, arg2, null, delayMillis);
        }

        public void SendRequestDelayed(int what, int arg1, int arg2, object obj, long delayMillis)
        {
            Request request = new Request
            {
                what = what,
                arg1 = arg1,
                arg2 = arg2,
                obj = obj,
            };

            switch (mThreadType)
            {
                case ThreadType.Main:
                    mThreadUtil.RunOnMainThreadDelayed(delegate ()
                    {
                        mHandler(request);
                    },
                    delayMillis);
                    break;
                case ThreadType.Work:
                    lock (mRequestQueue)
                    {
                        mRequestQueue.Enqueue(request);
                        mThreadUtil.RunOnWorkThreadDelayed(delegate ()
                        {
                            lock (mRequestQueue)
                            {
                                try
                                {
                                    mHandler(mRequestQueue.Dequeue());
                                }
                                catch (InvalidOperationException)
                                {

                                }
                            }
                        }, delayMillis);
                    }
                    break;
            }
        }

        private class Worker
        {

        }

    }
}
