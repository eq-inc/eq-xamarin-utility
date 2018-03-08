using Android.Hardware;
using Android.Runtime;
using Eq.Utility;
using Eq.Utility.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(ProximitySensorHelper_Android))]
namespace Eq.Utility.Droid
{
    public class ProximitySensorHelper_Android : IProximitySensor
    {
        private LocalSensorListener mListener = null;

        public void SetListener(SensorChanged listener, SensorNotifyRate sensorNotifyRate)
        {
            SensorManager sensorManager = (SensorManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.SensorService);
            Sensor promximitySensor = sensorManager.GetDefaultSensor(SensorType.Proximity);

            if (mListener != null)
            {
                sensorManager.UnregisterListener(mListener);
            }

            sensorManager.RegisterListener(mListener = new LocalSensorListener(listener), promximitySensor, SensorDelay.Normal);
        }

        public void ResetListener(SensorChanged listener)
        {
            if(mListener != null)
            {
                SensorManager sensorManager = (SensorManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.SensorService);
                sensorManager.UnregisterListener(mListener);

                mListener = null;
            }
        }

        private class LocalSensorListener : Java.Lang.Object, ISensorEventListener
        {
            private SensorChanged mListener = null;

            public LocalSensorListener(SensorChanged listener)
            {
                mListener = listener ?? throw new System.ArgumentNullException("listener == null");
            }

            public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
            {
            }

            public void OnFlushCompleted(Sensor sensor)
            {
            }

            public void OnSensorChanged(SensorEvent e)
            {
                ProximityEvent proximityEvent = new ProximityEvent();
                proximityEvent.accuracy = ProximitySensorHelper_Android.ExchangeAccuracy(e.Accuracy);
                proximityEvent.distanceCM = e.Values[0];

                mListener(proximityEvent);
            }
        }

        internal static SensorAccuracyType ExchangeAccuracy(SensorStatus sensorStatus)
        {
            SensorAccuracyType ret = SensorAccuracyType.Unreliable;

            switch (sensorStatus)
            {
                case SensorStatus.NoContact:
                    ret = SensorAccuracyType.NoContact;
                    break;
                case SensorStatus.Unreliable:
                    ret = SensorAccuracyType.Unreliable;
                    break;
                case SensorStatus.AccuracyLow:
                    ret = SensorAccuracyType.AccuracyLow;
                    break;
                case SensorStatus.AccuracyMedium:
                    ret = SensorAccuracyType.AccuracyMedium;
                    break;
                case SensorStatus.AccuracyHigh:
                    ret = SensorAccuracyType.AccuracyHigh;
                    break;
            }

            return ret;
        }

        internal static SensorNotifyRate ExchangeNotifyRate(SensorDelay sensorDelay)
        {
            SensorNotifyRate ret = SensorNotifyRate.Normal;

            switch (sensorDelay)
            {
                case SensorDelay.Fastest:
                    ret = SensorNotifyRate.Fastest;
                    break;
                case SensorDelay.Game:
                    ret = SensorNotifyRate.Game;
                    break;
                case SensorDelay.Ui:
                    ret = SensorNotifyRate.Ui;
                    break;
                case SensorDelay.Normal:
                    ret = SensorNotifyRate.Normal;
                    break;
            }

            return ret;
        }
    }
}
