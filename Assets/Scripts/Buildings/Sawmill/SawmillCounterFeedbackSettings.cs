using System;
using UnityEngine;

[Serializable]
public class SawmillCounterFeedbackSettings
{
    public float ScalePunch = 0.16f;
    public float ScalePunchDuration = 0.16f;
    public Color FlashColor = new(1f, 0.94f, 0.74f, 1f);
    public float FlashDuration = 0.12f;

    public SawmillCounterFeedbackSettings() { }

    public SawmillCounterFeedbackSettings(SawmillCounterFeedbackSettings template)
    {
        ScalePunch = template.ScalePunch;
        ScalePunchDuration = template.ScalePunchDuration;
        FlashColor = template.FlashColor;
        FlashDuration = template.FlashDuration;
    }
}
