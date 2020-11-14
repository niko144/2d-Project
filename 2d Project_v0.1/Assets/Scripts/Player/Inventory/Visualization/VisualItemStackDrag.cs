using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GameItems.Location;
using GameItems.Inventorys.Entitys.Player;

namespace GameItems.Vizualization
{
	// Written by Lukas Sacher / Camo
	public class VisualItemStackDrag : MonoBehaviour, IPointerClickHandler
	{
		public bool isDragged = false;

		ItemLocation previousLocation;
		ItemLocation currentLocation;

		Transform originalParent;

		PlayerInventory playerInventory;

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

				if (go != gameObject && go.transform.HasNameTag("VisualItemStack"))
				{
					return false;
				}

				if (go.transform.HasNameTag("Slot"))
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

				if (go.transform.HasNameTag("Slot"))
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
			if (previousLocation == null) previousLocation = new ItemLocation();

			currentLocation.slot = transform.parent.GetSiblingIndex();

			if (transform.CheckForNameTagInParents(VisualStackDragManager.current.hotbarSlotContainer.FindTagInName()))
			{
				currentLocation.generalPosition = ItemPosition.Hotbar;
			}
			else if (transform.CheckForNameTagInParents(VisualStackDragManager.current.inventorySlotContainer.FindTagInName()))
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
			if (playerInventory == null) playerInventory = GameManager.current.GetComponentFromLocalPlayer<PlayerInventory>();
			if (playerInventory != null) playerInventory.SwapItemPosition(previousLocation, currentLocation);
		}
	}
}