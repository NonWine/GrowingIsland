using System;

public interface ITreeDelayScheduler
{
    ITreeScheduledAction Schedule(float delaySeconds, Action callback);
}