﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameItems;
using GameItems.Drop;

public class ItemDropManager : MonoBehaviour
{
	public static ItemDropManager current;
	public GameObject droppedItemPrefab;
	public LayerMask whatCanPickUp;
	public float dropForce;
	public Sprite defaultSprite;

	private void Awake()
	{
		if (current == null || current.gameObject == null)
		{
			current = this;
		}
		else
		{
			throw new System.Exception("Make sure there is only one game object with the ItemDropManager script attached!");
		}
	}

    public void DropItemStack(Vector2 pos, ItemStack stack)
	{
		GameObject dropped = Instantiate(droppedItemPrefab, pos, Quaternion.identity);
		DroppedItem itemData = dropped.GetComponent<DroppedItem>();
		SpriteRenderer renderer = dropped.GetComponent<SpriteRenderer>();


		if (ItemManager.GetItemById(stack.itemId).sprite == null) renderer.sprite = defaultSprite;
		else renderer.sprite = ItemManager.GetItemById(stack.itemId).sprite;
		itemData.CopyFrom(stack);
		itemData.whatCanPickUp = whatCanPickUp;
		itemData.DropAnim(new Vector2(Random.Range(pos.x + 1.5f, pos.x - 1.5f), Random.Range(pos.y + 1.5f, pos.y - 1.5f)), dropForce);
	}
}
