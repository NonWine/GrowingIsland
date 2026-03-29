using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions
{
	public static class FindingChildExtension
	{
		public static Transform FindDeepChild(this Transform parent, string name)
		{
			if (parent != null)
			{
				var result = parent.Find(name);
				if (result != null)
					return result;

				foreach (Transform child in parent)
				{
					result = child.FindDeepChild(name);
					if (result != null)
						return result;
				}
			}

			return null;
		}

		public static T FindDeepChild<T>(this Transform parent, string name)
		{
			T result = default;

			var transform = parent.FindDeepChild(name);

			if (transform != null)
			{
				result = (typeof(T) == typeof(GameObject)) ? (T)Convert.ChangeType(transform.gameObject, typeof(T)) : transform.GetComponent<T>();
			}

			if (result == null)
			{
				Debug.LogError($"FindDeepChild didn't find: '{name}' on GameObject: '{parent.name}'");
			}

			return result;
		}



		public static GameObject FindInChildren(this GameObject go, string name)
		{
			return (from x in go.GetComponentsInChildren<Transform>()
					where x.gameObject.name == name
					select x.gameObject).First();
		}

		public static GameObject FindInActiveObjectByName(string name)
		{
			Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
			for (int i = 0; i < objs.Length; i++)
			{
				if (objs[i].hideFlags == HideFlags.None)
				{
					if (objs[i].name == name)
					{
						return objs[i].gameObject;
					}
				}
			}
			return null;
		}

		public static GameObject FindInActiveObjectByTag(string tag)
		{

			Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
			for (int i = 0; i < objs.Length; i++)
			{
				if (objs[i].hideFlags == HideFlags.None)
				{
					if (objs[i].CompareTag(tag))
					{
						return objs[i].gameObject;
					}
				}
			}
			return null;
		}

		public static GameObject FindInActiveObjectByLayer(int layer)
		{

			Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
			for (int i = 0; i < objs.Length; i++)
			{
				if (objs[i].hideFlags == HideFlags.None)
				{
					if (objs[i].gameObject.layer == layer)
					{
						return objs[i].gameObject;
					}
				}
			}
			return null;
		}

		public static GameObject[] FindInActiveObjectsByName(string name)
		{
			List<GameObject> validTransforms = new List<GameObject>();
			Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
			for (int i = 0; i < objs.Length; i++)
			{
				if (objs[i].hideFlags == HideFlags.None)
				{
					if (objs[i].gameObject.name == name)
					{
						validTransforms.Add(objs[i].gameObject);
					}
				}
			}
			return validTransforms.ToArray();
		}

		public static GameObject[] FindInActiveObjectsByTag(string tag)
		{
			List<GameObject> validTransforms = new List<GameObject>();
			Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
			for (int i = 0; i < objs.Length; i++)
			{
				if (objs[i].hideFlags == HideFlags.None)
				{
					if (objs[i].gameObject.CompareTag(tag))
					{
						validTransforms.Add(objs[i].gameObject);
					}
				}
			}
			return validTransforms.ToArray();
		}

		public static GameObject[] FindInActiveObjectsByLayer(int layer)
		{
			List<GameObject> validTransforms = new List<GameObject>();
			Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
			for (int i = 0; i < objs.Length; i++)
			{
				if (objs[i].hideFlags == HideFlags.None)
				{
					if (objs[i].gameObject.layer == layer)
					{
						validTransforms.Add(objs[i].gameObject);
					}
				}
			}
			return validTransforms.ToArray();
		}
	}
}


