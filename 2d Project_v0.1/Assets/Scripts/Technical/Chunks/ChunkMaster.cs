using UnityEngine;

namespace Chunks
{
    // Written by Lukas Sacher/Camo
    public class ChunkMaster : MonoBehaviour
    {
        public static ChunkMaster current = null;

        public GameObject player;
        [Space(10f)]
        public float loadingDistance;

        private void Awake()
        {
            if (current == null || current.gameObject == null)
            {
                current = this;
            }
            else
            {
                throw new System.Exception("Make sure there is only one game object with the ChunkMaster script attached!");
            }
        }
    }
}
