using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public static class CorutineExtensions
{
	public static Coroutine WaitSecond(this MonoBehaviour obj, float seconds, Action action)
	{
		if (obj.gameObject.activeSelf)
			return obj.StartCoroutine(Timer(seconds, action));
		else
			return null;
	}

	public static Coroutine WaitRealSecond(this MonoBehaviour obj, float seconds, Action action)
	{
		if (obj.gameObject.activeSelf)
			return obj.StartCoroutine(TimerReal(seconds, action));
		else
			return null;
	}

	public static Coroutine UpdateCoroutine(this MonoBehaviour obj, Action action)
	{
		if (obj.gameObject.activeSelf)
			return obj.StartCoroutine(UpdateCoroutines(action));
		else
			return null;
	}

	public static void StopAll(this MonoBehaviour obj)
	{
		obj.StopAllCoroutines();
	}

	public static Coroutine WaitFrame(this MonoBehaviour obj, Action action)
	{
		if (obj.gameObject.activeSelf)
			return obj.StartCoroutine(Timer(action));
		else
			return null;
	}
	public static Coroutine WaitFrameFixed(this MonoBehaviour obj, Action action)
	{
		if (obj.gameObject.activeSelf)
			return obj.StartCoroutine(TimerFixed(action));
		else
			return null;
	}

	public static Coroutine WaitUntillScript(this MonoBehaviour obj, Func<bool> func, Action action)
	{
		if (obj.gameObject.activeSelf)
			return obj.StartCoroutine(UntilCoroutines(func, action));
		else
			return null;
	}

	public static Coroutine WaitUntillScript(this MonoBehaviour obj, bool func, Action action)
	{
		if (obj.gameObject.activeSelf)
			return obj.StartCoroutine(UntilCoroutines(() => func, action));
		else
			return null;
	}


	public static Coroutine WaitWhileScript(this MonoBehaviour obj, Func<bool> boll, Action afterAction = null, Action actionWhile = null)
	{
		if (obj.gameObject.activeSelf)
			return obj.StartCoroutine(WaitWhile(boll, afterAction, actionWhile));
		else
			return null;
	}



	// Timers 
	private static IEnumerator Timer(Action action)
	{
		yield return new WaitForFixedUpdate();
		action?.Invoke();
	}

	private static IEnumerator TimerFixed(Action action)
	{
		yield return new WaitForEndOfFrame();
		action?.Invoke();
	}
	private static IEnumerator Timer(float timer, Action action)
	{
		yield return new WaitForSeconds(timer);
		action?.Invoke();
	}

	private static IEnumerator TimerReal(float timer, Action action)
	{
		yield return new WaitForSecondsRealtime(timer);
		action?.Invoke();
	}

	private static IEnumerator UpdateCoroutines(Action action)
	{
		while (true)
		{
			action?.Invoke();
			yield return null;
		}
	}

	private static IEnumerator UntilCoroutines(Func<bool> boll, Action action)
	{
		yield return WaitUntil(boll);
		action?.Invoke();
	}

	private static IEnumerator WaitWhile(Func<bool> boll, Action afterAction = null, Action actionWhile = null)
	{
		while (boll.Invoke() == true)
		{
			actionWhile?.Invoke();
			yield return new WaitForEndOfFrame();
		}
		afterAction?.Invoke();
		Debug.Log("EndWhile");
	}


	//async wait
	public static async Task WaitWhile(Func<bool> condition, int frequency = 25, int timeout = -1)
	{
		var waitTask = Task.Run(async () =>
		{
			while (condition()) await Task.Delay(frequency);
		});

		if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)))
			throw new TimeoutException();
	}

	public static async Task WaitUntil(Func<bool> condition, int frequency = 25, int timeout = -1) // await WaitUntil(() => s.Dequeue() == "bar");
	{
		var waitTask = Task.Run(async () =>
		{
			while (!condition()) await Task.Delay(frequency);
		});

		if (waitTask != await Task.WhenAny(waitTask,
				Task.Delay(timeout)))
			throw new TimeoutException();
	}
}
