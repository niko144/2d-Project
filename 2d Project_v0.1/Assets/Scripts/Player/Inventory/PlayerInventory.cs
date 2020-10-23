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
	public class PlayerInventory : EntityInventory
	{
		public int hotbarSlotAmount;
		public int inventorySlotAmount;

		public ItemStack[] hotbar;
		public ItemStack[] inventory;
		ItemStack[] mergedInventory
		{
			get
			{
				ItemStack[] ret = new ItemStack[hotbarSlotAmount + inventorySlotAmount];
				for (int i = 0; i < hotbar.Length; i++)
				{
					ret[i] = hotbar[i];
				}
				for (int i = hotbar.Length; i < hotbarSlotAmount + inventorySlotAmount; i++)
				{
					ret[i] = inventory[i - hotbarSlotAmount];
				}

				return ret;
			}
		}

		private bool hotbarIsFull => CheckFullHotbar();
		private bool inventoryIsFull => CheckFullInventoy();

		public event Action<ItemStack, ItemLocation> onSlotUpdate;

		private void Awake()
		{
			SetupInventory();
		}
		private void Start()
		{
			StartCoroutine(TestAddingItems());
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
		}

		/// <summary>
		/// Adds a stack of items to the inventory.
		/// </summary>
		/// <returns>Returns the overflowed item amount.</returns>
		public override int AddStack(ItemStack stack)
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
				if (hotbar[i] == null)
				{
					if (overflow > 0)
					{
						hotbar[i] = new ItemStack(stack.itemId, overflow);
						onSlotUpdate?.Invoke(hotbar[i], new ItemLocation(true, ItemPosition.Hotbar, i));
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
					onSlotUpdate?.Invoke(hotbar[i], new ItemLocation(true, ItemPosition.Hotbar, i));
				}
			}

			if (firstEmptySlot > -1 && overflow < 0)
			{
				hotbar[firstEmptySlot] = new ItemStack(stack.itemId, stack.size);
				onSlotUpdate?.Invoke(hotbar[firstEmptySlot], new ItemLocation(true, ItemPosition.Hotbar, firstEmptySlot));
				return 0;
			}
			if (overflow > 0 && !inventoryIsFull)
			{
				overflow = AddStackToInventory(new ItemStack(stack.itemId, overflow));
			}
			else if (overflow < 0)
			{
				AddStackToInventory(stack);
			}

			return overflow;
		}

		private int AddStackToInventory(ItemStack stack)
		{
			int firstEmptySlot = -1;
			int overflow = -1;

			for (int i = 0; i < inventory.Length; i++)
			{
				if (inventory[i] == null)
				{
					if (overflow > 0)
					{
						inventory[i] = new ItemStack(stack.itemId, overflow);
						onSlotUpdate?.Invoke(inventory[i], new ItemLocation(true, ItemPosition.Inventory, i));
						overflow = 0;
						break;
					}

					if (firstEmptySlot < 0) firstEmptySlot = i;
					continue;
				}

				if (inventory[i].itemId == stack.itemId)
				{
					overflow = inventory[i].AddItems(stack.size);
					onSlotUpdate?.Invoke(inventory[i], new ItemLocation(true, ItemPosition.Inventory, i));
				}
			}

			if (firstEmptySlot > -1 && overflow < 0)
			{
				inventory[firstEmptySlot] = new ItemStack(stack.itemId, stack.size);
				onSlotUpdate?.Invoke(inventory[firstEmptySlot], new ItemLocation(true, ItemPosition.Inventory, firstEmptySlot));
				return 0;
			}

			return overflow;
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
			if (itemPosition.hasItem)
			{
				if (itemPosition.generalPosition == ItemPosition.Hotbar)
				{
					hotbar[itemPosition.slot].RemoveItems(1);
					onSlotUpdate?.Invoke(hotbar[itemPosition.slot], new ItemLocation(true, ItemPosition.Hotbar, itemPosition.slot));
				}
				else if (itemPosition.generalPosition == ItemPosition.Hotbar)
				{
					inventory[itemPosition.slot].RemoveItems(1);
					onSlotUpdate?.Invoke(inventory[itemPosition.slot], new ItemLocation(true, ItemPosition.Inventory, itemPosition.slot));
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
				if (!itemPositions[i].hasItem) continue;

				if (itemPositions[i].generalPosition == ItemPosition.Hotbar)
				{
					if (stacks > 0)
					{
						hotbar[itemPositions[i].slot].RemoveItems(maxStackSize);
						onSlotUpdate?.Invoke(hotbar[itemPositions[i].slot], new ItemLocation(true, ItemPosition.Hotbar, itemPositions[i].slot));
						stacks--;
					}
					else
					{
						hotbar[itemPositions[i].slot].RemoveItems(rest);
						onSlotUpdate?.Invoke(hotbar[itemPositions[i].slot], new ItemLocation(true, ItemPosition.Hotbar, itemPositions[i].slot));
						break;
					}
				}
				else if (itemPositions[i].generalPosition == ItemPosition.Inventory)
				{
					if (stacks > 0)
					{
						inventory[itemPositions[i].slot].RemoveItems(maxStackSize);
						onSlotUpdate?.Invoke(inventory[itemPositions[i].slot], new ItemLocation(true, ItemPosition.Inventory, itemPositions[i].slot));
						stacks--;
					}
					else
					{
						inventory[itemPositions[i].slot].RemoveItems(rest);
						onSlotUpdate?.Invoke(inventory[itemPositions[i].slot], new ItemLocation(true, ItemPosition.Inventory, itemPositions[i].slot));
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
				hotbar[to.slot] = hotbar[from.slot];
				hotbar[from.slot] = null;
				onSlotUpdate?.Invoke(inventory[from.slot], new ItemLocation(true, ItemPosition.Hotbar, from.slot));
				onSlotUpdate?.Invoke(inventory[to.slot], new ItemLocation(true, ItemPosition.Hotbar, to.slot));
			}
			else if (from.generalPosition == ItemPosition.Hotbar && to.generalPosition == ItemPosition.Inventory)
			{
				inventory[to.slot] = hotbar[from.slot];
				hotbar[from.slot] = null;
				onSlotUpdate?.Invoke(inventory[from.slot], new ItemLocation(true, ItemPosition.Hotbar, from.slot));
				onSlotUpdate?.Invoke(inventory[to.slot], new ItemLocation(true, ItemPosition.Inventory, to.slot));
			}
			else if (from.generalPosition == ItemPosition.Inventory && to.generalPosition == ItemPosition.Hotbar)
			{
				hotbar[to.slot] = inventory[from.slot];
				inventory[from.slot] = null;
				onSlotUpdate?.Invoke(inventory[from.slot], new ItemLocation(true, ItemPosition.Inventory, from.slot));
				onSlotUpdate?.Invoke(inventory[to.slot], new ItemLocation(true, ItemPosition.Hotbar, to.slot));
			}
			else if (from.generalPosition == ItemPosition.Inventory && to.generalPosition == ItemPosition.Inventory)
			{
				inventory[to.slot] = inventory[from.slot];
				inventory[from.slot] = null;
				onSlotUpdate?.Invoke(inventory[from.slot], new ItemLocation(true, ItemPosition.Inventory, from.slot));
				onSlotUpdate?.Invoke(inventory[to.slot], new ItemLocation(true, ItemPosition.Inventory, to.slot));
			}
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
				onSlotUpdate?.Invoke(inventory[a.slot], new ItemLocation(true, ItemPosition.Hotbar, a.slot));
				onSlotUpdate?.Invoke(inventory[b.slot], new ItemLocation(true, ItemPosition.Hotbar, b.slot));
			}
			else if (a.generalPosition == ItemPosition.Hotbar && b.generalPosition == ItemPosition.Inventory)
			{
				ItemStack copy = (ItemStack)hotbar[a.slot].Copy();

				hotbar[a.slot] = (ItemStack)inventory[b.slot].Copy();
				inventory[b.slot] = (ItemStack)copy.Copy();
				onSlotUpdate?.Invoke(inventory[a.slot], new ItemLocation(true, ItemPosition.Hotbar, a.slot));
				onSlotUpdate?.Invoke(inventory[b.slot], new ItemLocation(true, ItemPosition.Inventory, b.slot));
			}
			else if (a.generalPosition == ItemPosition.Inventory && b.generalPosition == ItemPosition.Hotbar)
			{
				ItemStack copy = (ItemStack)inventory[a.slot].Copy();

				inventory[a.slot] = (ItemStack)hotbar[b.slot].Copy();
				hotbar[b.slot] = (ItemStack)copy.Copy();
				onSlotUpdate?.Invoke(inventory[a.slot], new ItemLocation(true, ItemPosition.Inventory, a.slot));
				onSlotUpdate?.Invoke(inventory[b.slot], new ItemLocation(true, ItemPosition.Hotbar, b.slot));
			}
			else if (a.generalPosition == ItemPosition.Inventory && b.generalPosition == ItemPosition.Inventory)
			{
				ItemStack copy = (ItemStack)inventory[a.slot].Copy();

				inventory[a.slot] = (ItemStack)inventory[b.slot].Copy();
				inventory[b.slot] = (ItemStack)copy.Copy();
				onSlotUpdate?.Invoke(inventory[a.slot], new ItemLocation(true, ItemPosition.Inventory, a.slot));
				onSlotUpdate?.Invoke(inventory[b.slot], new ItemLocation(true, ItemPosition.Inventory, b.slot));
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
					ItemLocation pos = new ItemLocation
					{
						generalPosition = ItemPosition.Hotbar,
						slot = i,
						hasItem = true
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
					ItemLocation pos = new ItemLocation
					{
						generalPosition = ItemPosition.Inventory,
						slot = i,
						hasItem = true
					};

					return pos;
				}
				i++;
			}

			return new ItemLocation
			{
				hasItem = false
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
					ItemLocation pos = new ItemLocation
					{
						generalPosition = ItemPosition.Hotbar,
						slot = i,
						hasItem = true
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
					ItemLocation pos = new ItemLocation
					{
						generalPosition = ItemPosition.Inventory,
						slot = i,
						hasItem = true
					};

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
	}
}
