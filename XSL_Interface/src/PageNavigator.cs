using System.Collections.Generic;
using Xamarin.Forms;

namespace Eq.Utility
{
    public class PageNavigator
    {
        private static readonly object sSemaphore = new object();
        private static readonly PageNavigator sInstance = new PageNavigator();

        public static PageNavigator Instance
        {
            get
            {
                return sInstance;
            }
        }

        private Stack<Page> mPageStack = new Stack<Page>();

        private PageNavigator()
        {

        }

        public void Push(Page nextPage)
        {
            if(Application.Current.MainPage != null)
            {
                mPageStack.Push(Application.Current.MainPage);
            }

            Application.Current.MainPage = nextPage;
        }

        public bool Pop()
        {
            Page prevPage = mPageStack.Count > 0 ? mPageStack.Pop() : null;
            bool ret = false;

            if (prevPage != null)
            {
                Application.Current.MainPage = prevPage;
                ret = true;
            }
            else
            {
                ret = false;
            }

            return ret;
        }
    }
}
