using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CocktailMover : MonoBehaviour
{
    public Image cocktailSlot;

    void Start()
    {
        Sprite savedSprite=CocktailSaver.Instance.GetCocktailImage();
        if(savedSprite!=null)
        {
            cocktailSlot.sprite= savedSprite;
        }
        else
        {
            Debug.Log("실패");
        }
    }
}


