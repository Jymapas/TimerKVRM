using System.Media;
using TimerKVRM.Properties;

namespace TimerKVRM.Services;

public sealed class SoundService : ISoundService
{
    private readonly SoundPlayer _break = new(Resources.bBreak);
    private readonly SoundPlayer _final = new(Resources.finialCountdown);
    private readonly SoundPlayer _start = new(Resources.start);
    private readonly SoundPlayer _ten = new(Resources.tenSeconds);
    private readonly SoundPlayer _warning = new(Resources.finalWarning);

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

    public void PlayFinalWarning()
    {
        _warning.Play();
    }

    public void StopAll()
    {
        _final.Stop();
        _break.Stop();
        _start.Stop();
        _ten.Stop();
        _warning.Stop();
    }
}