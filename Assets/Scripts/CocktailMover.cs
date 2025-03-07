using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CocktailMover : MonoBehaviour
{
    public Image cocktailSlot;
    public TMP_Text popupText;
    private string cocktailName; 
   

    void Start()
    {

        Sprite savedSprite=CocktailSaver.Instance.GetCocktailImage();
        if(savedSprite!=null)
        {
            cocktailSlot.sprite= savedSprite;
            cocktailName = cocktailSlot.sprite.name.Split('_')[0];
            popupText.text = $"<{cocktailName}>";
        }
        else
        {
            Debug.Log("실패");
            popupText.text = $"<NULL>";
        }
    }
}


