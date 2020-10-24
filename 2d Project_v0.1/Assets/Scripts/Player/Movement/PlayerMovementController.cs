using UnityEngine;
using PlayerInput;

namespace PlayerMovement
{
    //Written by LukasSacher / Camo
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController : MonoBehaviour
    {
        public float speed;
        Rigidbody2D rb;
        Vector2 moveInput;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            UserInput.moveUp += UpInput;
            UserInput.moveDown += DownInput;
            UserInput.moveLeft += LeftInput;
            UserInput.moveRight += RightInput;
            UserInput.onInputStart += ClearInput;
        }
        private void OnDestroy()
        {
            UserInput.moveUp -= UpInput;
            UserInput.moveDown -= DownInput;
            UserInput.moveLeft -= LeftInput;
            UserInput.moveRight -= RightInput;
            UserInput.onInputStart -= ClearInput;
        }

        private void FixedUpdate()
        {
            Move();
        }

        void ClearInput()
        {
            moveInput = Vector2.zero;
        }

        void UpInput()
        {
            moveInput.y = 1f;
        }
        void DownInput()
        {
            moveInput.y = -1f;
        }
        void LeftInput()
        {
            moveInput.x = -1f;
            transform.localScale = new Vector2(-1f, transform.localScale.y);
        }
        void RightInput()
        {
            moveInput.x = 1f;
            transform.localScale = new Vector2(1f, transform.localScale.y);
        }

        void Move()
        {
            moveInput.Normalize();
            rb.velocity = moveInput * speed * Time.deltaTime;
        }

        public Vector2 GetPlayerVelocity()
		{
            float x = Mathf.Clamp(rb.velocity.x, -1f, 1f);
            float y = Mathf.Clamp(rb.velocity.y, -1f, 1f);
            return new Vector2(x, y);
		}
    }
}