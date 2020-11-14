using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Printer
{
    public static void Print(object printing)
	{
#if UNITY_EDITOR
		Debug.Log(printing);
#endif
	}

	public static void Throw(object printing)
	{
#if UNITY_EDITOR
		Debug.LogError(printing);
#endif
	}

	public static void Warn(object printing)
	{
#if UNITY_EDITOR
		Debug.LogWarning(printing);
#endif
	}
}
