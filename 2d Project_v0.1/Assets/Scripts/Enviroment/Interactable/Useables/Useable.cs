using UnityEngine;
using Chunks;

namespace Useable
{
    // Written by Lukas Sacher/Camo
    public class Useable : MonoBehaviour
    {
		#region Info

        /* This class is only a base class.
         * For more complex useables you can make a custom class in the Useable.Custom namespace.
         * Those can override the start- and end interact functions.
         */

		#endregion

		public UseableData data;
        GameObject player = null;

        float timer = 0f;

		private void Start()
		{
            player = ChunkMaster.current.player;
            ResetTimer();
		}

		private void Update()
		{
            if(Vector2.Distance(transform.position, player.transform.position) > data.requiredDistance)
			{
                return;
			}

            Behaviour();

            if(timer <= 0f)
			{
                OnEndInteract();
			}
		}

        void Behaviour()
		{
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                // mouse is above this object.
                if (hit.collider != null && hit.collider.transform == transform)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        OnStartInteract();
                    }

                    Use();
                }
                else ResetTimer();
            }
            else ResetTimer();
        }

        void Use()
		{
            timer -= Time.deltaTime;
        }
		void ResetTimer()
		{
            timer = data.interactionTime;
        }

        public virtual void OnStartInteract()
        {

        }
        public virtual void OnEndInteract()
		{
            Destroy(gameObject);
		}
	}
}
