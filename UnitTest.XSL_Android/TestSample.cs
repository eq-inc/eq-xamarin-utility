using Android.OS;
using Eq.Utility;
using Eq.Utility.Droid;
using Java.Lang;
using NUnit.Framework;
using System;
using System.Reflection;


namespace UnitTest.Xamarin.Android
{
    [TestFixture]
    public class TestUtility
    {
        [Test]
        public void ReadWriteTest()
        {
            object[] valueArray =
            {
                (byte)10, (char)11, (short)12, (int)13, (ushort)14,  (UInt32)15, (UInt64)16, (long)17, "18", (float)19.0f, (double)20.0
            };

            for (int i = 0, size = valueArray.Length; i < size; i++)
            {
                PreferenceUtil.Write("key" + i.ToString(), valueArray[i]);
            }

            for (int i = 0, size = valueArray.Length; i < size; i++)
            {
                if (PreferenceUtil.Read("key" + i.ToString(), out object tempObject))
                {
                    if (!tempObject.GetType().Equals(valueArray[i].GetType()))
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    Assert.Fail();
                }
            }

            for (int i = 0, size = valueArray.Length; i < size; i++)
            {
                if (PreferenceUtil.Read("key" + i.ToString(), out Dummy test))
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        public void FlushTest()
        {
            object[] valueArray =
            {
                (byte)10, (char)11, (short)12, (int)13, (ushort)14,  (UInt32)15, (UInt64)16, (long)17, "18", (float)19.0f, (double)20.0
            };

            for (int i = 0, size = valueArray.Length; i < size; i++)
            {
                PreferenceUtil.Write("key" + i.ToString(), valueArray[i]);
            }

            PreferenceUtil.Flush();

            Type prefUtilType = typeof(PreferenceUtil);
            MethodInfo clearCacheMethodInfo = prefUtilType.GetMethod("ClearCache", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            clearCacheMethodInfo.Invoke(null, null);

            for (int i = 0, size = valueArray.Length; i < size; i++)
            {
                if (!PreferenceUtil.Read("key" + i.ToString(), out object tempObject))
                {
                    Assert.Fail();
                }
            }
        }

        private static readonly long DelayTimeMS = 1000;

        [Test]
        public void ThreadUtilTest()
        {
            long currentTime1MS = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            IThreadUtil threadUtil = new ThreadUtil_Android();
            threadUtil.RunOnWorkThreadDelayed(delegate ()
            {
                long timeoutMS = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                if (Looper.MainLooper.Thread.Equals(Thread.CurrentThread()))
                {
                    Assert.Fail();
                }

                if (timeoutMS < currentTime1MS + DelayTimeMS)
                {
                    Assert.Fail("expected timeout  = " + (currentTime1MS + DelayTimeMS) + ", real timeout = " + timeoutMS);
                }
            }, DelayTimeMS);

            long currentTime2MS = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            threadUtil.RunOnWorkThreadDelayed(delegate (object[] paramArray)
            {
                long timeoutMS = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                if (Looper.MainLooper.Thread.Equals(Thread.CurrentThread()))
                {
                    Assert.Fail();
                }

                if (timeoutMS < currentTime1MS + DelayTimeMS)
                {
                    Assert.Fail("expected timeout  = " + (currentTime2MS + DelayTimeMS) + ", real timeout = " + timeoutMS);
                }
            }, DelayTimeMS, null);

            threadUtil.RunOnWorkThread(delegate ()
            {
                if (Looper.MainLooper.Thread.Equals(Thread.CurrentThread()))
                {
                    Assert.Fail();
                }
            });

            threadUtil.RunOnWorkThread(delegate (object[] paramArray)
            {
                if (Looper.MainLooper.Thread.Equals(Thread.CurrentThread()))
                {
                    Assert.Fail();
                }

                if (paramArray.Length != 4)
                {
                    Assert.Fail();
                }
                else if (((int)paramArray[0]) != 1)
                {
                    Assert.Fail();
                }
                else if (((string)paramArray[1]) != "2")
                {
                    Assert.Fail();
                }
                else if (((float)paramArray[2]) != 3f)
                {
                    Assert.Fail();
                }
                else if (((double)paramArray[3]) != 4.0)
                {
                    Assert.Fail();
                }
            }, 1, "2", 3f, 4.0);

            threadUtil.RunOnMainThread(delegate ()
            {
                if (!Looper.MainLooper.Thread.Equals(Thread.CurrentThread()))
                {
                    Assert.Fail();
                }
            });

            threadUtil.RunOnMainThread(delegate (object[] paramArray)
            {
                if (!Looper.MainLooper.Thread.Equals(Thread.CurrentThread()))
                {
                    Assert.Fail();
                }

                if (paramArray.Length != 4)
                {
                    Assert.Fail();
                }
                else if (((int)paramArray[0]) != 1)
                {
                    Assert.Fail();
                }
                else if (((string)paramArray[1]) != "2")
                {
                    Assert.Fail();
                }
                else if (((float)paramArray[2]) != 3f)
                {
                    Assert.Fail();
                }
                else if (((double)paramArray[3]) != 4.0)
                {
                    Assert.Fail();
                }
            }, 1, "2", 3f, 4.0);

            long currentTime3MS = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            threadUtil.RunOnMainThreadDelayed(delegate ()
            {
                long timeoutMS = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                if (!Looper.MainLooper.Thread.Equals(Thread.CurrentThread()))
                {
                    Assert.Fail();
                }

                if (timeoutMS < currentTime3MS + DelayTimeMS)
                {
                    Assert.Fail("expected timeout  = " + (currentTime3MS + DelayTimeMS) + ", real timeout = " + timeoutMS);
                }
            }, DelayTimeMS);

            long currentTime4MS = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            threadUtil.RunOnMainThreadDelayed(delegate (object[] paramArray)
            {
                long timeoutMS = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                if (!Looper.MainLooper.Thread.Equals(Thread.CurrentThread()))
                {
                    Assert.Fail();
                }

                if (timeoutMS < currentTime3MS + DelayTimeMS)
                {
                    Assert.Fail("expected timeout  = " + (currentTime4MS + DelayTimeMS) + ", real timeout = " + timeoutMS);
                }
            }, DelayTimeMS, null);
        }

        private class Dummy
        {
            string dummy;
        }
    }
}