using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GameItems.Location;
using UnityEngine.UI;

public class VisualItemStackDrag : MonoBehaviour, IPointerClickHandler
{
    public bool isDragged = false;

	ItemLocation previousLocation;
	ItemLocation currentLocation;

	Transform originalParent;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (currentLocation == null) SetItemLocations();

		if (isDragged)
		{
			if (!FoundNewSlot())
			{
				SnapToOriginPosition();
				isDragged = !isDragged;
				return;
			}
		}
		else
		{
			
		}

		isDragged = !isDragged;

		if (isDragged)
		{
			previousLocation = currentLocation;

			originalParent = transform.parent;
			transform.SetParent(VisualStackDragManager.current.canvas);
		}
		else
		{
			SetToNearestSlot();
			SetItemLocations();
			UpdateInventoryData();
		}
	}

	bool FoundNewSlot()
	{
		List<RaycastResult> underMouse = Utility.RaycastMouse();

		foreach (RaycastResult raycastResult in underMouse)
		{
			GameObject go = raycastResult.gameObject;

			if (go != gameObject && go.name.Contains("_VisualItemStack"))
			{
				Debug.Log("epokf");
				return false;
			}

			if (go.name.Contains("_Slot"))
			{
				return true;
			}
		}
		return false;
	}

	void SetToNearestSlot()
	{
		List<RaycastResult> underMouse = Utility.RaycastMouse();

		foreach (RaycastResult raycastResult in underMouse)
		{
			GameObject go = raycastResult.gameObject;

			if (go.name.Contains("_Slot"))
			{
				transform.SetParent(go.transform);
				transform.localPosition = Vector2.zero;
				break;
			}
		}
	}

	void SnapToOriginPosition()
	{
		transform.SetParent(originalParent);
		transform.localPosition = Vector2.zero;
	}

	void SetItemLocations()
	{
		currentLocation = new ItemLocation();
		if(previousLocation == null) previousLocation = new ItemLocation();

		currentLocation.slot = transform.parent.GetSiblingIndex();

		if (transform.parent.parent == VisualStackDragManager.current.hotbarSlotContainer)
		{
			currentLocation.generalPosition = ItemPosition.Hotbar;
		}
		else if (transform.parent.parent == VisualStackDragManager.current.inventorySlotContainer)
		{
			currentLocation.generalPosition = ItemPosition.Inventory;
		}
		else
		{
			currentLocation.generalPosition = ItemPosition.Stored;
		}
	}

	void UpdateInventoryData()
	{
		VisualStackDragManager.current.inventory.SwapItemPosition(previousLocation, currentLocation);
	}
}
