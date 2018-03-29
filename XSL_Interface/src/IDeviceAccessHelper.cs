namespace Eq.Utility
{
    public interface IDeviceAccessHelper
    {
        bool ChangeScreenBrightness(object activityOrViewController, float brightness);

        int GetScreenWidth();

        int GetScreenHeight();
    }
}
