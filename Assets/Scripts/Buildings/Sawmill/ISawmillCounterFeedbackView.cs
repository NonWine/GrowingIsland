using TMPro;

public interface ISawmillCounterFeedbackView
{
    TMP_Text CurrentWoodText { get; }
    SawmillCounterFeedbackSettings CounterFeedbackSettings { get; }
}