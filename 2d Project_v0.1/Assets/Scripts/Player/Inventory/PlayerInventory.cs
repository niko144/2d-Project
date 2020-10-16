using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlayerInventory
{
	// Written by Lukas Sacher / Camo
	public class PlayerInventory : MonoBehaviour
    {
        public int hotbarSlotAmount;
        public int inventorySlotAmount;

        public ItemStack[] hotbar;
        public ItemStack[] inventory;

		public bool hotbarIsFull
		{
			get
			{
				return CheckFullHotbar();
			}
			private set
			{

			}
		}
		public bool inventoryIsFull
		{
			get
			{
				return CheckFullInventoy();
			}
			private set
			{

			}
		}

		private void Start()
		{
			SetupInventory();
			for (int i = 0; i < 5; i++)
			{
				AddItem(ItemManager.GetIdByName("IronSword"));
			}
		}

		void SetupInventory()
		{
			hotbar = new ItemStack[hotbarSlotAmount];
			inventory = new ItemStack[inventorySlotAmount];
		}

		/// <summary>
		/// Adds a stack of items to the inventory.
		/// </summary>
		/// <returns>Returns the overflowed item amount.</returns>
		public int AddStack(ItemStack stack)
		{
			int overflow;

			if (!hotbarIsFull)
			{
				overflow = AddStackToHotbar(stack);
				return overflow;
			}
			else if (!inventoryIsFull)
			{
				overflow = AddStackToInventory(stack);
				return overflow;
			}
			return stack.size;
		}
		#region AddStackFunctions_ForCleanCode
		int AddStackToHotbar(ItemStack stack)
		{
			int firstEmptySlot = -1;
			int overflow = -1;

			for (int i = 0; i < hotbar.Length; i++)
			{
				if(hotbar[i] == null)
				{
					if(overflow > 0)
					{
						hotbar[i] = new ItemStack(stack.itemId, overflow);
						overflow = 0;
						break;
					}

					if (firstEmptySlot < 0)
					{
						firstEmptySlot = i;
					}
					continue;
				}

				if (hotbar[i].itemId == stack.itemId)
				{
					overflow = hotbar[i].AddItems(stack.size);
				}
			}

			if(firstEmptySlot > -1 && overflow < 0)
			{
				hotbar[firstEmptySlot] = new ItemStack(stack.itemId, stack.size);
				return 0;
			}

			if(overflow > 0 && !inventoryIsFull)
			{
				overflow = AddStackToInventory(new ItemStack(stack.itemId, overflow));
			}
			else if(overflow < 0)
			{
				AddStackToInventory(stack);
			}

			return overflow;
		}
		int AddStackToInventory(ItemStack stack)
		{
			int firstEmptySlot = -1;
			int overflow = -1;

			for (int i = 0; i < inventory.Length; i++)
			{
				if (inventory[i] == null)
				{
					if(overflow > 0)
					{
						inventory[i] = new ItemStack(stack.itemId, overflow);
						overflow = 0;
						break;
					}

					if (firstEmptySlot < 0) firstEmptySlot = i;
					continue;
				}

				if (inventory[i].itemId == stack.itemId)
				{
					overflow = inventory[i].AddItems(stack.size);
				}
			}

			if (firstEmptySlot > -1 && overflow < 0)
			{
				inventory[firstEmptySlot] = new ItemStack(stack.itemId, stack.size);
				return 0;
			}

			return overflow;
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
		public bool AddItem(string itemId)
		{
			Item item = ItemManager.GetItemById(itemId);
			if (hotbarIsFull && inventoryIsFull)
			{
				return false;
			}
			else
			{
				AddStack(new ItemStack(item.GetItemId(), 1));
				return true;
			}
		}

		public void RemoveItemOfType(string itemId)
		{
			ItemInventoryPosition itemPosition = SearchForItem(itemId);
			if (itemPosition.isValid)
			{
				if(itemPosition.generalPosition == ItemPosition.Hotbar)
				{
					hotbar[itemPosition.slot].RemoveItems(1);
				}
				else if (itemPosition.generalPosition == ItemPosition.Hotbar)
				{
					inventory[itemPosition.slot].RemoveItems(1);
				}
			}
			CleanSlots();
		}
		public void RemoveItemOfType(string itemId, int amount)
		{
			ItemInventoryPosition[] itemPositions = SearchForItems(itemId);
			int maxStackSize = ItemManager.GetStackSizeById(itemId);
			int stacks = (int)((float)amount / (float)maxStackSize);
			int rest = amount - stacks * maxStackSize;

			foreach (ItemInventoryPosition pos in itemPositions)
			{
				if (!pos.isValid) continue;

				if (pos.generalPosition == ItemPosition.Hotbar)
				{
					if (stacks > 0)
					{
						hotbar[pos.slot].RemoveItems(maxStackSize);
						stacks--;
					}
					else 
					{
						hotbar[pos.slot].RemoveItems(rest);
						break;
					}
				}
				else if (pos.generalPosition == ItemPosition.Inventory)
				{
					if (stacks > 0)
					{
						inventory[pos.slot].RemoveItems(maxStackSize);
						stacks--;
					}
					else
					{
						inventory[pos.slot].RemoveItems(rest);
						break;
					}
				}
			}

			CleanSlots();
		}

		void CleanSlots()
		{
			for (int i = 0; i < hotbar.Length; i++)
			{
				if(hotbar[i] != null && hotbar[i].size <= 0)
				{
					hotbar[i] = null;
				}
			}
			for (int i = 0; i < inventory.Length; i++)
			{
				if (inventory[i] != null && hotbar[i].size <= 0)
				{
					inventory[i] = null;
				}
			}
		}

		ItemInventoryPosition SearchForItem(string itemId)
		{
			int i = 0;
			foreach (ItemStack item in hotbar)
			{
				if (item == null)
				{
					i++;
					continue;
				}
				if (item.itemId == itemId)
				{
					ItemInventoryPosition pos = new ItemInventoryPosition
					{
						generalPosition = ItemPosition.Hotbar,
						slot = i,
						isValid = true
					};

					return pos;
				}
				i++;
			}

			i = 0;
			foreach (ItemStack item in hotbar)
			{
				if (item == null)
				{
					i++;
					continue;
				}
				if (item.itemId == itemId)
				{
					ItemInventoryPosition pos = new ItemInventoryPosition
					{
						generalPosition = ItemPosition.Inventory,
						slot = i,
						isValid = true
					};

					return pos;
				}
				i++;
			}

			return new ItemInventoryPosition
			{
				isValid = false
			};
		}
		ItemInventoryPosition[] SearchForItems(string itemId)
		{
			List<ItemInventoryPosition> positions = new List<ItemInventoryPosition>();

			int i = 0;
			foreach (ItemStack item in hotbar)
			{
				if(item == null)
				{
					i++;
					continue;
				}
				if (item.itemId == itemId)
				{
					ItemInventoryPosition pos = new ItemInventoryPosition
					{
						generalPosition = ItemPosition.Hotbar,
						slot = i,
						isValid = true
					};

					positions.Add(pos);
				}
				i++;
			}

			if (positions.Count > 0) return positions.ToArray();

			i = 0;
			foreach (ItemStack item in hotbar)
			{
				if (item == null)
				{
					i++;
					continue;
				}
				if (item.itemId == itemId)
				{
					ItemInventoryPosition pos = new ItemInventoryPosition
					{
						generalPosition = ItemPosition.Inventory,
						slot = i,
						isValid = true
					};

					positions.Add(pos);
				}
				i++;
			}

			return positions.ToArray();
		}

		bool CheckFullHotbar()
		{
			foreach (ItemStack stack in hotbar)
			{
				if (stack == null) return false;
				if(stack.size < stack.maxSize)
				{
					return false;
				}
			}
			return true;
		}
		bool CheckFullInventoy()
		{
			foreach (ItemStack stack in inventory)
			{
				if (stack == null) return false;
				if (stack.size < stack.maxSize)
				{
					return false;
				}
			}
			return true;
		}
    }

	enum ItemPosition
	{
		Inventory,
		Hotbar
	}

	class ItemInventoryPosition
	{
		public bool isValid = false;
		public ItemPosition generalPosition;
		public int slot;
	}

	[System.Serializable]
    public class ItemStack
	{
		public string itemId;
		public int size;
		public int maxSize;

		public ItemStack(string itemId, int size)
		{
			this.size = size;
			this.itemId = itemId;
			maxSize = ItemManager.GetStackSizeById(itemId);

			if(size > maxSize)
			{
				Debug.LogWarning($"Trying to create an item stack bigger then the maximum stack size. {itemId}");
				this.size = maxSize;
			}
			else if (size <= 0)
			{
				Debug.LogWarning($"Trying to create an item stack with a negative stack size. {itemId}");
				this.size = 1;
			}
		}

		/// <summary>
		/// Adds the amount to the size
		/// </summary>
		/// <returns>returns the amount that's left when the </returns>
		public int AddItems(int amount)
		{
			size += amount;

			int overflow = Mathf.Clamp( size - maxSize, 0, maxSize);
			size = Mathf.Clamp(size, 0, maxSize);
			return overflow;
		}
		/// <summary>
		/// Removes the amount from the size
		/// </summary>
		public void RemoveItems(int amount)
		{
			size -= amount;
			size = Mathf.Clamp(size, 0, maxSize);
		}
	}
}
