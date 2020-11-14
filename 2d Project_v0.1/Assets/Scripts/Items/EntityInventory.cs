using GameItems;
using UnityEngine;

namespace GameItems.Inventorys.Entitys
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

