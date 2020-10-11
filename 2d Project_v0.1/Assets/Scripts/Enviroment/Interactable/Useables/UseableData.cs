using UnityEngine;

namespace Useable
{
    // Written by Lukas Sacher/Camo
    [CreateAssetMenu(fileName = "NewUseable", menuName = "Useable")]
    public class UseableData : ScriptableObject
    {
        public float requiredDistance = 1f;
        public float interactionTime = .8f;
    }
}
