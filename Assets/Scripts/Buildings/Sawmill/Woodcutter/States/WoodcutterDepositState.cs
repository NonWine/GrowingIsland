using System.Collections;
using Zenject;

public class WoodcutterDepositState : WoodcutterState
{
    [Inject] private IWoodcutterDepositSession depositSession;

    public override void Enter()
    {
        depositSession.Start(OnDepositRoutineCompleted);
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
        depositSession.Stop();
    }

    private void OnDepositRoutineCompleted(WoodcutterDepositRoutineResult result)
    {
        switch (result)
        {
            case WoodcutterDepositRoutineResult.WaitForStorage:
                ChangeState<WoodcutterWaitingState>();
                break;
            case WoodcutterDepositRoutineResult.ContinueSearching:
                ChangeState<WoodcutterSearchTreeState>();
                break;
        }
    }
}
