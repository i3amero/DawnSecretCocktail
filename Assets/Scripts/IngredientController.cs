// OnIngredientClicked에서 out키워드 있는 이유를 잘 모르겠습니다...?
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 이건 취향 차이인데, dialogText같은 텍스트들은 null일때, setAvtive(false)보단,
/// dialogText = "" 이런식으로 초기화 하는게 더 좋을 것 같아요. 왜냐면 얘가 꺼져있는데 검색할 때가 생길 수는데, 
/// 비활성상태에서는 검색이 안되어서..
/// </summary>
public class IngredientController : MonoBehaviour
{
    public SlotController slotController;
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;
    public float autoHideTime = 5f;

    public void OnIngredientClicked(Sprite ingredientSprite)
    {
        if (dialogPanel.activeSelf)
        {
            return;
        }
        string failMessage;
        if (!slotController.AddToSlot(ingredientSprite, out failMessage))
        {
            dialogText.text = failMessage;
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
