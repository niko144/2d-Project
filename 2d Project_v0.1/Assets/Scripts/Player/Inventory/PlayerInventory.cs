using System;
using System.Collections;
using System.Collections.Generic;
using GameItems;
using GameItems.Location;
using UnityEngine;

// Written by Lukas Sacher / Camo

namespace Inventory.Player
{
	/// <summary>
	/// The inventory of the player. This class only contains the data. Visualizing the inventory is handled by other classes.
	/// </summary>
	public class PlayerInventory : EntityInventory, IItemStore
	{
		public int hotbarSlotAmount;
		public int inventorySlotAmount;

		public ItemStack[] hotbar;
		public ItemStack[] inventory;
		ItemStack[] mergedInventory;

		private bool hotbarIsFull => CheckFullHotbar();
		private bool inventoryIsFull => CheckFullInventoy();

		public event Action<ItemStack, ItemLocation> onSlotUpdate;

		private void Awake()
		{
			SetupInventory();
		}
		private void Start()
		{
			//StartCoroutine(TestAddingItems());
		}

		IEnumerator TestAddingItems()
		{
			AddStack(new ItemStack(ItemManager.GetIdByName("Wood"), 13));
			yield return new WaitForSeconds(.5f);
			AddStack(new ItemStack(ItemManager.GetIdByName("Wood"), 13));
			yield return new WaitForSeconds(.5f);
			AddStack(new ItemStack(ItemManager.GetIdByName("Titanium"), 10));
			yield return new WaitForSeconds(.5f);
			AddStack(new ItemStack(ItemManager.GetIdByName("Iron"), 1));
			yield return new WaitForSeconds(.5f);
			AddStack(new ItemStack(ItemManager.GetIdByName("Spring"), 5));
			yield return new WaitForSeconds(.5f);
			AddStack(new ItemStack(ItemManager.GetIdByName("Wire"), 2));
			yield return new WaitForSeconds(.5f);
		}

		private void SetupInventory()
		{
			hotbar = new ItemStack[hotbarSlotAmount];
			inventory = new ItemStack[inventorySlotAmount];
			mergedInventory = new ItemStack[inventorySlotAmount + hotbarSlotAmount];
		}

		/// <summary>
		/// Adds a stack of items to the inventory.
		/// </summary>
		/// <returns>Returns the overflowed item amount, that doesn't fit into the whole inventory.</returns>

		public override int AddStack(ItemStack stack)
		{
			int overflow = stack.size;
			bool existingStacksLeft = true;

			while (existingStacksLeft)
			{
				(existingStacksLeft, overflow) = AddToExistingStacks(stack, overflow);
			}

			if (overflow <= 0)
			{
				EditedMergedInventory();
				return 0;
			}

			ItemStack overflowData = new ItemStack(stack.itemId, overflow);

			for (int i = 0; i < mergedInventory.Length; i++)
			{
				if (mergedInventory[i] == null)
				{
					SetMergedInventorSlot(i, overflowData);
					EditedMergedInventory();
					return 0;
				}
			}

			EditedMergedInventory();
			return overflow;
		}
		#region AddStackFunctions ForCleanCode

		(bool, int) AddToExistingStacks(ItemStack stack, int currentOverflow)
		{
			int overflow = currentOverflow;
			bool foundStack = false;

			for (int i = 0; i < mergedInventory.Length; i++)
			{
				if (mergedInventory[i] == null) continue;

				if (mergedInventory[i].itemId == stack.itemId && !mergedInventory[i].isFullStack)
				{
					overflow = AddItemsToMergedInventorySlot(i, stack.size);
					foundStack = true;
					break;
				}
			}

			return (foundStack, overflow);
		}

