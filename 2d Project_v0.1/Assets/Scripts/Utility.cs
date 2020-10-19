using System;
using System.Collections.Generic;

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
}
