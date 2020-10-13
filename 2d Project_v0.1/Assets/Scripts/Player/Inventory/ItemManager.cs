using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLoad;

namespace PlayerInventory
{
	// Written by Lukas Sacher / Camo
	public static class ItemManager
	{
		const int idNameStartIndex = 11;

		public static Item[] items;

		[ExecuteOnGameLoad]
		static void GetItemData()
		{
			items = Resources.LoadAll<Item>("");
		}

		public static int GetStackSizeById(string id)
		{
			string itemName = GetNameById(id);

			foreach (Item item in items)
			{
				if(itemName == item.name)
				{
					return item.stackSize;
				}
			}

			return -1;
		}

		public static string GetNameById(string stringId)
		{
			char[] id = stringId.ToCharArray();
			string n = "";

			for (int i = idNameStartIndex; i < id.Length; i++)
			{
				if(id[i] == '_')
				{
					return n;
				}

				n += id[i];
			}

			return n;
		}
	}
}
