using System;

public interface IWoodcutterDepositSession
{
    void Start(Action<WoodcutterDepositRoutineResult> onCompleted);
    void Stop();
}
