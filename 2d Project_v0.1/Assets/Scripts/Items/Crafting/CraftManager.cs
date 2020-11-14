using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLoad;
using GameItems;
using Random = UnityEngine.Random;
using GameItems.Crafts;

namespace GameItems.Manager
{
	// Written by Lukas Sacher / Camo

	/// <summary>
	/// It manages the items and provides handy functions. Also has all existing items as an array.
	/// </summary>
	public static class CraftManager
	{
		public static CraftingRecipe[] allCrafts;
		public static Craft[] crafts;

		[ExecuteOnGameLoad]
		static void GetItemData()
		{
			allCrafts = Resources.LoadAll<CraftingRecipe>("");

			for (int i = 0; i < allCrafts.Length; i++)
			{
				allCrafts[i] = allCrafts[i].Copy() as CraftingRecipe;
			}
		}

		public static void ChooseCrafts()
		{
			crafts = new Craft[allCrafts.Length];

			for (int i = 0; i < allCrafts.Length; i++)
			{
				float random01 = Random.Range(0f, 1f);
				for (int j = 0; j < allCrafts[i].PossibleCrafts.Length; j++)
				{
					allCrafts[i].PossibleCrafts[j].result = allCrafts[i].Result.Copy() as CraftItem;

					if (random01 < allCrafts[i].PossibleCrafts[j].ChooseChance)
					{
						crafts[i] = allCrafts[i].PossibleCrafts[j].Copy() as Craft;
						break;
					}
				}
			}
		}
		public static void AdjustChooseChances()
		{
			foreach (CraftingRecipe c in allCrafts)
			{
				for (int i = 1; i < c.PossibleCrafts.Length; i++)
				{
					c.PossibleCrafts[i].AddToChooseChance(c.PossibleCrafts[i - 1].ChooseChance);
				}
			}
		}

		public static ItemStack[] CraftItemsToItemStacks(CraftItem[] items)
		{
			List<ItemStack> ret = new List<ItemStack>();

			int retIndex = 0;
			int amountLeft = 0;
			for (int i = 0; i < items.Length; i++, retIndex++)
			{
				if (amountLeft <= 0) amountLeft = items[i].amount;

				string currentId = items[i].item.GetItemId();

				ret.Add(new ItemStack(currentId, Mathf.Clamp(amountLeft, 0, ItemManager.GetStackSizeById(currentId))));
				amountLeft -= ret[retIndex].size;

				if (amountLeft > 0) i--;
			}

			return ret.ToArray();
		}

		public static Craft GetCraftByResultItemID(string id)
		{
			if (crafts == null)
			{
				Printer.Throw("Craft array of the Craft Finder isn't assigned!");
			}

			foreach (Craft craft in crafts)
			{
				if (craft.result.item.GetItemId() == id)
				{
					return craft;
				}
			}
			return null;
		}
	}
}
