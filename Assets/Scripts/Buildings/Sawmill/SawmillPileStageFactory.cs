using System.Collections.Generic;
using UnityEngine;

public sealed class SawmillPileStageFactory
{
    private readonly SawmillPileLayoutCalculator layoutCalculator;
    private readonly SawmillPileVisualSettings settings;

    public SawmillPileStageFactory(SawmillPileLayoutCalculator layoutCalculator, SawmillPileVisualSettings settings)
    {
        this.layoutCalculator = layoutCalculator;
        this.settings = settings;
    }

    public List<Transform> CreateStages(SawmillView view, int stageCount)
    {
        var stages = new List<Transform>(Mathf.Max(0, stageCount));
        if (stageCount <= 0)
            return stages;

        GameObject pileTemplate = ResolvePileTemplate(view);
        if (pileTemplate == null)
            return stages;

        Transform pileRoot = view.PileRoot != null ? view.PileRoot : view.DepositPoint;
        if (pileRoot == null)
            return stages;

        for (int stageIndex = 0; stageIndex < stageCount; stageIndex++)
        {
            Transform stageRoot = new GameObject($"PileStage_{stageIndex + 1}").transform;
            stageRoot.SetParent(pileRoot, false);
            stageRoot.localPosition = layoutCalculator.GetStageLocalPosition(settings, stageIndex);
            stageRoot.localRotation = Quaternion.identity;
            stageRoot.localScale = Vector3.one;
            stageRoot.gameObject.SetActive(false);

            BuildStageLogs(stageRoot, pileTemplate, settings, stageIndex);
            stages.Add(stageRoot);
        }

        return stages;
    }

    private void BuildStageLogs(Transform stageRoot, GameObject pileTemplate, SawmillPileVisualSettings settings, int stageIndex)
    {
        int logCount = Mathf.Max(1, settings.LogsPerStage);
        for (int logIndex = 0; logIndex < logCount; logIndex++)
        {
            GameObject log = Object.Instantiate(pileTemplate, stageRoot);
            PrepareLog(log);

            Transform logTransform = log.transform;
            logTransform.localPosition = layoutCalculator.GetLogLocalPosition(settings, stageIndex, logIndex);
            logTransform.localRotation = layoutCalculator.GetLogLocalRotation(settings, stageIndex);
        }
    }

    private GameObject ResolvePileTemplate(SawmillView view)
    {
        if (settings.LogPrefab != null)
            return settings.LogPrefab;

        Transform depositPoint = view.DepositPoint;
        if (depositPoint.childCount > 0)
            return depositPoint.GetChild(0).gameObject;

        return null;
    }

    private static void PrepareLog(GameObject log)
    {
        log.SetActive(true);

        foreach (Collider collider in log.GetComponentsInChildren<Collider>(true))
            collider.enabled = false;

        foreach (Rigidbody rigidbody in log.GetComponentsInChildren<Rigidbody>(true))
            rigidbody.isKinematic = true;
    }
}
