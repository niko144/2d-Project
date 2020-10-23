using UnityEngine;
using UnityEngine.UI;
using GameItems;

namespace Inventory.Vizualization
{
    // Written by Lukas Sacher / Camo
    public class VisualItemStack : MonoBehaviour
    {
        [SerializeField] Text sizeNumberDisplay = null;
        [SerializeField] Image stackImage = null;

        public void SetByStack(ItemStack stack)
		{
            transform.localPosition = Vector2.zero;
            if (stack == null) Destroy(gameObject);
			else
			{
                SetSizeNumber(stack.size);
                SetStackSprite(ItemManager.GetItemById(stack.itemId).sprite);
            }
		}
        public void SetSizeNumber(int num)
		{
            transform.localPosition = Vector2.zero;
            sizeNumberDisplay.text = num.ToString();
		}
        public void SetStackSprite(Sprite sprite)
		{
            transform.localPosition = Vector2.zero;
            stackImage.sprite = sprite;
		}
    }
}