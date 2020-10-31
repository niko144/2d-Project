using UnityEngine;
using NPC.Drop;

namespace NPCLootMono
{
    public class NPCLoot : MonoBehaviour
    {
		public DropData dropData;
        public Vector2 selfPosition;

        void Start()
        {
            selfPosition.x = transform.position.x;
            selfPosition.y = transform.position.y;
            dropData.AdjustChooseChances();
        }
        
	}
}