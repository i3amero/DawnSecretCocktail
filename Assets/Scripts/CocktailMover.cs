using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CocktailMover : MonoBehaviour
{
    public Image cocktailSlot;

    void Start()
    {
        GameObject preservedGameObject = GameObject.Find("cocktailImage");
        if (preservedGameObject != null)
        {
            Debug.Log("cocktailImage 찾음");

            Image preservedImage = preservedGameObject.GetComponent<Image>();
            cocktailSlot.sprite = preservedImage.sprite;
        }
        else
        {
            Debug.LogWarning("cocktailImage 못찾음.");
        }
    }
}


