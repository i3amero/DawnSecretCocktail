using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class IngredientController : MonoBehaviour
{
    public SlotController slotController;
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;
    public float autoHideTime = 3f;

    public void OnIngredientClicked(Sprite ingredientSprite)
    {
        if(!slotController.AddToSlot(ingredientSprite))
        {
            dialogText.text = "슬롯이 가득 찼다네.";
            dialogPanel.SetActive(true);
            CancelInvoke(nameof(HideDialog));
            Invoke(nameof(HideDialog), autoHideTime);
        }
    }
    public void HideDialog()
    {
        dialogPanel.SetActive(false);
    }
}
