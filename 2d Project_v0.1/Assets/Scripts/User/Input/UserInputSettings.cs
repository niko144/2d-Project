using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserInput
{
	// Written by Lukas Sacher/Camo
    public class UserInputSettings : MonoBehaviour
    {
        public static UserInputSettings current = null;

		// movement
		public KeyCode up;
		public KeyCode down;
		public KeyCode left;
		public KeyCode right;
		[Space(10f)] // pickup
		public KeyCode pickUp;
		public KeyCode drop;
		[Space(10f)] // menus
		public KeyCode toggleInventory;

		private void Awake()
		{
			if(current == null || current.gameObject == null)
			{
				current = this;
			}
			else
			{
				throw new System.Exception("Make sure there is only one game object with the UserInputSettings script attached!");
			}
		}

		private void Start()
		{
			UserInput.Setup();
		}

		private void Update()
		{
			UserInput.Update();
		}
	}
}
