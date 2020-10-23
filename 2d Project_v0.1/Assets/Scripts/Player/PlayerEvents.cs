using System;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;


namespace Events.Player
{
    //Written by Muneeb Ur Rahman with the help of Camo
	public class PlayerEvents : MonoBehaviour
	{
		public static PlayerEvents current = null;
        public Dictionary<Type, PlayerEventsBase> events;

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
            foreach (PlayerEventsBase e in events.Values)
            {
                e.OnEnable();
            }
        }
        private void OnDisable()
        {
            foreach (PlayerEventsBase e in events.Values)
            {
                e.OnDisable();
            }
        }
        void InitializeEvents()
        {
            Type eventType = typeof(PlayerEventsBase);
            Assembly assembly = Assembly.GetExecutingAssembly();

            Type[] assemblyTypes = assembly.GetTypes();
            List<Type> playerEventTypes = new List<Type>();

            foreach (Type type in assemblyTypes)
            {
                if (type.IsSubclassOf(eventType))
                {
                    playerEventTypes.Add(type);
                }
            }

            events = new Dictionary<Type, PlayerEventsBase>();

            for (int i = 0; i < playerEventTypes.Count; i++)
            {
                events.Add(playerEventTypes[i], (PlayerEventsBase)playerEventTypes[i].GetConstructor(new Type[0]).Invoke(null));
            }
        }
    }
    public abstract class PlayerEventsBase
    {
        public abstract void OnEnable();
        public abstract void OnDisable();
    }
    public class HealthEvents : PlayerEventsBase
    {
        public event Action<int> onDamageEvent;
        public event Action<int> onHealEvent;

        public void InvokeOnDamage(int damageAmount)
        {
            onDamageEvent?.Invoke(damageAmount);
        }
        public void InvokeOnHeal(int healAmount)
        {
            onHealEvent?.Invoke(healAmount);
        }
        public override void OnDisable()
        {
        }

        public override void OnEnable()
        {
        }
    }
    public class RequirementsEvents : PlayerEventsBase
    {
        public event Action<float> onEatEvent;
        public event Action<float> onDrinkEvent;

        public void InvokeOnEat(float foodAmount)
        {
            onEatEvent(foodAmount);
        }
        public void InvokeOnDrink(float waterAmount)
        {
            onDrinkEvent(waterAmount);
        }
        public override void OnDisable()
        {
        }

        public override void OnEnable()
        {
        }
    }
}