using System.Collections.Generic;
using UnityEngine;

public interface ISawmillImpactFeedbackView
{
    Transform DepositPoint { get; }
    Transform ImpactRoot { get; }
    AudioSource AudioSource { get; }
}

public interface ISawmillPileVisualTarget
{
    Transform DepositPoint { get; }
    Transform PileRoot { get; }
}

public interface ISawmillStorageFeedback
{
    void PlayStorageChanged();
}

public interface ISawmillImpactFeedback
{
    void Play(float impactStrength);
}

public interface ISawmillImpactAnimator
{
    void Play(float impactStrength);
}

public interface ISawmillImpactAudioPlayer
{
    void Play();
}

public interface ISawmillImpactVfxPlayer
{
    void Play();
}

public interface ISawmillPileVisualizer
{
    void RenderStorage(int current, int capacity, bool animateStageChange);
    void PlayImpact(float impactStrength);
}

public interface ISawmillPileLayoutCalculator
{
    int GetStageCount(SawmillPileVisualSettings settings, int capacity);
    int GetVisibleStageCount(SawmillPileVisualSettings settings, int current, int capacity, int availableStageCount);
    Vector3 GetStageLocalPosition(SawmillPileVisualSettings settings, int stageIndex);
    Vector3 GetLogLocalPosition(SawmillPileVisualSettings settings, int stageIndex, int logIndex);
    Quaternion GetLogLocalRotation(SawmillPileVisualSettings settings, int stageIndex);
}

public interface ISawmillPileStageFactory
{
    List<Transform> CreateStages(ISawmillPileVisualTarget view, int stageCount);
}

public interface ISawmillPileAnimator
{
    void ApplyVisibility(IReadOnlyList<Transform> stageRoots, SawmillPileVisualSettings settings, int visibleStages, bool animateStageChange);
    void PlayImpact(IReadOnlyList<Transform> stageRoots, SawmillImpactFeedbackSettings impactFeedback, float impactStrength);
}
