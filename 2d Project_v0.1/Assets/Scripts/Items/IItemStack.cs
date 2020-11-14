

using GameItems.Location;
using UnityEngine;

namespace GameItems
{
	public class ItemStack : ICopyable
	{
		public string itemId;
		public int size;
		public int maxSize;
		public ItemLocation location;

		public bool isFullStack => maxSize == size;

		public ItemStack(string itemId, int size)
		{
			this.size = size;
			this.itemId = itemId;
			maxSize = ItemManager.GetStackSizeById(itemId);

			if (maxSize <= 0) return;

			if (size > maxSize)
			{
				Printer.Warn($"Trying to create an item stack bigger then the maximum stack size. {itemId}");
				this.size = maxSize;
			}
			else if (size <= 0)
			{
				Printer.Warn($"Trying to create an item stack with a negative stack size. {itemId}");
				this.size = 0;
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

		public ICopyable Copy()
		{
			ItemStack copy = new ItemStack(itemId, size);
			return copy;
		}
	}
}
