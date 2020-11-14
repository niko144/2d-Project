using UnityEngine;

namespace GameItems.Drops
{
    //Written by Muneeb UR Rahman
    [CreateAssetMenu(fileName = "NewDropData", menuName = "NPCDropData")]
    public class DropData : ScriptableObject
    {
        public Loot[] possibleDrops;
        [HideInInspector] public DropData[] drops;
        private float randomNumber;
        private void OnValidate()
        {
            CheckChoosingChances();
        }

        public void CheckChoosingChances()
        {
            float totalChance = 0f;

            foreach (Loot loot in possibleDrops)
            {
                totalChance += loot.chooseChance;
            }
            if (totalChance != 1f)
            {
                Debug.LogWarning($"The choosing chances for the drop '{name}' don't result in 100%.");
            }

        }
        public void AdjustChooseChances()
        {
            Vector2 npcPosition = GameObject.FindObjectOfType<NPCLoot>().selfPosition;
            foreach (DropData d in drops)
            {
                for (int i = 1; i < d.possibleDrops.Length; i++)
                {
                    d.possibleDrops[i].chooseChance += d.possibleDrops[i - 1].chooseChance;
                }
            }

            float randomNumber = Random.Range(0f, 1f);
            Debug.Log(randomNumber);
            foreach (Loot loot in possibleDrops)
            {
                if (randomNumber <= loot.chooseChance)
                {
                    for (int i = 0; i < loot.dropItems.Length; i++)
                    {
                        ItemDropManager.current.DropItemStack(npcPosition, new ItemStack(loot.dropItems[i].item.GetItemId(), loot.dropItems[i].amount));
                    }
                    return;
                }
            }
        }
    }
    

    [System.Serializable]
    public class Loot
    {
        public DropItems[] dropItems;
        [Range(0f, 1f)]
        public float chooseChance = 1f;
    }

    [System.Serializable]
    public class DropItems
    {
        public int amount;
        public Item item;
    }
}