using UnityEngine;
using UnityEngine.Events;

public class AnimationEventsView : MonoBehaviour
{
  [SerializeField]  public  UnityEvent OnDamageTriger;

    public void InvokeDamageTrigger() => OnDamageTriger?.Invoke();
}