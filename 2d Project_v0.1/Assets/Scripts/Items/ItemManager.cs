using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLoad;

namespace GameItems
{
	// Written by Lukas Sacher / Camo

	/// <summary>
	/// It manages the items and provides handy functions. Also has all existing items as an array.
	/// </summary>
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
		public static string GetIdByName(string name)
		{
			foreach (Item item in items)
			{
				if(GetNameById(item.GetItemId()) == name)
				{
					return item.GetItemId();
				}
			}
			throw new System.Exception($"Couldn't find item with name: '{name}'!");
		}
		public static Item GetItemById(string itemId)
		{
			foreach (Item item in items)
			{
				if(item.GetItemId() == itemId)
				{
					return item;
				}
			}
			throw new System.Exception($"Couldn't find item with id: '{itemId}'!");
		}
	}
}
