using UnityEngine;
using PlayerInput;

namespace PlayerStats {
    public class PlayerHealth : MonoBehaviour
    {
        //Written By Muneeb Ur Rahman
        [HideInInspector] public int currentHealth;
        public int maxHealth;
        [HideInInspector] public float hunger;
        public int maxHunger;
        [HideInInspector] public float thirst;
        public int maxThirst;
        public float hungerCoef;
        public float thirstCoef;
        [HideInInspector]public bool isMoving;
        void Start()
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
                if (hunger < maxHunger)
                {
                    hunger += hungerCoef * Time.deltaTime;
                }
                if (thirst < maxThirst)
                {
                    thirst += thirstCoef * Time.deltaTime;
                }
            }
            else
            {
                if (hunger < maxHunger)
                {
                    hunger += (hungerCoef * 1.2f) * Time.deltaTime;
                }
                if (thirst < maxThirst)
                {
                    thirst += (thirstCoef * 1.2f) * Time.deltaTime;
                }
            }
        }
        public void Heal(int healAmount)
        {
            currentHealth += healAmount;
            if (currentHealth < maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
        public void Damage(int damageAmount)
        {
            currentHealth -= damageAmount;
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
        }

        public void Eat(float foodAmount)
        {
            hunger -= foodAmount;
            if (hunger < 0)
            {
                hunger = 0;
            }
        }
        public void Drink(float waterAmount)
        {
            thirst -= waterAmount;
            if (thirst < 0)
            {
                thirst = 0;
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