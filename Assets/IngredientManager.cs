using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    public SlotManager slotManager;

    public void OnIngredientClicked(Sprite ingredientSprite)
    {
        if(ingredientSprite == null)
        {
            Debug.Log($"Ingredient Sprite: {ingredientSprite.name}");
        }
        if(!slotManager.AddToSlot(ingredientSprite))
        {
            Debug.Log("½½·Ô °¡µæÂü");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
