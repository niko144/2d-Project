using GameItems;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCraftingRecipe", menuName = "Items/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public Craft[] possibleCrafts;
    public CraftItem result;

	private void OnValidate()
	{
		CheckChoosingChances();
	}

	void CheckChoosingChances()
	{
        float totalChance = 0f;

		foreach (Craft craft in possibleCrafts)
		{
			totalChance += craft.chooseChance;
		}

        if(totalChance != 1f)
		{
            Debug.LogWarning($"The choosing chances for the craft '{name}' don't result in 100%.");
		}
	}

	public CraftingRecipe Copy()
	{
		CraftingRecipe newRecipe = CreateInstance<CraftingRecipe>();

		newRecipe.possibleCrafts = new Craft[possibleCrafts.Length];

		for (int i = 0; i < possibleCrafts.Length; i++)
		{
			newRecipe.possibleCrafts[i] = possibleCrafts[i].Copy();
		}

		newRecipe.result = result.Copy();
		return newRecipe;
	}
}

[System.Serializable]
public class Craft
{
    public CraftItem[] required;
    [Space(8f)]
    [Range(0f, 1f)]
    public float chooseChance = 1f;

	[HideInInspector] public CraftItem result;

	public Craft Copy()
	{
		Craft newCraft = new Craft();
		newCraft.required = new CraftItem[required.Length];

		for (int i = 0; i < required.Length; i++)
		{
			newCraft.required[i] = required[i].Copy();
		}
		newCraft.chooseChance = chooseChance;
		newCraft.result = result.Copy();

		return newCraft;
	}
}
[System.Serializable]
public class CraftItem
{
    public Item item;
    public int amount;

	public CraftItem Copy()
	{
		CraftItem newItem = new CraftItem();
		newItem.item = item;
		newItem.amount = amount;

		return newItem;
	}
}