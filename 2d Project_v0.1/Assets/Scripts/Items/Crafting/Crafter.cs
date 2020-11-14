using UnityEngine;
using GameItems.Inventorys.Entitys.Player;
using GameItems.Drops;
using GameItems.Crafts;

namespace GameItems.Manager
{
	// Written by Lukas Sacher / Camo
	public class Crafter : MonoBehaviour
	{
		PlayerInventory playerInventory;

		private void Awake()
		{
			playerInventory = GameManager.current.GetComponentFromLocalPlayer<PlayerInventory>();
		}

		public void CraftItem(Item item)
		{
			Craft craft = CraftManager.GetCraftByResultItemID(item.GetItemId());

			ItemStack[] reqStacks = CraftManager.CraftItemsToItemStacks(craft.required);
			ItemStack[] resultStacks = CraftManager.CraftItemsToItemStacks(new CraftItem[] { craft.result });

			if (playerInventory.HasStacks(reqStacks))
			{
				CraftToPlayerInventory(reqStacks, resultStacks);
			}
		}

		void CraftToPlayerInventory(ItemStack[] reqStacks, ItemStack[] resultStacks)
		{
			foreach (ItemStack s in reqStacks)
			{
				playerInventory.RemoveItemOfType(s.itemId, s.size);
			}

			for (int i = 0; i < resultStacks.Length; i++)
			{
				int overflow = playerInventory.AddStack(resultStacks[i]);
				int overflowLeft = overflow;
				int stackSize = ItemManager.GetStackSizeById(resultStacks[0].itemId);

				while (overflowLeft > 0)
				{
					ItemDropManager.current.DropItemStack(playerInventory.transform.position, new ItemStack(resultStacks[0].itemId, overflow));
					overflowLeft -= stackSize;
				}
			}
		}
	}
}
