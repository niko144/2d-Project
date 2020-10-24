using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerMovement;

namespace Animations.Player
{

	// Written by Lukas Sacher / Camo
	public class PlayerAnimationController : MonoBehaviour
	{
		public PlayerMovementController movement;
		public Animator torsoAnim;
		public Animator legsAnim;

		Vector2 lastPlayerVel;

		private void Update()
		{
			Vector2 vel = movement.GetPlayerVelocity();

			if (Mathf.Abs(vel.x) < 0.05f && Mathf.Abs(vel.y) < 0.05f)
			{
				torsoAnim.SetBool("IsMoving", false);
				torsoAnim.SetFloat("LastHorizontal", Mathf.Abs(lastPlayerVel.x));
				torsoAnim.SetFloat("LastVertical", lastPlayerVel.y);

				legsAnim.SetBool("IsMoving", false);
				legsAnim.SetFloat("LastHorizontal", Mathf.Abs(lastPlayerVel.x));
				legsAnim.SetFloat("LastVertical", lastPlayerVel.y);
			}
			else
			{
				// Prevent weird bug when walking up diagonally.
				if (Mathf.Abs(vel.x) == vel.y)
				{
					vel.x = 0f;
				}

				torsoAnim.SetFloat("Horizontal", Mathf.Abs(vel.x));
				torsoAnim.SetFloat("Vertical", vel.y);
				torsoAnim.SetBool("IsMoving", true);

				legsAnim.SetFloat("Horizontal", Mathf.Abs(vel.x));
				legsAnim.SetFloat("Vertical", vel.y);
				legsAnim.SetBool("IsMoving", true);

				lastPlayerVel = vel;
			}
		}
	}
}