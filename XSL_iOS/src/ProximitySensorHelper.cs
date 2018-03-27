using Eq.Utility.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ProximitySensorHelper_iOS))]
namespace Eq.Utility.iOS
{
    public class ProximitySensorHelper_iOS : IProximitySensor
    {
        private LocalSensorListener mListener = null;

        public void SetListener(SensorChanged listener, SensorNotifyRate sensorNotifyRate)
        {
            mListener = new LocalSensorListener(listener);

            UIDevice.CurrentDevice.ProximityMonitoringEnabled = true;
            NSNotificationCenter.DefaultCenter.AddObserver(new NSString(), mListener.Listen);
        }

        public void ResetListener(SensorChanged listener)
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver(mListener);
        }

        private class LocalSensorListener : NSObject
        {
            private SensorChanged mSensorChangeListener;

            public LocalSensorListener(SensorChanged sensorChangeListener)
            {
                mSensorChangeListener = sensorChangeListener;
            }

            public void Listen(NSNotification notification)
            {
                ProximityEvent proximityEvent = new ProximityEvent();

                if (UIDevice.CurrentDevice.ProximityState == true)
                {
                    proximityEvent.accuracy = SensorAccuracyType.AccuracyMedium;
                    proximityEvent.distanceCM = 0;
                }
                else
                {
                    proximityEvent.accuracy = SensorAccuracyType.AccuracyMedium;
                    proximityEvent.distanceCM = 10;
                }

                mSensorChangeListener(proximityEvent);
            }
        }
    }
}
