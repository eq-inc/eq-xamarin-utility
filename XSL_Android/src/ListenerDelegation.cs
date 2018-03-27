using Android.Views;
using Android.Widget;
using System;

namespace XSL_Android
{
    public class Delegator
    {
        public class BaseListener : Java.Lang.Object
        {
            protected object mOwner;

            internal BaseListener(object owner)
            {
                mOwner = owner;
            }

            public object Owner
            {
                get
                {
                    return mOwner;
                }
                set
                {
                    mOwner = value;
                }
            }
        }

        public class ViewClickListener : BaseListener, View.IOnClickListener
        {
            public delegate void OnClickDelegate(object owner, View view);

            private OnClickDelegate mDelegator;

            public ViewClickListener(OnClickDelegate delegator, object owner=null) : base(owner)
            {
                mDelegator = delegator ?? throw new ArgumentNullException("delegator == null");
            }

            public void OnClick(View v)
            {
                mDelegator(mOwner, v);
            }
        }

        public class ViewTouchListener : BaseListener, View.IOnTouchListener
        {
            public delegate bool OnTouchDelegate(object owner, View v, MotionEvent e);

            private OnTouchDelegate mDelegator;

            public ViewTouchListener(OnTouchDelegate delegator, object owner = null) : base(owner)
            {
                mDelegator = delegator ?? throw new ArgumentNullException("delegator == null");
            }

            public bool OnTouch(View v, MotionEvent e)
            {
                return mDelegator(mOwner, v, e);
            }
        }

        public class CompoundButtonCheckedChangeListener : BaseListener, CompoundButton.IOnCheckedChangeListener
        {
            public delegate void OnCheckedChangeDelegate(object owner, CompoundButton buttonView, bool isChecked);

            private OnCheckedChangeDelegate mDelegator;

            public CompoundButtonCheckedChangeListener(OnCheckedChangeDelegate delegator, object owner=null) : base(owner)
            {
                mDelegator = delegator ?? throw new ArgumentNullException("delegator == null");
            }

            public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
            {
                mDelegator(mOwner, buttonView, isChecked);
            }
        }
    }
}
