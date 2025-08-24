namespace TimerKVRM.Services;

public interface ISoundService
{
    void PlayStart();
    void PlayBreak();
    void PlayTenSeconds();
    void PlayFinal();
    void PlayFinalWarning();
    void StopAll();
}