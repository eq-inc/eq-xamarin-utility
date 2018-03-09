using Android.OS;
using System.Collections.Generic;

namespace Eq.Utility.Droid
{
    public class ExtHandler
    {
        private Handler mHandler;
        private Dictionary<long, object> mParamDic = new Dictionary<long, object>();
        public delegate bool MessageHandler(Message msg);

        public ExtHandler()
        {
            mHandler = new Handler();
        }

        public ExtHandler(Looper looper)
        {
            mHandler = new Handler(looper);
        }

        public ExtHandler(MessageHandler callback)
        {
            mHandler = new Handler(new LocalHandlerCallback(this, callback));
        }

        public ExtHandler(Looper looper, MessageHandler callback)
        {
            mHandler = new Handler(looper, new LocalHandlerCallback(this, callback));
        }

        public Message ObtainMessage(int what)
        {
            return ObtainMessage(what, 0, 0, null);
        }

        public Message ObtainMessage(int what, object csObject)
        {
            return ObtainMessage(what, 0, 0, csObject);
        }

        public Message ObtainMessage(int what, int arg1, int arg2)
        {
            return ObtainMessage(what, arg1, arg2, null);
        }

        public Message ObtainMessage(int what, int arg1, int arg2, object csObject)
        {
            int hashCode = csObject == null ? 0 : csObject.GetHashCode();

            if(hashCode != 0)
            {
                mParamDic[hashCode] = csObject;
            }

            return mHandler.ObtainMessage(what, arg1, arg2, new Java.Lang.Long(hashCode));
        }

        public bool Post(Java.Lang.IRunnable runnable)
        {
            return mHandler.Post(runnable);
        }

        public bool Post(System.Action action)
        {
            return mHandler.Post(action);
        }

        public bool PostAtFrontOfQueue(Java.Lang.IRunnable runnable)
        {
            return mHandler.PostAtFrontOfQueue(runnable);
        }

        public bool PostAtFrontOfQueue(System.Action action)
        {
            return mHandler.PostAtFrontOfQueue(action);
        }

        public bool PostAtTime(Java.Lang.IRunnable runnable, long uptimeMillis)
        {
            return mHandler.PostAtTime(runnable, uptimeMillis);
        }

        public bool PostAtTime(System.Action action, long uptimeMillis)
        {
            return mHandler.PostAtTime(action, uptimeMillis);
        }

        public bool PostDelayed(Java.Lang.IRunnable runnable, long delayMillis)
        {
            return mHandler.PostDelayed(runnable, delayMillis);
        }

        public bool PostDelayed(System.Action action, long delayMillis)
        {
            return mHandler.PostDelayed(action, delayMillis);
        }

        public void SendEmptyMessage(int what)
        {
            mHandler.SendEmptyMessage(what);
        }

        public bool SendEmptyMessageAtTime(int what, long uptimeMillis)
        {
            return mHandler.SendEmptyMessageAtTime(what, uptimeMillis);
        }

        public bool SendEmptyMessageDelayed(int what, long delayMills)
        {
            return mHandler.SendEmptyMessageDelayed(what, delayMills);
        }

        public bool SendMessageAtTime(Message msg, long uptimeMilles)
        {
            return mHandler.SendMessageAtTime(msg, uptimeMilles);
        }

        public bool SendMessageDelayed(Message msg, long delayMills)
        {
            return mHandler.SendMessageDelayed(msg, delayMills);
        }

        public void SendMessage(Message msg)
        {
            mHandler.SendMessage(msg);
        }

        public bool SendMessageAtFrontOfQueue(Message msg)
        {
            return mHandler.SendMessageAtFrontOfQueue(msg);
        }

        public object GetCsObject(Message message)
        {
            object ret = null;
            Java.Lang.Object jObject = message.Obj;
            long hashCode = (jObject != null) ? ((Java.Lang.Long)jObject).LongValue() : 0L;

            if(hashCode != 0)
            {
                if(mParamDic.TryGetValue(hashCode, out ret))
                {
                    mParamDic.Remove(hashCode);
                }
                else
                {
                    ret = null;
                }
            }

            return ret;
        }

        private class LocalHandlerCallback : Java.Lang.Object, Handler.ICallback
        {
            private ExtHandler mHandler;
            private MessageHandler mCallback;

            public LocalHandlerCallback(ExtHandler handler, MessageHandler callback)
            {
                mHandler = handler ?? throw new System.NullReferenceException("handler == null");
                mCallback = callback ?? throw new System.NullReferenceException("callback == null");
            }

            public bool HandleMessage(Message msg)
            {
                bool ret = false;
                if (mCallback != null)
                {
                    ret = mCallback(msg);
                }
                mHandler.GetCsObject(msg);

                return ret;
            }
        }
    }
}