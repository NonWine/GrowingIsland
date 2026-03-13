using System;
using UnityEngine;

public sealed class WoodcutterDepositSession : IWoodcutterDepositSession
{
    private readonly WoodcutterView view;
    private readonly WoodCutterFacade woodCutterFacade;
    private readonly IWoodcutterDepositVisualController visualController;
    private readonly IWoodcutterDepositProjectileLauncher projectileLauncher;
    private readonly IWoodcutterDepositRoutine depositRoutine;

    private Coroutine depositCoroutine;
    private bool isActive;

    public WoodcutterDepositSession(
        WoodcutterView view, 
        WoodCutterFacade woodCutterFacade,
        IWoodcutterDepositVisualController visualController,
        IWoodcutterDepositProjectileLauncher projectileLauncher,
        IWoodcutterDepositRoutine depositRoutine)
    {
        this.view = view;
        this.woodCutterFacade = woodCutterFacade;
        this.visualController = visualController;
        this.projectileLauncher = projectileLauncher;
        this.depositRoutine = depositRoutine;
    }

    public void Start(Action<WoodcutterDepositRoutineResult> onCompleted)
    {
        Stop();

        isActive = true;
        bool startWithVariantB = UnityEngine.Random.value > 0.5f;

        view.Agent.isStopped = true;
        visualController.BeginSession(woodCutterFacade.HasWood);
        depositCoroutine = view.StartCoroutine(
            depositRoutine.Execute(
                startWithVariantB,
                () => isActive,
                result =>
                {
                    if (!isActive)
                        return;

                    onCompleted?.Invoke(result);
                }));
    }

    public void Stop()
    {
        isActive = false;

        if (depositCoroutine != null)
        {
            view.StopCoroutine(depositCoroutine);
            depositCoroutine = null;
        }

        projectileLauncher.ResetSession();
        visualController.EndSession();
    }
}
