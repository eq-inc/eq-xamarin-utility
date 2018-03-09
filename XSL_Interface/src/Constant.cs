namespace Eq.Utility
{
    public enum SensorAccuracyType
    {
        NoContact = -1,
        //
        // 概要:
        //     The values returned by this sensor cannot be trusted, calibration is needed or
        //     the environment doesn't allow readings
        Unreliable = 0,
        //
        // 概要:
        //     This sensor is reporting data with low accuracy, calibration with the environment
        //     is needed
        AccuracyLow = 1,
        //
        // 概要:
        //     This sensor is reporting data with an average level of accuracy, calibration
        //     with the environment may improve the readings
        AccuracyMedium = 2,
        //
        // 概要:
        //     This sensor is reporting data with maximum accuracy
        AccuracyHigh = 3
    }

    public enum SensorNotifyRate
    {
        //
        // 概要:
        //     get sensor data as fast as possible
        Fastest = 0,
        //
        // 概要:
        //     rate suitable for games
        Game = 1,
        //
        // 概要:
        //     rate suitable for the user interface
        Ui = 2,
        //
        // 概要:
        //     rate (default) suitable for screen orientation changes
        Normal = 3
    }

    public enum AudioStreamType
    {
        //
        // 概要:
        //     Use this constant as the value for audioStreamType to request that the default
        //     stream type for notifications be used. Currently the default stream type is Android.Media.Stream.Notification.
        NotificationDefault = -1,
        //
        // 概要:
        //     The audio stream for phone calls
        VoiceCall = 0,
        //
        // 概要:
        //     The audio stream for system sounds
        System = 1,
        //
        // 概要:
        //     The audio stream for the phone ring
        Ring = 2,
        //
        // 概要:
        //     The audio stream for music playback
        Music = 3,
        //
        // 概要:
        //     The audio stream for alarms
        Alarm = 4,
        //
        // 概要:
        //     The audio stream for notifications
        Notification = 5,
        //
        // 概要:
        //     The audio stream for DTMF Tones
        Dtmf = 8
    }
}
