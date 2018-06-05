using NUnit.Framework;

namespace UnitTest.Xamarin.Android
{
    [TestFixture]
    public class TestAndroid
    {
        [Test]
        public void ApiTest()
        {
            string text = global::Android.App.Application.Context.CacheDir.AbsolutePath;
            Assert.True(true);
        }
    }
}