using UnityEngine;
using Inventory;
using PlayerInput;
using System;
using Events.Entitys;

namespace GameItems.Drop
{
	// Written by Lukas Sacher / Camo

	/// <summary>
	/// This MonoBehaviour is attached to items, wich are lying around on the ground.
	/// Add this component when you drop a stack of items.
	/// The CopyFrom() functions sets the data the dropped item has (wich item it is and how big the stack is).
	/// </summary>
	public class DroppedItem : MonoBehaviour
	{
		public ItemStack data = null;
		public float range = 2f;
		public LayerMask whatCanPickUp;
		public Animator anim;

		private void Start()
		{
			anim = GetComponent<Animator>();
			data = new ItemStack(ItemManager.GetIdByName("Battery"), 2);
		}

		Collider2D[] nearEntitys;
		private void Update()
		{
			nearEntitys = Physics2D.OverlapCircleAll(transform.position, range, whatCanPickUp);
			anim.SetBool("InRange", nearEntitys.Length > 0);
		}

		private void OnEnable()
		{
			Debug.Log(EntityEvents.current.name);
			(EntityEvents.current.events[typeof(EntityPickUpEvents)] as EntityPickUpEvents).entityPickUp += TryCollectBy;
		}
		private void OnDisable()
		{
			(EntityEvents.current.events[typeof(EntityPickUpEvents)] as EntityPickUpEvents).entityPickUp -= TryCollectBy;
		}

		public void CopyFrom(ItemStack from)
		{
			data = (ItemStack)from.Copy();
		}

		/// <summary>
		/// Destroys the Dropped item. Doesn't handle item overflow.
		/// </summary>
		/// <returns>the stack</returns>
		public virtual ItemStack Collect()
		{
			if(data == null)
			{
				throw new System.Exception($"No DropItem.data assigned ({transform.name}). Use the CopyFrom function to assign data.");
			}
			Destroy(gameObject);
			return data;
		}
		/// <summary>
		/// Collects the item and adds it to the inventory automatically.
		/// </summary>
		public virtual void CollectAndAdd(EntityInventory inventory)
		{
			if (data == null)
			{
				throw new System.Exception($"No DropItem.data assigned ({transform.name}). Use the CopyFrom function to assign data.");
			}

			int rest = inventory.AddStack(data);
			if(rest > 0)
			{
				data = new ItemStack(data.itemId, rest);
			}
			else
			{
				Destroy(gameObject);
			}
		}

		public void TryCollectBy(GameObject entity)
		{
			foreach (Collider2D col in nearEntitys)
			{
				if (entity != col.gameObject) continue;

				try
				{
					CollectAndAdd(col.gameObject.GetComponent<EntityInventory>());
					return;
				}
				catch
				{
					throw new System.Exception($"GameObject '{col.gameObject.name}' is on the Player layer but has no Inventory!");
				}
			}
		}
	}
}
