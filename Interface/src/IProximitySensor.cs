namespace Eq.Utility
{
    public delegate void SensorChanged(ProximityEvent proximityEvent);

    public interface IProximitySensor
    {
        void SetListener(SensorChanged listener, SensorNotifyRate sensorNotifyRate);

        void ResetListener(SensorChanged listener);
    }

    public class ProximityEvent
    {
        public SensorAccuracyType accuracy;

        public float distanceCM;
    }
}
