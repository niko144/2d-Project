﻿using UnityEngine;
using Inventory.Player;
using GameItems;
using GameItems.Location;

namespace Inventory.Vizualization
{
	// Written by Lukas Sacher / Camo
	public class InventoryVisualization : MonoBehaviour
	{
		public GameObject player;
		PlayerInventory playerInventory;
		public GameObject slotPrefab;
		public GameObject itemStackPrefab;
		[Space(15f)]
		public Transform inventorySlotContainer;
		public Transform hotbarSlotContainer;

		private GameObject[] inventorySlots;
		private GameObject[] hotbarSlots;

		private void Awake()
		{
			InitializeVariables();
			InstantiateAllSlots();
			playerInventory.onSlotUpdate += UpdateSlot;
		}

		void InitializeVariables()
		{
			playerInventory = player.GetComponent<PlayerInventory>();

			hotbarSlots = new GameObject[playerInventory.hotbarSlotAmount];
			inventorySlots = new GameObject[playerInventory.inventorySlotAmount];
		}
		void InstantiateAllSlots()
		{
			ClearContainer(hotbarSlotContainer);
			ClearContainer(inventorySlotContainer);

			for (int i = 0; i < hotbarSlots.Length; i++)
			{
				hotbarSlots[i] = Instantiate(slotPrefab, hotbarSlotContainer);
			}
			for (int i = 0; i < inventorySlots.Length; i++)
			{
				inventorySlots[i] = Instantiate(slotPrefab, inventorySlotContainer);
			}
		}

		public void RedrawInventory()
		{
			ItemStack[] hotbar = GetHotbarData();
			ItemStack[] inventory = GetInventoryData();

			for (int i = 0; i < hotbar.Length; i++)
			{
				Debug.Log(hotbarSlots[i].transform.GetSiblingIndex());
				if (hotbar[i] == null) continue;

				VisualItemStack stack = Instantiate(itemStackPrefab, hotbarSlots[i].transform).GetComponent<VisualItemStack>();

				stack.transform.localPosition = Vector2.zero;
				stack.SetSizeNumber(hotbar[i].size);
				stack.SetStackSprite(ItemManager.GetItemById(hotbar[i].itemId).sprite);
			}
			for (int i = 0; i < inventory.Length; i++)
			{
				if (inventory[i] == null) continue;

				VisualItemStack stack = Instantiate(itemStackPrefab, inventorySlots[i].transform).GetComponent<VisualItemStack>();

				stack.transform.localPosition = Vector2.zero;
				stack.SetSizeNumber(inventory[i].size);
				stack.SetStackSprite(ItemManager.GetItemById(inventory[i].itemId).sprite);
			}
		}

		void UpdateSlot(ItemStack data, ItemLocation slot)
		{
			if(slot.generalPosition == ItemPosition.Hotbar)
			{
				if(hotbarSlots[slot.slot].transform.childCount == 0 && data != null)
				{
					Instantiate(itemStackPrefab, hotbarSlots[slot.slot].transform).GetComponent<VisualItemStack>().SetByStack(data);
				}
				else
				{
					hotbarSlots[slot.slot].transform.GetChild(0).GetComponent<VisualItemStack>().SetByStack(data);
				}
			}
			else if(slot.generalPosition == ItemPosition.Inventory)
			{
				if (inventorySlots[slot.slot].transform.childCount == 0 && data != null)
				{
					Instantiate(itemStackPrefab, inventorySlots[slot.slot].transform).GetComponent<VisualItemStack>().SetByStack(data);
				}
				else
				{
					inventorySlots[slot.slot].transform.GetChild(0).GetComponent<VisualItemStack>().SetByStack(data);
				}
			}
		}

		void ClearContainer(Transform container)
		{
			for (int i = container.childCount - 1; i >= 0; i--)
			{
				Destroy(container.GetChild(i).gameObject);
			}
		}

		private ItemStack[] GetHotbarData()
		{
			return playerInventory.hotbar;
		}
		private ItemStack[] GetInventoryData()
		{
			return playerInventory.inventory;
		}
	}
}