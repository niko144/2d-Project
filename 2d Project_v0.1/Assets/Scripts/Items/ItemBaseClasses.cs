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
	}

	/// <summary>
	/// The position of an item.
	/// </summary>
	public class ItemLocation
	{
		public bool isStored 
		{
			get => itemStore == null;
		}
		public ItemPosition generalPosition;
		public int slot;
		public IItemStore itemStore = null;

		public ItemLocation()
		{

		}
		public ItemLocation(ItemPosition generalPos, int index, IItemStore itemStore)
		{
			generalPosition = generalPos;
			slot = index;
			this.itemStore = itemStore;
		}
		public ItemLocation(ItemPosition generalPos, IItemStore itemStore)
		{
			generalPosition = generalPos;
			slot = -1;
			this.itemStore = itemStore;
		}
	}

	public interface IItemStore
	{

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

		public bool isFull => maxSize == size;

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

		public void SetupLocation(ItemPosition pos, int index, IItemStore store)
		{
			location = new ItemLocation(pos, index, store);
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