		#endregion
		/// <summary>
		/// Adds an item to the inventory.
		/// </summary>
		/// <returns>Returns if the item could be added.</returns>
		public override bool AddItem(Item item)
		{
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
		public override bool AddItem(string itemId)
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
			ItemLocation itemPosition = SearchForItem(itemId);
			if (itemPosition.isStored)
			{
				if (itemPosition.generalPosition == ItemPosition.Hotbar)
				{
					hotbar[itemPosition.slot].RemoveItems(1);
					onSlotUpdate?.Invoke(hotbar[itemPosition.slot], new ItemLocation(ItemPosition.Hotbar, itemPosition.slot, this));
				}
				else if (itemPosition.generalPosition == ItemPosition.Hotbar)
				{
					inventory[itemPosition.slot].RemoveItems(1);
					onSlotUpdate?.Invoke(inventory[itemPosition.slot], new ItemLocation(ItemPosition.Inventory, itemPosition.slot, this));
				}
			}
			CleanSlots();
		}
		public override void RemoveItemOfType(string itemId, int amount)
		{
			ItemLocation[] itemPositions = SearchForItems(itemId);
			int maxStackSize = ItemManager.GetStackSizeById(itemId);
			int stacks = (int)((float)amount / (float)maxStackSize);
			int rest = amount - stacks * maxStackSize;

			for (int i = itemPositions.Length - 1; i >= 0; i--)
			{
				if (!itemPositions[i].isStored) continue;

				if (itemPositions[i].generalPosition == ItemPosition.Hotbar)
				{
					if (stacks > 0)
					{
						hotbar[itemPositions[i].slot].RemoveItems(maxStackSize);
						onSlotUpdate?.Invoke(hotbar[itemPositions[i].slot], new ItemLocation(ItemPosition.Hotbar, itemPositions[i].slot, this));
						stacks--;
					}
					else
					{
						hotbar[itemPositions[i].slot].RemoveItems(rest);
						onSlotUpdate?.Invoke(hotbar[itemPositions[i].slot], new ItemLocation(ItemPosition.Hotbar, itemPositions[i].slot, this));
						break;
					}
				}
				else if (itemPositions[i].generalPosition == ItemPosition.Inventory)
				{
					if (stacks > 0)
					{
						inventory[itemPositions[i].slot].RemoveItems(maxStackSize);
						onSlotUpdate?.Invoke(inventory[itemPositions[i].slot], new ItemLocation(ItemPosition.Inventory, itemPositions[i].slot, this));
						stacks--;
					}
					else
					{
						inventory[itemPositions[i].slot].RemoveItems(rest);
						onSlotUpdate?.Invoke(inventory[itemPositions[i].slot], new ItemLocation(ItemPosition.Inventory, itemPositions[i].slot, this));
						break;
					}
				}
			}

			CleanSlots();
		}

		public override bool HasStacks(ItemStack[] stacks)
		{
			List<ItemStack> requiredStacks = stacks.ToList<ItemStack>();

			// Put the required stacks in the correct format
			for (int i = 0; i < requiredStacks.Count; i++)
			{
				if (i == requiredStacks.Count - 1) break;

				for (int j = i + 1; j < requiredStacks.Count; j++)
				{
					if (requiredStacks[i].itemId == requiredStacks[j].itemId)
					{
						int reqSize = requiredStacks[i].size;

						requiredStacks[i].AddItems(requiredStacks[j].size);
						requiredStacks[j].RemoveItems(ItemManager.GetStackSizeById(requiredStacks[i].itemId) - reqSize);

						if (requiredStacks[j].size == 0)
						{
							requiredStacks.RemoveAt(j);
							j--;
						}
						else
						{
							break;
						}
					}
				}
			}

			// Check the inventory for the stacks
			for (int i = 0; i < mergedInventory.Length; i++)
			{
				if (mergedInventory[i] == null) continue;
				for (int j = 0; j < requiredStacks.Count; j++)
				{
					if (requiredStacks[j] == null)
					{
						Debug.LogWarning($"No item in the required stacks should be null. Check at index '{j}'");
						continue;
					}

					if (mergedInventory[i].itemId == requiredStacks[j].itemId)
					{
						requiredStacks[j].RemoveItems(mergedInventory[i].size);

						if (requiredStacks[j].size == 0)
						{
							requiredStacks.RemoveAt(j);
						}
					}
				}
			}

			return requiredStacks.Count < 1;
		}

