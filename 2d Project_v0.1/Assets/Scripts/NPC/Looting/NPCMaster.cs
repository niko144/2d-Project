using UnityEngine;

namespace NPC
{
    // Written by Prem
    public class NPCMaster : MonoBehaviour
    {
        public delegate void GeneralEventHandler();
        public event GeneralEventHandler EventNPCDie;
        public event GeneralEventHandler EventNPCLowHealth;
        public event GeneralEventHandler EventNPCHealthRecovered;


        public delegate void HealthEventHandler(int health);
        public event HealthEventHandler EventNPCDeductHealth;
        public event HealthEventHandler EventNPCIncreaseHealth;


        public void CallEventNPCDie()
        {
            EventNPCDie?.Invoke();
        }
        public void CallEventNPCLowHealth()
        {
            EventNPCLowHealth?.Invoke();
        }
        public void CallEventNPCHealthRecovered()
        {
            EventNPCHealthRecovered?.Invoke();
        }
        public void CallEventNPCDeductHealth(int health)
        {
            EventNPCDeductHealth?.Invoke(health);
        }
        public void CallEventNPCIncreaseHealth(int health)
        {
            EventNPCIncreaseHealth?.Invoke(health);
        }

    }
}
