using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Add this interface to a class to prevent reference copys.
/// </summary>
public interface IListCopyable
{
	IListCopyable Copy();
}

/// <summary>
/// Utility functions everybody can use.
/// </summary>
public static class Utility
{
	public static List<T> ToList<T>(this Array array) where T : IListCopyable
	{
		List<T> ret = new List<T>();

		for (int i = 0; i < array.Length; i++)
		{
			var val = (T)array.GetValue(i);
			ret.Add((T)val.Copy());
		}

		return ret;
	}
	public static List<T> ToList<T>(this List<T> list) where T : IListCopyable
	{
		List<T> ret = new List<T>();

		for (int i = 0; i < list.Count; i++)
		{
			var val = list[i];
			ret.Add((T)val.Copy());
		}

		return ret;
	}

	/// <summary>
	/// returns 1 if the value is > 0 and -1 if the value is < 0. Otherwise 0.
	/// </summary>
	public static float GetAbsValue(this float target)
	{
		if (target > 0f) return 1f;
		if (target < 0f) return -1f;
		return 0f;
	}

	/// <summary>
	/// Returns all ui elements under the mouse.
	/// </summary>
	/// <returns></returns>
	public static List<RaycastResult> RaycastMouse()
	{

		PointerEventData pointerData = new PointerEventData(EventSystem.current)
		{
			pointerId = -1,
		};

		pointerData.position = Input.mousePosition;

		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerData, results);

		return results;
	}
}