		private void CleanSlots()
		{
			for (int i = 0; i < hotbar.Length; i++)
			{
				if (hotbar[i] != null && hotbar[i].size <= 0)
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

		/// <summary>
		/// Sets the 'to' slot to the 'from' slot and the 'from' slot to null.
		/// </summary>
		public void SwapItemPosition(ItemLocation from, ItemLocation to)
		{
			if (from.generalPosition == ItemPosition.Hotbar && to.generalPosition == ItemPosition.Hotbar)
			{
				hotbar[to.slot] = hotbar[from.slot].Copy() as ItemStack;
				hotbar[from.slot] = null;
				onSlotUpdate?.Invoke(inventory[from.slot], new ItemLocation(ItemPosition.Hotbar, from.slot, this));
				onSlotUpdate?.Invoke(inventory[to.slot], new ItemLocation(ItemPosition.Hotbar, to.slot, this));
			}
			else if (from.generalPosition == ItemPosition.Hotbar && to.generalPosition == ItemPosition.Inventory)
			{
				inventory[to.slot] = hotbar[from.slot].Copy() as ItemStack;
				hotbar[from.slot] = null;
				onSlotUpdate?.Invoke(inventory[from.slot], new ItemLocation(ItemPosition.Hotbar, from.slot, this));
				onSlotUpdate?.Invoke(inventory[to.slot], new ItemLocation(ItemPosition.Inventory, to.slot, this));
			}
			else if (from.generalPosition == ItemPosition.Inventory && to.generalPosition == ItemPosition.Hotbar)
			{
				hotbar[to.slot] = inventory[from.slot].Copy() as ItemStack;
				inventory[from.slot] = null;
				onSlotUpdate?.Invoke(inventory[from.slot], new ItemLocation(ItemPosition.Inventory, from.slot, this));
				onSlotUpdate?.Invoke(inventory[to.slot], new ItemLocation(ItemPosition.Hotbar, to.slot, this));
			}
			else if (from.generalPosition == ItemPosition.Inventory && to.generalPosition == ItemPosition.Inventory)
			{
				inventory[to.slot] = inventory[from.slot].Copy() as ItemStack;
				inventory[from.slot] = null;
				onSlotUpdate?.Invoke(inventory[from.slot], new ItemLocation(ItemPosition.Inventory, from.slot, this));
				onSlotUpdate?.Invoke(inventory[to.slot], new ItemLocation(ItemPosition.Inventory, to.slot, this));
			}

			EditedSplitInventory();
		}
		/// <summary>
		/// Changes the items at the given positions
		/// </summary>
		public void SwitchItems(ItemLocation a, ItemLocation b)
		{
			if (a.generalPosition == ItemPosition.Hotbar && b.generalPosition == ItemPosition.Hotbar)
			{
				ItemStack copy = (ItemStack)hotbar[a.slot].Copy();

				hotbar[a.slot] = (ItemStack)hotbar[b.slot].Copy();
				hotbar[b.slot] = (ItemStack)copy.Copy();
				onSlotUpdate?.Invoke(inventory[a.slot], new ItemLocation(ItemPosition.Hotbar, a.slot, this));
				onSlotUpdate?.Invoke(inventory[b.slot], new ItemLocation(ItemPosition.Hotbar, b.slot, this));
			}
			else if (a.generalPosition == ItemPosition.Hotbar && b.generalPosition == ItemPosition.Inventory)
			{
				ItemStack copy = (ItemStack)hotbar[a.slot].Copy();

				hotbar[a.slot] = (ItemStack)inventory[b.slot].Copy();
				inventory[b.slot] = (ItemStack)copy.Copy();
				onSlotUpdate?.Invoke(inventory[a.slot], new ItemLocation(ItemPosition.Hotbar, a.slot, this));
				onSlotUpdate?.Invoke(inventory[b.slot], new ItemLocation(ItemPosition.Inventory, b.slot, this));
			}
			else if (a.generalPosition == ItemPosition.Inventory && b.generalPosition == ItemPosition.Hotbar)
			{
				ItemStack copy = (ItemStack)inventory[a.slot].Copy();

				inventory[a.slot] = (ItemStack)hotbar[b.slot].Copy();
				hotbar[b.slot] = (ItemStack)copy.Copy();
				onSlotUpdate?.Invoke(inventory[a.slot], new ItemLocation(ItemPosition.Inventory, a.slot, this));
				onSlotUpdate?.Invoke(inventory[b.slot], new ItemLocation(ItemPosition.Hotbar, b.slot, this));
			}
			else if (a.generalPosition == ItemPosition.Inventory && b.generalPosition == ItemPosition.Inventory)
			{
				ItemStack copy = (ItemStack)inventory[a.slot].Copy();

				inventory[a.slot] = (ItemStack)inventory[b.slot].Copy();
				inventory[b.slot] = (ItemStack)copy.Copy();
				onSlotUpdate?.Invoke(inventory[a.slot], new ItemLocation(ItemPosition.Inventory, a.slot, this));
				onSlotUpdate?.Invoke(inventory[b.slot], new ItemLocation(ItemPosition.Inventory, b.slot, this));
			}
		}

		ItemLocation SearchForItem(string itemId)
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
					ItemLocation pos = new ItemLocation(ItemPosition.Hotbar, i, this);

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
					ItemLocation pos = new ItemLocation(ItemPosition.Inventory, i, this);

					return pos;
				}
				i++;
			}

