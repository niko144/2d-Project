using UnityEngine;

namespace NPC.Drop.Items
{
    // Written by Prem
    public class ItemMaster : MonoBehaviour
    {
        // if access to player 
        //private Player player;

        public delegate void GeneralEventHandler();
        public event GeneralEventHandler EventObjectThrow;

        public void CallEventObjectThrow()
        {
            EventObjectThrow?.Invoke();
        }
    }
}