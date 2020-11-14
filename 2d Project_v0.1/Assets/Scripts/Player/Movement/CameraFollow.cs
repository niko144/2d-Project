using UnityEngine;

namespace PlayerMovement
{
    // Written by Lukas Sacher/Camo
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;
        [Space(5f)]
        public float smoothness;

        void Start()
        {
            // prevent small variables in the Inspector.
            smoothness *= .05f;
        }

        void Update()
        {
            if (target == null)
            {
                Printer.Warn("No target to the camera follow class assigned.");
                return;
            }

            Follow();
        }

        Vector3 vel;
        void Follow()
        {
            Vector3 targetPos = target.position + offset;

            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref vel, smoothness);
        }

        #region InspectorStuff
        private void OnValidate()
        {
            // smoothness can only be assigned with a value between 0 and 10
            if (smoothness < 0f) smoothness = 0f;
            else if (smoothness > 10f) smoothness = 10f;
        }
        #endregion
    }
}
