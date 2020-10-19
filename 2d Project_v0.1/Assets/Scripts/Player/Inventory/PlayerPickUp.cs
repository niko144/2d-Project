using UnityEngine;
using PlayerInput;
using Events.Entitys;

public class PlayerPickUp : MonoBehaviour
{
	private void OnEnable()
	{
		UserInput.pickUp += PickUp;
	}
	private void OnDisable()
	{
		UserInput.pickUp -= PickUp;
	}

	void PickUp()
	{
		(EntityEvents.current.events[typeof(EntityPickUpEvents)] as EntityPickUpEvents).EntityPickUp(gameObject);
	}
}