			return new ItemLocation
			{
				itemStore = null
			};
		}
		ItemLocation[] SearchForItems(string itemId)
		{
			List<ItemLocation> positions = new List<ItemLocation>();

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
					ItemLocation pos = new ItemLocation(ItemPosition.Hotbar, i, this);

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
					ItemLocation pos = new ItemLocation(ItemPosition.Inventory, i, this);

					positions.Add(pos);
				}
				i++;
			}

			return positions.ToArray();
		}

		private bool CheckFullHotbar()
		{
			foreach (ItemStack stack in hotbar)
			{
				if (stack == null) return false;
				if (stack.size < stack.maxSize)
				{
					return false;
				}
			}
			return true;
		}
		private bool CheckFullInventoy()
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

		void EditedSplitInventory()
		{
			for (int i = 0; i < hotbar.Length; i++)
			{
				if (hotbar[i] == null)
				{
					mergedInventory[i] = null;
					continue;
				}

				mergedInventory[i] = hotbar[i].Copy() as ItemStack;
			}

			int mergedIndex = hotbar.Length;
			for (int i = 0; i < inventory.Length; i++, mergedIndex++)
			{
				if(inventory[i] == null)
				{
					mergedInventory[mergedIndex] = null;
					continue;
				}

				mergedInventory[mergedIndex] = inventory[i].Copy() as ItemStack;
			}
		}
		void EditedMergedInventory()
		{
			int mergedIndex = 0;
			for (int i = 0; i < hotbar.Length; i++, mergedIndex++)
			{
				hotbar[i] = mergedInventory[mergedIndex];
			}

			for (int i = 0; i < inventory.Length; i++, mergedIndex++)
			{
				inventory[i] = mergedInventory[mergedIndex];
			}
		}

		void SetHotbarSlot(int index, ItemStack data)
		{
			hotbar[index] = data;
			mergedInventory[index] = hotbar[index].Copy() as ItemStack;
			onSlotUpdate?.Invoke(hotbar[index], new ItemLocation(ItemPosition.Hotbar, index, this));
		}
		int AddItemsToHotbarSlot(int index, int amount)
		{
			int overflow = 0;

			if(amount < 0)
			{
				hotbar[index].RemoveItems(Mathf.Abs(amount));
				mergedInventory[index] = hotbar[index].Copy() as ItemStack;
				onSlotUpdate?.Invoke(hotbar[index], new ItemLocation(ItemPosition.Hotbar, index, this));
			}
			else if (amount > 0)
			{
				overflow = hotbar[index].AddItems(amount);
				mergedInventory[index] = hotbar[index].Copy() as ItemStack;
				onSlotUpdate?.Invoke(hotbar[index], new ItemLocation(ItemPosition.Hotbar, index, this));
			}

			return overflow;
		}
		void SetInventorySlot(int index, ItemStack data)
		{
			inventory[index] = data;
			mergedInventory[hotbarSlotAmount + index] = inventory[index].Copy() as ItemStack;
			onSlotUpdate?.Invoke(inventory[index], new ItemLocation(ItemPosition.Inventory, index, this));
		}
		int AddItemsToInventorySlot(int index, int amount)
		{
			int overflow = 0;

			if (amount < 0)
			{
				inventory[index].RemoveItems(Mathf.Abs(amount));
				mergedInventory[hotbarSlotAmount + index].size = inventory[index].size;
				onSlotUpdate?.Invoke(inventory[index], new ItemLocation(ItemPosition.Inventory, index, this));
			}
			else if (amount > 0)
			{
				overflow = inventory[index].AddItems(amount);
				mergedInventory[hotbarSlotAmount + index].size = inventory[index].size;
				onSlotUpdate?.Invoke(inventory[index], new ItemLocation(ItemPosition.Inventory, index, this));
			}

			return overflow;
		}
		void SetMergedInventorSlot(int index, ItemStack data)
		{
			if (index < hotbarSlotAmount)
			{
				SetHotbarSlot(index, data);
			}
			else
			{
				SetInventorySlot(index - hotbarSlotAmount, data);
			}
		}
		int AddItemsToMergedInventorySlot(int index, int amount)
		{
			if(index < hotbarSlotAmount)
			{
				return AddItemsToHotbarSlot(index, amount);
			}
			else
			{
				return AddItemsToInventorySlot(index - hotbarSlotAmount, amount);
			}
		}
	}
}
