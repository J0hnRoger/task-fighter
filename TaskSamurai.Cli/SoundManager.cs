using NAudio.Wave;

namespace TaskSamurai;

public class SoundManager
{
    private readonly string _soundLibraryPath;

    public SoundManager(string soundLibraryPath)
    {
        _soundLibraryPath = soundLibraryPath;
    }
    
    public void Play(string audioName)
    {
        using(var audioFile = new AudioFileReader(_soundLibraryPath + audioName + ".mp3"))
            using(var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(1000);
                }
            }
    }
}