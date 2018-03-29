using Eq.Utility.iOS;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceAccessHelper_iOS))]
namespace Eq.Utility.iOS
{
    public class DeviceAccessHelper_iOS : IDeviceAccessHelper
    {
        public bool ChangeScreenBrightness(object activityOrViewController, float brightness)
        {
            UIScreen.MainScreen.Brightness = brightness;
            return true;
        }

        public int GetScreenHeight()
        {
            return (int)UIScreen.MainScreen.Bounds.Height;
        }

        public int GetScreenWidth()
        {
            return (int)UIScreen.MainScreen.Bounds.Width;
        }
    }
}
