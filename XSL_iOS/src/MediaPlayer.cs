using AVFoundation;
using Eq.Utility;
using Foundation;
using System;
using System.IO;
using Twilio_ProgrammableVideo.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(MediaPlayerHelper_iOS))]
namespace Twilio_ProgrammableVideo.iOS
{
    public class MediaPlayerHelper_iOS : IMediaPlayerHelper
    {
        public IMediaPlayer Create()
        {
            return new MediaPlayer_iOS();
        }
    }

    public class MediaPlayer_iOS : IMediaPlayer
    {
        private AVAudioPlayer mAudioPlayer;
        private bool mLoop = false;
        private float mLeftVolume = 0.1f;
        private float mRightVolume = 0.1f;

        public void SetAudioStreamType(AudioStreamType streamType)
        {
            // 処理なし
        }

        public void SetLooping(bool loop)
        {
            mLoop = loop;
            if(mAudioPlayer != null)
            {
                mAudioPlayer.NumberOfLoops = loop ? nint.MaxValue : 0;
            }
        }

        public void Prepare(string assetName, HandlePrepare prepareHandler)
        {
            if(mAudioPlayer != null)
            {
                mAudioPlayer.Stop();
                mAudioPlayer.Dispose();
            }

            bool result = false;
            int extStartDotIndex = assetName.LastIndexOf('.');

            if (extStartDotIndex > 0)
            {
                //string fileName = Path.GetFileNameWithoutExtension(assetName);
                //string extName = Path.GetExtension(assetName);

                //NSDataAsset assetData = new NSDataAsset(fileName);
                //NSError error = null;

                //if (assetData != null)
                //{
                //    AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
                //    AVAudioSession.SharedInstance().SetActive(true);
                //    mAudioPlayer = new AVAudioPlayer(assetData.Data, extName, out error);
                //    SetLooping(mLoop);
                //    SetVolume(mLeftVolume, mRightVolume);
                //}

                string extName = Path.GetExtension(assetName);
                NSError error = null;
                AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
                AVAudioSession.SharedInstance().SetActive(true);
                mAudioPlayer = new AVAudioPlayer(new NSUrl(assetName), extName, out error);
                SetLooping(mLoop);
                SetVolume(mLeftVolume, mRightVolume);

                result = error == null;

                if (result)
                {
                    result = mAudioPlayer.PrepareToPlay();
                }
            }

            prepareHandler?.Invoke(assetName, result);
        }

        public void Start()
        {
            if(mAudioPlayer == null)
            {
                throw new NullReferenceException("mMediaPlayer == null. need to call Prepare");
            }

            mAudioPlayer.Play();
        }

        public void Pause()
        {
            if (mAudioPlayer == null)
            {
                throw new NullReferenceException("mMediaPlayer == null. need to call Prepare");
            }

            mAudioPlayer.Pause();
        }

        public void Stop()
        {
            if (mAudioPlayer == null)
            {
                throw new NullReferenceException("mMediaPlayer == null. need to call Prepare");
            }

            mAudioPlayer.Stop();
        }

        public void Release()
        {
            if (mAudioPlayer == null)
            {
                throw new NullReferenceException("mMediaPlayer == null. need to call Prepare");
            }

            mAudioPlayer.Stop();
            mAudioPlayer.Dispose();
        }

        public void SetVolume(float leftVolume, float rightVolume)
        {
            mLeftVolume = leftVolume;
            mRightVolume = rightVolume;
            if (mAudioPlayer != null)
            {
                mAudioPlayer.SetVolume(leftVolume, rightVolume);
            }
        }
    }
}
