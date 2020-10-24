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
		[Space(12f)]
		public float shootAnimLength = .8f;
		float shootAnimTimer = 0f;

		Vector2 lastPlayerVel;
		Vector2 startTorsoScale;

		bool isMoving;

		private void Start()
		{
			startTorsoScale = torsoAnim.transform.localScale;
		}

		Vector2 lastMousePos = Vector2.one;

		private void Update()
		{
			MovementAnimation();
			ShootingAnimations();
		}

		private void LateUpdate()
		{
			LockOrientation();
		}

		#region MovementAnims
		void MovementAnimation()
		{
			Vector2 vel = movement.GetPlayerVelocity();

			if (Mathf.Abs(vel.x) < 0.05f && Mathf.Abs(vel.y) < 0.05f)
			{
				NotMoving();
			}
			else
			{
				Moving(vel);
			}
		}

		void Moving(Vector2 playerVelocity)
		{
			if (!isMoving)
			{
				StartMoving();
				isMoving = true;
			}

			// Prevent weird bug when walking up diagonally.
			if (Mathf.Abs(playerVelocity.x) == playerVelocity.y)
			{
				playerVelocity.x = 0f;
			}

			SetMoveValues(playerVelocity);

			lastPlayerVel = playerVelocity;
		}
		void NotMoving()
		{
			if (isMoving)
			{
				StopMoving();
				isMoving = false;
			}
			SetLastMoveValues();
		}

		void StartMoving()
		{
			legsAnim.SetBool("IsMoving", true);
			torsoAnim.SetBool("IsMoving", true);
		}
		void StopMoving()
		{
			legsAnim.SetBool("IsMoving", false);
			torsoAnim.SetBool("IsMoving", false);
		}
		void SetMoveValues(Vector2 playerVelocity)
		{
			torsoAnim.SetFloat("Horizontal", Mathf.Abs(playerVelocity.x));
			torsoAnim.SetFloat("Vertical", playerVelocity.y);

			legsAnim.SetFloat("Horizontal", Mathf.Abs(playerVelocity.x));
			legsAnim.SetFloat("Vertical", playerVelocity.y);
		}
		void SetLastMoveValues()
		{
			torsoAnim.SetFloat("LastHorizontal", Mathf.Abs(lastPlayerVel.x));
			torsoAnim.SetFloat("LastVertical", lastPlayerVel.y);

			legsAnim.SetFloat("LastHorizontal", Mathf.Abs(lastPlayerVel.x));
			legsAnim.SetFloat("LastVertical", lastPlayerVel.y);
		}
		#endregion

		#region ShootingAnims
		void ShootingAnimations()
		{
			if (shootAnimTimer > 0f)
			{
				shootAnimTimer -= Time.deltaTime;
			}
			else
			{
				SetDefaulAnimState();
			}

			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				lastMousePos = (Vector2)Input.mousePosition - new Vector2(Screen.width, Screen.height) * 0.5f;
				SetTorsoAnimationValues();
			}
		}

		void LockOrientation()
		{
			if (shootAnimTimer <= 0f)
			{
				return;
			}

			if (movement.transform.localScale.x.GetAbsValue() != lastMousePos.x.GetAbsValue())
			{
				// Prevent setting the scale every frame
				Vector2 targetScale = new Vector2(-Mathf.Abs(startTorsoScale.x), startTorsoScale.y);
				if (torsoAnim.transform.localScale == (Vector3)targetScale) return;

				torsoAnim.transform.localScale = targetScale;
			}
			else
			{
				// Prevent setting the scale every frame
				if (torsoAnim.transform.localScale == (Vector3)startTorsoScale) return;

				torsoAnim.transform.localScale = startTorsoScale;
			}
		}
		void SetTorsoAnimationValues()
		{
			float centerOriginMouseX = Mathf.Abs(Input.mousePosition.x - Screen.width * 0.5f) / Screen.width * 2f;
			float centerOriginMouseY = (Input.mousePosition.y - Screen.height * 0.5f) / Screen.height * 2f;

			shootAnimTimer = shootAnimLength;
			torsoAnim.SetFloat("MouseX", centerOriginMouseX);
			torsoAnim.SetFloat("MouseY", centerOriginMouseY);

			torsoAnim.SetFloat("ShootTimer", 1f);
		}

		void SetDefaulAnimState()
		{
			torsoAnim.transform.localScale = startTorsoScale;
			torsoAnim.SetFloat("ShootTimer", -1f);
		}
		#endregion
	}
}