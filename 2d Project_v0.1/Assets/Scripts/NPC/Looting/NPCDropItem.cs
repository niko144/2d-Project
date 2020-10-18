using System.Collections;
using UnityEngine;
using NPC.Drop.Items;

namespace NPC.Drop
{
    // Written by Prem
    public class NPCDropItem : MonoBehaviour
    {
        private NPCMaster npcMaster;
        public GameObject[] itemToDrop;


        void OnEnable()
        {
            SetInitialReferences();
            npcMaster.EventNPCDie += DropItems;
        }
        void OnDisable()
        {
            npcMaster.EventNPCDie -= DropItems;
        }


        void SetInitialReferences()
        {
            npcMaster = GetComponent<NPCMaster>();
        }

        void DropItems()
        {
            if (itemToDrop.Length > 0)
            {
                foreach (GameObject item in itemToDrop)
                {
                    StartCoroutine(PauseBeforeDrop(item)); //otherwise the event gets fired before the start
                }
            }
        }


        IEnumerator PauseBeforeDrop(GameObject itemToDrop)
        {
            yield return new WaitForSeconds(0.05f);
            itemToDrop.SetActive(true);
            itemToDrop.transform.SetParent(null);
            yield return new WaitForSeconds(0.05f);
            if (itemToDrop.GetComponent<ItemMaster>() != null)
            {
                itemToDrop.GetComponent<ItemMaster>().CallEventObjectThrow();
            }
        }
    }
}