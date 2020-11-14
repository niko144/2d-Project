using UnityEngine;

namespace GameItems.Crafts
{
	// Written by Lukas Sacher / Camo
	[CreateAssetMenu(fileName = "NewCraftingRecipe", menuName = "Items/Recipe")]
	public class CraftingRecipe : ScriptableObject, ICopyable
	{
		[SerializeField] Craft[] possibleCrafts;
		public Craft[] PossibleCrafts
		{
			get
			{
				return possibleCrafts;
			}
		}

		[SerializeField] CraftItem result;
		public CraftItem Result
		{
			get
			{
				return result;
			}
		}

		#region Editor
		private void OnValidate()
		{
			CheckChoosingChances();
		}
		void CheckChoosingChances()
		{
			float totalChance = 0f;

			foreach (Craft craft in possibleCrafts)
			{
				totalChance += craft.ChooseChance;
			}

			if (totalChance != 1f)
			{
				Printer.Warn($"The choosing chances for the craft '{name}' don't result in 100%.");
			}
		}
		#endregion

		public ICopyable Copy()
		{
			CraftingRecipe newRecipe = CreateInstance<CraftingRecipe>();

			newRecipe.possibleCrafts = new Craft[possibleCrafts.Length];

			for (int i = 0; i < possibleCrafts.Length; i++)
			{
				newRecipe.possibleCrafts[i] = possibleCrafts[i].Copy() as Craft;
			}
			
			newRecipe.result = result.Copy() as CraftItem;
			return newRecipe;
		}
	}

	[System.Serializable]
	public class Craft : ICopyable
	{
		public CraftItem[] required;
		[Space(8f)]
		[Range(0f, 1f), SerializeField]
		float chooseChance = 1f;
		public float ChooseChance
		{
			get
			{
				return chooseChance;
			}
		}
		bool increasedChooseChance = false;

		public CraftItem result
		{
			get
			{
				return result;
			}
			set
			{
				if (value == null) Printer.Throw("Can't set Craft.result to null.");

				result = value;
			}
		}

		public void AddToChooseChance(float amount)
		{
			if (increasedChooseChance) return;

			chooseChance += amount;
		}

		public ICopyable Copy()
		{
			Craft newCraft = new Craft();
			newCraft.required = new CraftItem[required.Length];

			for (int i = 0; i < required.Length; i++)
			{
				newCraft.required[i] = required[i].Copy() as CraftItem;
			}
			newCraft.chooseChance = chooseChance;
			newCraft.result = result.Copy() as CraftItem;

			return newCraft;
		}
	}
	[System.Serializable]
	public class CraftItem : ICopyable
	{
		public Item item;
		public int amount;

		public ICopyable Copy()
		{
			CraftItem newItem = new CraftItem();
			newItem.item = item;
			newItem.amount = amount;

			return newItem;
		}
	}
}