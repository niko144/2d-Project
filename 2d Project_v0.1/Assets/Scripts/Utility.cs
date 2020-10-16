using System;
using System.Collections.Generic;
using UnityEngine;

public interface IListCopyable
{
	IListCopyable Copy();
}

public static class Utility
{
    public static List<T> ToList<T> (this Array array) where T : IListCopyable
	{
		List <T> ret = new List<T>();

		for (int i = 0; i < array.Length; i++)
		{
			var val = (T)array.GetValue(i);
			ret.Add((T)val.Copy());
		}

		return ret;
	}
}
