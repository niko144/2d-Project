using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Drop.Items
{
    // Written by Prem
    public class ItemThrow : MonoBehaviour
    {
        private ItemMaster itemMaster;
        private Rigidbody2D rb;
        private Vector2 throwDirection;
    

        public bool canBeThrown;
        public float throwForce;


        void Start()
        {
            SetInitialReferences();
        }


        void SetInitialReferences()
        {
            itemMaster = GetComponent<ItemMaster>();
            rb = GetComponent<Rigidbody2D>();
        }

        void CarryOutThrowActions()
        {

        }

        void HurlItem()
        {
            rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        }
    }
}
