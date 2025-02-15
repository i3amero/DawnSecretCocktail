using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocktailButton : MonoBehaviour
{
    public CocktailPopup cocktailpopup;

    public void OnCharacterButtonClicked()
    {
        cocktailpopup.ShowPopup();
    }
}
