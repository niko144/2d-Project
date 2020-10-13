using UnityEngine;

namespace PlayerInventory
{
	// Written by Lukas Sacher / Camo
	// Idea by Lukas Sacher and Muneeb Ur Rahman
	public class PlayerInventory : MonoBehaviour
    {
        public int hotbarSlotAmount;
        public int inventorySlotAmount;

        [HideInInspector] public ItemStack[] hotbar;
        [HideInInspector] public ItemStack[] inventory;

		bool hotbarIsFull = false;
		bool inventoryIsFull = false;

		private void Start()
		{
			SetupInventory();
		}

		void SetupInventory()
		{
			hotbar = new ItemStack[hotbarSlotAmount];
			inventory = new ItemStack[inventorySlotAmount];
		}

		/// <summary>
		/// Adds a stack of items to the inventory.
		/// </summary>
		/// <returns>Returns if the stack could be added.</returns>
		public bool AddStack(ItemStack stack)
		{
			if (!hotbarIsFull)
			{
				AddStackToHotbar(stack);
				return true;
			}
			else if (!inventoryIsFull)
			{
				AddStackToInventory(stack);
				return true;
			}
			else
			{
				return false;
			}
		}
		#region AddStackFunctions_ForCleanCode
		void AddStackToHotbar(ItemStack stack)
		{
			int firstEmptySlot = -1;

			for (int i = 0; i < hotbar.Length; i++)
			{
				if(hotbar[i] == null)
				{
					if(firstEmptySlot < 0) firstEmptySlot = i;
					continue;
				}

				if (hotbar[i].itemId == stack.itemId)
				{
					hotbar[i].AddItems(stack.size);
					return;
				}
			}

			if(firstEmptySlot > -1)
			{
				hotbar[firstEmptySlot] = new ItemStack(stack.itemId, stack.size);
			}
		}
		void AddStackToInventory(ItemStack stack)
		{
			int firstEmptySlot = -1;

			for (int i = 0; i < inventory.Length; i++)
			{
				if (inventory[i] == null)
				{
					if (firstEmptySlot < 0) firstEmptySlot = i;
					break;
				}

				if (inventory[i].itemId == stack.itemId)
				{
					inventory[i].AddItems(stack.size);
					return;
				}
			}

			if (firstEmptySlot > -1)
			{
				inventory[firstEmptySlot] = new ItemStack(stack.itemId, stack.size);
			}
		}
		#endregion
		/// <summary>
		/// Adds an item to the inventory.
		/// </summary>
		/// <returns>Returns if the item could be added.</returns>
		public bool AddItem(Item item)
		{
			if(hotbarIsFull && inventoryIsFull)
			{
				return false;
			}
			else
			{
				AddStack(new ItemStack(item.GetItemId(), 1));
				return true;
			}
		}
    }

    public class ItemStack
	{
		public string itemId;
		public int size;

		public ItemStack(string itemId, int size)
		{
			this.size = size;
			this.itemId = itemId;
		}

		/// <summary>
		/// Adds the amount to the size
		/// </summary>
		/// <returns>returns the amount that's left when the </returns>
		public int AddItems(int amount)
		{
			size += amount;

			//Clamp size to maxStackSize
			//return size - maxStackSizes

			return 0;
		}
	}
}
