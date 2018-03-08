using Android.Content.Res;
using System;
using Xamarin.Forms;
using Eq.Utility.Droid;

[assembly: Dependency(typeof(MediaPlayer_Android))]
namespace Eq.Utility.Droid
{
    public class MediaPlayer_Android : IMediaPlayer
    {
        private Android.Media.MediaPlayer mMediaPlayer;
        private AudioStreamType mAudioStreamType = AudioStreamType.Music;
        private bool mLoop = false;
        private float mLeftVolume = 0.1f;
        private float mRightVolume = 0.1f;

        public void SetAudioStreamType(AudioStreamType streamType)
        {
            mAudioStreamType = streamType;
            if (mMediaPlayer != null)
            {
                mMediaPlayer.SetAudioStreamType(ExchangeAudioStreamType(streamType));
            }
        }

        public void SetLooping(bool loop)
        {
            mLoop = loop;
            if (mMediaPlayer != null)
            {
                mMediaPlayer.Looping = loop;
            }
        }

        public void Prepare(string assetName, HandlePrepare prepareHandler)
        {
            if (mMediaPlayer == null)
            {
                mMediaPlayer = new Android.Media.MediaPlayer();
            }

            AssetFileDescriptor assetFd = Android.App.Application.Context.Assets.OpenFd(assetName);
            mMediaPlayer.Reset();
            mMediaPlayer.SetDataSource(assetFd);
            mMediaPlayer.SetAudioStreamType(ExchangeAudioStreamType(mAudioStreamType));
            mMediaPlayer.Looping = mLoop;
            mMediaPlayer.SetOnPreparedListener(new LocalPrepareListener(prepareHandler));
        }

        public void Start()
        {
            if(mMediaPlayer == null)
            {
                throw new NullReferenceException("mMediaPlayer == null. need to call Prepare");
            }

            mMediaPlayer.Start();
        }

        public void Pause()
        {
            if (mMediaPlayer == null)
            {
                throw new NullReferenceException("mMediaPlayer == null. need to call Prepare");
            }

            mMediaPlayer.Pause();
        }

        public void Stop()
        {
            if (mMediaPlayer == null)
            {
                throw new NullReferenceException("mMediaPlayer == null. need to call Prepare");
            }

            mMediaPlayer.Stop();
        }

        public void Release()
        {
            if (mMediaPlayer == null)
            {
                throw new NullReferenceException("mMediaPlayer == null. need to call Prepare");
            }

            mMediaPlayer.Release();
        }

        public void SetVolume(float leftVolume, float rightVolume)
        {
            mLeftVolume = leftVolume;
            mRightVolume = rightVolume;
            if (mMediaPlayer != null)
            {
                mMediaPlayer.SetVolume(leftVolume, rightVolume);
            }
        }

        private Android.Media.Stream ExchangeAudioStreamType(AudioStreamType value)
        {
            Android.Media.Stream ret = Android.Media.Stream.Music;

            switch (value)
            {
                case AudioStreamType.NotificationDefault:
                    ret = Android.Media.Stream.NotificationDefault;
                    break;
                case AudioStreamType.VoiceCall:
                    ret = Android.Media.Stream.VoiceCall;
                    break;
                case AudioStreamType.System:
                    ret = Android.Media.Stream.System;
                    break;
                case AudioStreamType.Ring:
                    ret = Android.Media.Stream.Ring;
                    break;
                case AudioStreamType.Music:
                    ret = Android.Media.Stream.Music;
                    break;
                case AudioStreamType.Alarm:
                    ret = Android.Media.Stream.Alarm;
                    break;
                case AudioStreamType.Notification:
                    ret = Android.Media.Stream.Notification;
                    break;
                case AudioStreamType.Dtmf:
                    ret = Android.Media.Stream.Dtmf;
                    break;
            }

            return ret;
        }

        class LocalPrepareListener : Java.Lang.Object, Android.Media.MediaPlayer.IOnPreparedListener
        {
            private HandlePrepare mPrepareHandler;

            public LocalPrepareListener(HandlePrepare prepareHandler)
            {
                mPrepareHandler = prepareHandler ?? throw new NullReferenceException("prepareHandler == null");
            }

            public void OnPrepared(Android.Media.MediaPlayer mp)
            {
                throw new NotImplementedException();
            }
        }
    }
}