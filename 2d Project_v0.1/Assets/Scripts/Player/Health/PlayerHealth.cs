using UnityEngine;
using PlayerInput;
using Events.Player;

namespace PlayerStats {

    //Written By Muneeb Ur Rahman
    public class PlayerHealth : MonoBehaviour
    {
        public int currentHealth;
        public int maxHealth;
        [HideInInspector]public float currentHunger;
        public int maxHunger;
        [HideInInspector]public float currentThirst;
        public int maxThirst;
        [Space(15f)]
        public float hungerCoef;
        public float thirstCoef;
        [Space(5f)]
        public float moveMultiplier = 1.2f;
        bool isMoving;

        
        private void Awake()
        {
            currentHealth = maxHealth;
            UserInput.moveUp += MoveCheck;
            UserInput.moveDown += MoveCheck;
            UserInput.moveLeft += MoveCheck;
            UserInput.moveRight += MoveCheck;
            UserInput.onInputStart += ClearInput;

        }
        private void OnDestroy()
        {
            UserInput.moveUp -= MoveCheck;
            UserInput.moveDown -= MoveCheck;
            UserInput.moveLeft -= MoveCheck;
            UserInput.moveRight -= MoveCheck;
            UserInput.onInputStart -= ClearInput;
        }
        public void Update()
        {
            if (!isMoving) {
                if (currentHunger < maxHunger)
                {
                    currentHunger += hungerCoef * Time.deltaTime;
                }
                if (currentThirst < maxThirst)
                {
                    currentThirst += thirstCoef * Time.deltaTime;
                }
            }
            else
            {
                if (currentHunger < maxHunger)
                {
                    currentHunger += (hungerCoef * moveMultiplier) * Time.deltaTime;
                }
                if (currentThirst < maxThirst)
                {
                    currentThirst += (thirstCoef * moveMultiplier) * Time.deltaTime;
                }
            }
        }
        public void Heal(int healAmount)
        {
            (PlayerEvents.current.events[typeof(HealthEvents)] as HealthEvents).InvokeOnHeal(healAmount);
            currentHealth += healAmount;
            if (currentHealth < maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
        public void Damage(int damageAmount)
        {
            (PlayerEvents.current.events[typeof(HealthEvents)] as HealthEvents).InvokeOnDamage(damageAmount);
            currentHealth -= damageAmount;
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
        }

        public void Eat(float foodAmount)
        {
            (PlayerEvents.current.events[typeof(RequirementsEvents)] as RequirementsEvents).InvokeOnEat(foodAmount);
            currentHunger -= foodAmount;
            if (currentHunger < 0)
            {
                currentHunger = 0;
            }
        }
        public void Drink(float waterAmount)
        {
            (PlayerEvents.current.events[typeof(RequirementsEvents)] as RequirementsEvents).InvokeOnDrink(waterAmount);
            currentThirst -= waterAmount;
            if (currentThirst < 0)
            {
                currentThirst = 0;
            }
        }
        void MoveCheck()
        {
            isMoving = true;
        }
        void ClearInput()
        {
            isMoving = false;
        }
    }
}