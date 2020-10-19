using System.Collections;
using GameItems;
using UnityEngine;
using GameItems.Location;

namespace Inventory
{
	public abstract class EntityInventory : MonoBehaviour
	{
		public abstract int AddStack(ItemStack stack);
		public abstract bool AddItem(Item item);
		public abstract bool AddItem(string id);
		public abstract void RemoveItemOfType(string id, int amount);
		public abstract bool HasStacks(ItemStack[] stacks);
	}
}


namespace GameItems.Location
{
	public enum ItemPosition
	{
		Inventory,
		Hotbar,
		Stored,
		Dropped
	}

	/// <summary>
	/// The position of an item.
	/// </summary>
	public class ItemLocation
	{
		/// <summary>
		/// If the position is assigned to an item.
		/// </summary>
		public bool hasItem = false;
		public ItemPosition generalPosition;
		public int slot;

		public ItemLocation()
		{

		}
		public ItemLocation(bool valid, ItemPosition generalPos, int index)
		{
			hasItem = valid;
			generalPosition = generalPos;
			slot = index;
		}
		public ItemLocation(bool valid, ItemPosition generalPos)
		{
			hasItem = valid;
			generalPosition = generalPos;
			slot = -1;
		}
	}
}

namespace GameItems
{
	//[System.Serializable]
	public class ItemStack : IListCopyable
	{
		public string itemId;
		public int size;
		public int maxSize;
		public ItemLocation location;

		public ItemStack(string itemId, int size)
		{
			this.size = size;
			this.itemId = itemId;
			maxSize = ItemManager.GetStackSizeById(itemId);

			if (size > maxSize)
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

		public void SetupLocation(ItemPosition pos, int index)
		{
			location = new ItemLocation(true, pos, index);
		}

		/// <summary>
		/// Adds the amount to the size
		/// </summary>
		/// <returns>returns the amount that's left when the </returns>
		public int AddItems(int amount)
		{
			size += amount;

			int overflow = Mathf.Clamp(size - maxSize, 0, maxSize);
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

		public IListCopyable Copy()
		{
			ItemStack copy = new ItemStack(itemId, size);
			return copy;
		}
	}
}