﻿using System;
using UnityEngine;

namespace PlayerInput
{
    // Written by Lukas Sacher/Camo
    public static class UserInput
    {
        static bool isSetup = false;

        static UserInputSettings inputSettings;

        // movement
        public static Action moveUp;
        public static Action moveDown;
        public static Action moveLeft;
        public static Action moveRight;

        // pickup
        public static Action pickUp;
        public static Action drop;

        // menus
        public static Action toggleInventory;

        // handy
        public static Action onInputStart;
        public static Action onInputCompleted;

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

            onInputStart?.Invoke();

            MovementInput();
            PickUpInput();
            MenuInput();

            onInputCompleted?.Invoke();
		}

        #region InputFuncs
        static void MovementInput()
		{
			if (Input.GetKey(inputSettings.up))
			{
                moveUp?.Invoke();
			}
            if (Input.GetKey(inputSettings.down))
            {
                moveDown?.Invoke();
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
