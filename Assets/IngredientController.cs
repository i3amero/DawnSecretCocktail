using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientController : MonoBehaviour
{
    public SlotController slotController;

    public void OnIngredientClicked(Sprite ingredientSprite)
    {
        if(ingredientSprite == null)
        {
            Debug.Log($"Ingredient Sprite: {ingredientSprite.name}");
        }
        if(!slotController.AddToSlot(ingredientSprite))
        {
            Debug.Log("슬롯 가득참");
        }
    }
}
