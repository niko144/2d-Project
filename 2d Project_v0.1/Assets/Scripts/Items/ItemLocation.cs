using GameItems.Inventorys.Entitys.Player;

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
		static int hotbarSize = -1;

		public bool isStored
		{
			get => itemStore == null;
		}
		public ItemPosition generalPosition;
		public int slot;
		public IItemStore itemStore = null;

		public ItemLocation()
		{
			InitHotbarSize();
		}
		public ItemLocation(ItemPosition generalPos, int index, IItemStore itemStore)
		{
			InitHotbarSize();
			generalPosition = generalPos;
			slot = index;
			this.itemStore = itemStore;
		}
		public ItemLocation(ItemPosition generalPos, IItemStore itemStore)
		{
			InitHotbarSize();
			generalPosition = generalPos;
			slot = -1;
			this.itemStore = itemStore;
		}

		public int GetMergedInventoryIndex()
		{
			if (generalPosition == ItemPosition.Hotbar)
			{
				return slot;
			}
			else if (generalPosition == ItemPosition.Inventory)
			{
				return slot + hotbarSize;
			}
			else return -1;
		}

		void InitHotbarSize()
		{
			if (hotbarSize > 0) return;

			PlayerInventory inventory = GameManager.current.LocalPlayer?.GetComponent<PlayerInventory>();

			if (inventory != null) hotbarSize = inventory.HotbarSlotAmount;
		}
	}

	public interface IItemStore
	{

	}
}
