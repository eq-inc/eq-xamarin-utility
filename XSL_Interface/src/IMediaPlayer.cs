namespace Eq.Utility
{
    public delegate void HandlePrepare(string url, bool prepared);

    public interface IMediaPlayerHelper
    {
        IMediaPlayer Create();
    }

    public interface IMediaPlayer
    {
        void SetAudioStreamType(AudioStreamType streamType);
        void SetLooping(bool loop);
        void Prepare(string assetName, HandlePrepare prepareHandler);
        void Start();
        void Pause();
        void Stop();
        void Release();
        void SetVolume(float leftVolume, float rightVolume);
    }
}
