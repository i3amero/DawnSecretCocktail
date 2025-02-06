using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CocktailSaver : MonoBehaviour
{
   public static CocktailSaver Instance { get; private set; }
   private Sprite cocktailSprite;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCocktailImage(Sprite sprite)
    {
        cocktailSprite = sprite;
    }
    public Sprite GetCocktailImage()
    {
        return cocktailSprite;
    }
}
