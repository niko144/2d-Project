using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Can be edited by anyone, but remember to talk to others so we won't get merge issues.
[DefaultExecutionOrder(-1000)]
public class GameManager : MonoBehaviour
{
	public static GameManager current;

	GameObject localPlayer = null;

	public GameObject LocalPlayer
	{
		get
		{
			if (localPlayer == null) InitializeLocalPlayer();

			return localPlayer;
		}
		private set { }
	}

	public void Awake()
	{
		if (current == null || current.gameObject == null)
		{
			current = this;
		}
		else
		{
			Printer.Throw($"Make sure there is only one '{this.GetType().Name}' in the scene.");
		}
	}

	// Would have to be updated with multiplayer
	void InitializeLocalPlayer()
	{
		localPlayer = GameObject.FindGameObjectWithTag("Player");
	}

	public T GetComponentFromLocalPlayer<T>() where T : Component
	{
		if (LocalPlayer == null) return null;

		return LocalPlayer.GetComponent<T>();
	}
}
