﻿using UnityEngine;
using GameItems.Inventorys;
using GameItems;
using GameItems.Location;
using PlayerInput;
using GameItems.Inventorys.Entitys.Player;

namespace GameItems.Vizualization
{
	// Written by Lukas Sacher / Camo
	public class InventoryVisualization : MonoBehaviour
	{
		GameObject player;
		PlayerInventory playerInventory;
		public GameObject slotPrefab;
		public GameObject itemStackPrefab;
		[Space(15f)]
		public Transform inventorySlotContainer;
		public Transform hotbarSlotContainer;
		public GameObject permanentHotbarObject;
		public Transform permanentHotbarContainer;

		private GameObject[] inventorySlots;
		private GameObject[] hotbarSlots;

		bool inventoryOpen = false;

		private void Awake()
		{
			InitializeVariables();
			InstantiateAllSlots();

			inventoryOpen = transform.GetChild(0).gameObject.activeSelf;

			playerInventory.onSlotUpdate += UpdateSlot;
			UserInput.toggleInventory += ToggleInventory;
		}

		void InitializeVariables()
		{
			player = GameManager.current.LocalPlayer;
			playerInventory = player.GetComponent<PlayerInventory>();

			hotbarSlots = new GameObject[playerInventory.HotbarSlotAmount];
			inventorySlots = new GameObject[playerInventory.InventorySlotAmount];
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
				Printer.Print(hotbarSlots[i].transform.GetSiblingIndex());
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

		void ToggleInventory()
		{
			inventoryOpen = !inventoryOpen;
			transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
			permanentHotbarObject.gameObject.SetActive(!permanentHotbarObject.activeSelf);
			CopyInventoryHotbarToPermanentHotbar();
		}

		void CopyInventoryHotbarToPermanentHotbar()
		{
			ClearContainer(permanentHotbarContainer);

			for (int i = 0; i < hotbarSlotContainer.childCount; i++)
			{
				GameObject slotContainer = Instantiate(hotbarSlotContainer.GetChild(i).gameObject, permanentHotbarContainer);
				Destroy(slotContainer.GetComponentInChildren<VisualItemStackDrag>());
			}
		}

		void UpdateSlot(ItemStack data, ItemLocation slot)
		{
			if (data == null) return;

			if(slot.generalPosition == ItemPosition.Hotbar)
			{
				if(hotbarSlots[slot.slot].transform.childCount == 0)
				{
					Instantiate(itemStackPrefab, hotbarSlots[slot.slot].transform).GetComponent<VisualItemStack>().SetByStack(data);
				}
				else
				{
					hotbarSlots[slot.slot].transform.GetComponentInChildren<VisualItemStack>().SetByStack(data);
				}
			}
			else if(slot.generalPosition == ItemPosition.Inventory)
			{
				if (inventorySlots[slot.slot].transform.childCount == 0)
				{
					Instantiate(itemStackPrefab, inventorySlots[slot.slot].transform).GetComponent<VisualItemStack>().SetByStack(data);
				}
				else
				{
					inventorySlots[slot.slot].transform.GetComponentInChildren<VisualItemStack>().SetByStack(data);
				}
			}
			if(!inventoryOpen) CopyInventoryHotbarToPermanentHotbar();
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
			return playerInventory.Hotbar;
		}
		private ItemStack[] GetInventoryData()
		{
			return playerInventory.Inventory;
		}
	}
}