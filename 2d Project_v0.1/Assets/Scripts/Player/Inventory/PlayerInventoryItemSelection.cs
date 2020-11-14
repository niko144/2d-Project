using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameItems.Inventorys;
using GameItems;
using PlayerInput;
using GameItems.Inventorys.Entitys.Player;

[RequireComponent(typeof(PlayerInventory))]
public class PlayerInventoryItemSelection : MonoBehaviour
{
    PlayerInventory inventory;

    public GameObject slotSelectionUiPrefab;
    GameObject slotSelectionUiObj;

    public Transform hotbarSlotsContainer;
    Transform[] uiHotabarSlots;
    public ItemStack selected { get; private set; }
    public bool selectedUseable { get; private set; }

    void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
    }

	private void Start()
	{
        UserInput.slotSelection += SelectSlot;

        uiHotabarSlots = new Transform[hotbarSlotsContainer.childCount];

		for (int i = 0; i < hotbarSlotsContainer.childCount; i++)
		{
            uiHotabarSlots[i] = hotbarSlotsContainer.GetChild(i);
		}

        slotSelectionUiObj = Instantiate(slotSelectionUiPrefab, hotbarSlotsContainer.parent);
	}

	void SelectSlot(int slot)
	{
        SelectSlotUiVisualization(slot);

        selected = inventory.Hotbar[slot]?.Copy() as ItemStack;
	}

    void SelectSlotUiVisualization(int slot)
	{

    }
}
