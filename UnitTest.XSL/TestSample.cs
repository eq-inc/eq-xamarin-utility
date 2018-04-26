using Eq.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;

namespace UnitTest.Xamarin
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
            IThreadUtil threadUtil = DependencyService.Get<IThreadUtil>();

            threadUtil.RunOnWorkThreadDelayed(delegate ()
            {
                long timeoutMS = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                if (threadUtil.IsMainThread())
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

                if (threadUtil.IsMainThread())
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
                if (threadUtil.IsMainThread())
                {
                    Assert.Fail();
                }
            });

            threadUtil.RunOnWorkThread(delegate (object[] paramArray)
            {
                if (threadUtil.IsMainThread())
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
                if (!threadUtil.IsMainThread())
                {
                    Assert.Fail();
                }
            });

            threadUtil.RunOnMainThread(delegate (object[] paramArray)
            {
                if (!threadUtil.IsMainThread())
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

                if (!threadUtil.IsMainThread())
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

                if (!threadUtil.IsMainThread())
                {
                    Assert.Fail();
                }

                if (timeoutMS < currentTime3MS + DelayTimeMS)
                {
                    Assert.Fail("expected timeout  = " + (currentTime4MS + DelayTimeMS) + ", real timeout = " + timeoutMS);
                }
            }, DelayTimeMS, null);
        }

        [Test]
        public void RequestWorkerTest()
        {
            LogController.GlobalOutputLogCategory = LogController.LogCategoryAll;

            List<int> whatList1 = new List<int>();
            IThreadUtil threadUtil = DependencyService.Get<IThreadUtil>();
            IRequestWorker requestWorker1 = DependencyService.Get<IRequestWorkerBuilder>().Create(delegate (Request request)
            {
                int targetWhat = whatList1[0];

                if(targetWhat != request.what)
                {
                    Assert.Fail("order illegal");
                }
                if (!threadUtil.IsMainThread())
                {
                    Assert.Fail("not work on main thread");
                }
                if((request.arg1 != 10) || (request.arg2 != 10) || !request.obj.Equals(whatList1))
                {
                    Assert.Fail("paramters are not enough");
                }

                whatList1.RemoveAt(0);
            });
            for(int i=0; i<10; i++)
            {
                whatList1.Add(i);
                requestWorker1.SendRequest(i, 10, 10, whatList1);
            }

            List<int> whatList2 = new List<int>();
            IRequestWorker requestWorker2 = DependencyService.Get<IRequestWorkerBuilder>().Create(delegate (Request request)
            {
                int targetWhat = whatList2[0];

                if (targetWhat != request.what)
                {
                    Assert.Fail("order illegal");
                }
                if (threadUtil.IsMainThread())
                {
                    Assert.Fail("not work on work thread");
                }
                if ((request.arg1 != 10) || (request.arg2 != 10) || !request.obj.Equals(whatList2))
                {
                    Assert.Fail("paramters are not enough");
                }

                whatList2.RemoveAt(0);
            }, ThreadType.Work);
            for (int i = 0; i < 10; i++)
            {
                whatList2.Add(i);
                requestWorker2.SendRequest(i, 10, 10, whatList2);
            }

            List<int> whatList3 = new List<int>();
            IRequestWorker requestWorker3 = DependencyService.Get<IRequestWorkerBuilder>().Create(delegate (Request request)
            {
                int targetWhat = whatList3[0];

                if (targetWhat != request.what)
                {
                    Assert.Fail("order illegal");
                }
                if (!threadUtil.IsMainThread())
                {
                    Assert.Fail("not work on main thread");
                }
                if ((request.arg1 != 10) || (request.arg2 != 10) || !request.obj.Equals(whatList3))
                {
                    Assert.Fail("paramters are not enough");
                }

                whatList3.RemoveAt(0);
            });
            for (int i = 0; i < 10; i++)
            {
                whatList3.Add(i);
                requestWorker3.SendRequestDelayed(i, 10, 10, whatList3, 1000);
            }

            List<int> whatList4 = new List<int>();
            IRequestWorker requestWorker4 = DependencyService.Get<IRequestWorkerBuilder>().Create(delegate (Request request)
            {
                int targetWhat = whatList4[0];

                if (targetWhat != request.what)
                {
                    Assert.Fail("order illegal");
                }
                if (threadUtil.IsMainThread())
                {
                    Assert.Fail("not work on work thread");
                }
                if ((request.arg1 != 10) || (request.arg2 != 10) || !request.obj.Equals(whatList4))
                {
                    Assert.Fail("paramters are not enough");
                }

                whatList4.RemoveAt(0);
            }, ThreadType.Work);
            for (int i = 0; i < 10; i++)
            {
                whatList4.Add(i);
                requestWorker4.SendRequestDelayed(i, 10, 10, whatList4, 1000);
            }
        }

        [Test]
        public void JsonConvertTest()
        {
            RequestParam requestParam = new RequestParam();
            Dictionary<string, object> jsonInstance = new Dictionary<string, object>();
            jsonInstance["param1"] = "value1";
            jsonInstance["param2"] = 2;
            jsonInstance["param3"] = requestParam;
            requestParam.param1 = "2-1";
            requestParam.param2 = 22;
            requestParam.param3 = new string[] { "3-3-0", "3-3-1", "3-3-2", "3-3-3", "3-3-4", "3-3-5", "3-3-6", "3-3-7", "3-3-8", "3-3-9" };
            requestParam.param4 = new int[] { 330, 331, 332, 333, 334, 335, 336, 337, 338, 339 };

            string jsonInstanceStr = Newtonsoft.Json.JsonConvert.SerializeObject(jsonInstance);
            Assert.True(jsonInstanceStr.Contains("\"param1\""));
            Assert.True(jsonInstanceStr.Contains("\"value1\""));
            Assert.True(jsonInstanceStr.Contains("\"param2\""));
            Assert.True(jsonInstanceStr.Contains("2"));
            Assert.True(jsonInstanceStr.Contains("\"param3\""));
            Assert.True(jsonInstanceStr.Contains("\"2-1\""));
            Assert.True(jsonInstanceStr.Contains("22"));
            Assert.True(jsonInstanceStr.Contains("\"3-3-0\""));
            Assert.True(jsonInstanceStr.Contains("\"3-3-1\""));
            Assert.True(jsonInstanceStr.Contains("\"3-3-2\""));
            Assert.True(jsonInstanceStr.Contains("\"3-3-3\""));
            Assert.True(jsonInstanceStr.Contains("\"3-3-4\""));
            Assert.True(jsonInstanceStr.Contains("\"3-3-5\""));
            Assert.True(jsonInstanceStr.Contains("\"3-3-6\""));
            Assert.True(jsonInstanceStr.Contains("\"3-3-7\""));
            Assert.True(jsonInstanceStr.Contains("\"3-3-8\""));
            Assert.True(jsonInstanceStr.Contains("\"3-3-9\""));

            Assert.True(jsonInstanceStr.Contains("330"));
            Assert.True(jsonInstanceStr.Contains("331"));
            Assert.True(jsonInstanceStr.Contains("332"));
            Assert.True(jsonInstanceStr.Contains("333"));
            Assert.True(jsonInstanceStr.Contains("334"));
            Assert.True(jsonInstanceStr.Contains("335"));
            Assert.True(jsonInstanceStr.Contains("336"));
            Assert.True(jsonInstanceStr.Contains("337"));
            Assert.True(jsonInstanceStr.Contains("338"));
            Assert.True(jsonInstanceStr.Contains("339"));
        }

        [Test]
        public async void ApiAccessHelperTest()
        {
            RequestToken requestToken = new RequestToken();
            ApiAccessHelper<RequestToken, ResponseToken> apiAccessHelper = new ApiAccessHelper<RequestToken, ResponseToken>();
            apiAccessHelper.Method = ApiAccessHelper<RequestToken, ResponseToken>.MethodType.Post;
            apiAccessHelper.Url = "http://api.zipaddress.net/?zipcode=2700023";
            apiAccessHelper.RequestEntity = requestToken;

            ResponseToken response = await apiAccessHelper.ExecuteAsync();
            Assert.NotNull(response);
            Assert.NotNull(response.data);
        }

        [Test]
        public void ApiAccessHelperTest2()
        {
            ApiAccessHelper<RequestToken, ResponseToken> apiAccessHelper = new ApiAccessHelper<RequestToken, ResponseToken>();
            apiAccessHelper.Method = ApiAccessHelper<RequestToken, ResponseToken>.MethodType.Post;
            apiAccessHelper.Url = "http://api.zipaddress.net/?zipcode=2700023";

            ResponseToken response = apiAccessHelper.Execute();
            Assert.NotNull(response);
            Assert.NotNull(response.data);
        }

        private class Dummy
        {
            string dummy;
        }

        public class RequestToken
        {
            public string param1;
            public int param2;
            public RequestParam param3;
            public string[] param4;
            public int[] param5;
        }

        public class RequestParam
        {
            public string param1;
            public int param2;
            public string[] param3;
            public int[] param4;
        }

        public class ResponseToken
        {
            public int code;
            public ResponseParam data;
        }

        public class ResponseParam
        {
            public string pref;
            public string address;
            public string city;
            public string town;
            public string fullAddress;
        }
    }
}