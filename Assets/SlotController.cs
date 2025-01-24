using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    public Image[] slots;
    [SerializeField]private Sprite defaultSprite;

    // Start is called before the first frame update
    public bool AddToSlot(Sprite ingredientSprite)
    {
        foreach(var slot in slots)
        {
            if(slot.sprite==defaultSprite)
            {
                slot.sprite = ingredientSprite;
                return true;
            }
        }
        return false;
    }

    public void ClearSlot(Image slot)
    {
        slot.sprite = defaultSprite;
    }
    
    public void OnSlotClicked(Image slot)
    {
        if(slot.sprite != defaultSprite)
        {
            ClearSlot(slot);
        }
    }

    public string[] GetSlotIngredients()
    {
        List<string> ingredients = new List<string>();
        foreach (var slot in slots)
        {
            if (slot.sprite != defaultSprite)
            {
                ingredients.Add(slot.sprite.name);
            }
        }
        return ingredients.ToArray();
    }
}
