using System.Media;

namespace TimerKVRM.Services;

public sealed class SoundService : ISoundService
{
    private readonly SoundPlayer _final = new(Properties.Resources.finialCountdown);
    private readonly SoundPlayer _start = new(Properties.Resources.start);
    private readonly SoundPlayer _break = new(Properties.Resources.bBreak);
    private readonly SoundPlayer _ten = new(Properties.Resources.tenSeconds);

    public void PlayStart() => _start.Play();
    public void PlayBreak() => _break.Play();
    public void PlayTenSeconds() => _ten.Play();
    public void PlayFinal() => _final.Play();
    public void StopFinal() => _final.Stop();
}