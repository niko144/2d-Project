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
			for (int i = 0; i < hotbar.Length; i++)
			{
				if (hotbar[i].itemId == stack.itemId)
				{

				}
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
	}
}
