using UnityEngine;

namespace PlayerInput
{
	// Written by Lukas Sacher/Camo
    public class UserInputSettings : MonoBehaviour
    {
        public static UserInputSettings current = null;

		public KeyCode up;
		public KeyCode down;
		public KeyCode left;
		public KeyCode right;
		[Space(10f)]
		public KeyCode pickUp;
		public KeyCode drop;
		[Space(10f)]
		public KeyCode toggleInventory;
		[Space(10f)]
		public KeyCode[] hotbarSlots;

		private void Awake()
		{
			if(current == null || current.gameObject == null)
			{
				current = this;
			}
			else
			{
				Printer.Throw("Make sure there is only one game object with the UserInputSettings script attached!");
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
