using UnityEngine;

namespace GameItems
{
    // Written by Lukas Sacher / Camo

    /// <summary>
    /// Item data as scriptable object.
    /// </summary>
    [CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
    public class Item : ScriptableObject
    {
        public string itemName;
        public int stackSize = 16;
        public Sprite sprite;

        string itemId;
        bool hasId = false;

        public void GenerateItemId()
		{
			if (itemName.Contains("\t") || itemName.Contains("\n") || itemName.Contains(" "))
			{
                Printer.Throw("Item names should always be a string without spaces!");
			}

            itemId = $"item_data: {itemName}_{stackSize}_";
            hasId = true;
		}
        public string GetItemId()
		{
			if (!hasId)
			{
                GenerateItemId();
			}
            return itemId;
		}
    }
}
