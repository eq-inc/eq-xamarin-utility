using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Display;
using Android.Views;
using Eq.Utility.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceAccessHelper_Android))]
namespace Eq.Utility.Droid
{
    public class DeviceAccessHelper_Android : IDeviceAccessHelper
    {
        public bool ChangeScreenBrightness(object activityOrViewController, float brightness)
        {
            bool ret = false;
            Activity activity = activityOrViewController as Activity;

            if (activity != null)
            {
                Window window = activity.Window;
                WindowManagerLayoutParams layoutParams = window.Attributes;
                layoutParams.ScreenBrightness = brightness;
                window.Attributes = layoutParams;

                ret = true;
            }

            return ret;
        }

        public int GetScreenHeight()
        {
            Point screenSize = new Point();
            ((DisplayManager)Application.Context.GetSystemService(Context.DisplayService)).GetDisplay(Display.DefaultDisplay).GetRealSize(screenSize);
            return screenSize.Y;
        }

        public int GetScreenWidth()
        {
            Point screenSize = new Point();
            ((DisplayManager)Application.Context.GetSystemService(Context.DisplayService)).GetDisplay(Display.DefaultDisplay).GetRealSize(screenSize);
            return screenSize.X;
        }
    }
}
