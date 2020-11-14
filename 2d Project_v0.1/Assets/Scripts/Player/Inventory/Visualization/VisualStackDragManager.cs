using GameItems;
using GameItems.Location;
using GameItems.Inventorys;
using System;
using System.Collections.Generic;
using UnityEngine;
using GameItems.Inventorys.Entitys.Player;

public class VisualStackDragManager : MonoBehaviour
{
	public static VisualStackDragManager current = null;

	PlayerInventory inventory = null;

    public Transform inventorySlotContainer;
    public Transform hotbarSlotContainer;
    public Transform canvas;

    List<VisualItemStackDrag> filledSlots = new List<VisualItemStackDrag>();

	Action onEndDrag;

	private void Awake()
	{
		if (current == null || current.gameObject == null)
		{
			current = this;
		}
		else
		{
			Printer.Throw($"Make sure there is only one '{this.GetType().Name}' in the scene.");
		}

		inventory = GameManager.current.LocalPlayer?.GetComponent<PlayerInventory>();
	}

	private void Start()
	{
		inventory.onSlotUpdate += StashGetAllStackDraggers;
	}

	void Update()
    {
		MoveDraggedSlots();
    }

	void MoveDraggedSlots()
	{
		foreach (VisualItemStackDrag slot in filledSlots)
		{
			if (slot == null)
			{
				continue;
			}

			if (slot.isDragged)
			{
				slot.transform.position = Input.mousePosition;
				return;
			}
		}
		InvokeOnEndDrag();
	}

	void InvokeOnEndDrag()
	{
		if (onEndDrag == null) return;

		onEndDrag.Invoke();

		//Unsubscribe all stashed onEndDrag clients
		Delegate[] clientList = onEndDrag.GetInvocationList();

		foreach (Delegate client in clientList)
		{
			onEndDrag -= client as Action;
		}
	}

	void StashGetAllStackDraggers(ItemStack stack, ItemLocation location)
	{
		// prevent double subscriptions
		onEndDrag -= GetAllStackDraggers;
		onEndDrag += GetAllStackDraggers;
	}

	void GetAllStackDraggers()
	{
		filledSlots.Clear();

		for (int i = 0; i < hotbarSlotContainer.childCount; i++)
		{
			VisualItemStackDrag item = hotbarSlotContainer.GetChild(i).GetComponentInChildren<VisualItemStackDrag>();
			if (item == null) continue;

			filledSlots.Add(item);
		}

		for (int i = 0; i < inventorySlotContainer.childCount; i++)
		{
			VisualItemStackDrag item = inventorySlotContainer.GetChild(i).GetComponentInChildren<VisualItemStackDrag>();
			if (item == null) continue;

			filledSlots.Add(item);
		}
	}

	public void SwitchStackFromTo(ItemLocation previous, ItemLocation current)
	{
		if (previous.generalPosition == current.generalPosition && previous.slot == current.slot) return;

		inventory.SwapItemPosition(previous, current);
	}
}
