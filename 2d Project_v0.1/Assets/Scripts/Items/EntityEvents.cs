using System;
using UnityEngine;
using PlayerInput;
using System.Reflection;
using System.Collections.Generic;

namespace Events.Entitys
{
	public class EntityEvents : MonoBehaviour
	{
		public static EntityEvents current = null;
		public Dictionary<Type, EntityEventsBase> events;

		private void Awake()
		{
			if (current == null || current.gameObject == null)
			{
				current = this;
			}
			else
			{
				throw new System.Exception($"Make sure there is only one '{this.GetType().Name}' in the scene.");
			}

			InitializeEvents();
		}

		private void OnEnable()
		{
			foreach (EntityEventsBase e in events.Values)
			{
				e.OnEnable();
			}
		}
		private void OnDisable()
		{
			foreach (EntityEventsBase e in events.Values)
			{
				e.OnDisable();
			}
		}

		void InitializeEvents()
		{
			Type eventType = typeof(EntityEventsBase);
			Assembly assembly = Assembly.GetExecutingAssembly();

			Type[] assemblyTypes = assembly.GetTypes();
			List<Type> entityEventTypes = new List<Type>();

			foreach (Type type in assemblyTypes)
			{
				if (type.IsSubclassOf(eventType))
				{
					entityEventTypes.Add(type);
				}
			}

			events = new Dictionary<Type, EntityEventsBase>();

			for (int i = 0; i < entityEventTypes.Count; i++)
			{
				events.Add(entityEventTypes[i], (EntityEventsBase)entityEventTypes[i].GetConstructor(new Type[0]).Invoke(null));
			}
		}
	}

	public abstract class EntityEventsBase
	{
		public abstract void OnEnable();
		public abstract void OnDisable();
	}

	public class EntityPickUpEvents : EntityEventsBase
	{
		public event Action<GameObject> entityPickUp;

		public void EntityPickUp(GameObject entity)
		{
			entityPickUp?.Invoke(entity);
		}

		public override void OnDisable()
		{
		}

		public override void OnEnable()
		{
		}
	}
}