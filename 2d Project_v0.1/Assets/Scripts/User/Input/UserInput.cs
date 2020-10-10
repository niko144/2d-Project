using System;
using UnityEngine;

namespace UserInput
{
    // Written by Lukas Sacher/Camo
    public static class UserInput
    {
        static bool isSetup = false;

        static UserInputSettings inputSettings;

        // movement
        public static Action moveForward;
        public static Action moveBackwards;
        public static Action moveLeft;
        public static Action moveRight;

        // pickup
        public static Action pickUp;
        public static Action drop;

        // menus
        public static Action toggleInventory;

        public static void Setup()
		{
            inputSettings = UserInputSettings.current;
            isSetup = true;
		}

        public static void Update()
		{
			if (!isSetup)
			{
                throw new Exception("The UserInput is not setup! Make sure there is a game object with the UserInputSettings script");
			}

            MovementInput();
            PickUpInput();
            MenuInput();
		}

		#region InputFuncs
		static void MovementInput()
		{
			if (Input.GetKey(inputSettings.forward))
			{
                moveForward?.Invoke();
			}
            if (Input.GetKey(inputSettings.backwards))
            {
                moveBackwards?.Invoke();
            }
            if (Input.GetKey(inputSettings.left))
            {
                moveLeft?.Invoke();
            }
            if (Input.GetKey(inputSettings.right))
            {
                moveRight?.Invoke();
            }
        }
        static void PickUpInput()
		{
            if (Input.GetKeyDown(inputSettings.pickUp))
            {
                pickUp?.Invoke();
            }
            if (Input.GetKeyDown(inputSettings.drop))
            {
                drop?.Invoke();
            }
        }
        static void MenuInput()
		{
            if (Input.GetKeyDown(inputSettings.toggleInventory))
            {
                toggleInventory?.Invoke();
            }
        }
		#endregion
	}
}
