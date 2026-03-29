using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace Extensions
{
	public static class ButtonExtension
	{
		public static void Set(this Button button, UnityAction action)
		{
			if (button != null && action != null)
			{
				button.onClick.AddListener(action);
			}
		}

		public static void Remove(this Button button, UnityAction action)
		{
			if (button != null && action != null)
			{
				button.onClick.RemoveListener(action);
			}
		}

		public static void AddListener(this EventTrigger trigger, EventTriggerType eventType, System.Action<PointerEventData> listener)
		{
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = eventType;
			entry.callback.AddListener(data => listener.Invoke((PointerEventData)data));
			trigger.triggers.Add(entry);
		}

        public static void AddListener(this EventTrigger eventTrigger, EventTriggerType triggerType,
         UnityAction<BaseEventData> call)
        {
            if (eventTrigger == null)
                throw new ArgumentNullException(nameof(eventTrigger));
            if (call == null)
                throw new ArgumentNullException(nameof(call));
            EventTrigger.Entry entry = eventTrigger.triggers.Find(e => e.eventID == triggerType);
            if (entry == null)
            {
                entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
                eventTrigger.triggers.Add(entry);
            }
            entry.callback.AddListener(call);
        }
        public static void RemoveListener(this EventTrigger eventTrigger, EventTriggerType triggerType,
            UnityAction<BaseEventData> call)
        {
            if (eventTrigger == null)
                throw new ArgumentNullException(nameof(eventTrigger));
            if (call == null)
                throw new ArgumentNullException(nameof(call));
            EventTrigger.Entry entry = eventTrigger.triggers.Find(e => e.eventID == triggerType);
            entry?.callback.RemoveListener(call);
        }
        public static void RemoveAllListeners(this EventTrigger eventTrigger, EventTriggerType triggerType)
        {
            if (eventTrigger == null)
                throw new ArgumentNullException(nameof(eventTrigger));
            EventTrigger.Entry entry = eventTrigger.triggers.Find(e => e.eventID == triggerType);
            entry?.callback.RemoveAllListeners();
        }

        public static void SimulateClick(this Button button)
        {
			ExecuteEvents.Execute(button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
			//using (var e = new NavigationSubmitEvent() { target = WButton })
			//	WButton.SendEvent(e);
		}

		public static void SimulateClick2(this Button button)
		{
			var go = button.gameObject;
			var ped = new PointerEventData(EventSystem.current);
			ExecuteEvents.Execute(go, ped, ExecuteEvents.pointerEnterHandler);
			ExecuteEvents.Execute(go, ped, ExecuteEvents.submitHandler);
		}

	}
}


