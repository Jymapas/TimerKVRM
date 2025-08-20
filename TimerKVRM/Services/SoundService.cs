using System.Media;
using TimerKVRM.Properties;

namespace TimerKVRM.Services;

public sealed class SoundService : ISoundService
{
    private readonly SoundPlayer _break = new(Resources.bBreak);
    private readonly SoundPlayer _final = new(Resources.finialCountdown);
    private readonly SoundPlayer _start = new(Resources.start);
    private readonly SoundPlayer _ten = new(Resources.tenSeconds);

    public void PlayStart()
    {
        _start.Play();
    }

    public void PlayBreak()
    {
        _break.Play();
    }

    public void PlayTenSeconds()
    {
        _ten.Play();
    }

    public void PlayFinal()
    {
        _final.Play();
    }

    public void StopFinal()
    {
        _final.Stop();
    }
}