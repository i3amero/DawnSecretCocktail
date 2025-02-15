using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;
    public float autoHideTime = 3f;
    string message = "";

    public Image[] slots;
    [SerializeField]private Sprite defaultSprite;

    void Start()
    {
        dialogPanel.SetActive(false);
    }

    public bool AddToSlot(Sprite ingredientSprite, out string failMessage)
    {
        failMessage = "";

        // 이미 슬롯에 존재하는 재료인지 확인
        foreach (var slot in slots)
        {
            if (slot.sprite == ingredientSprite)
            {
                failMessage = "이미 선택된 재료라네.";
                return false;
            }
        }

        // 빈 슬롯을 찾아 재료 추가
        foreach (var slot in slots)
        {
            if (slot.sprite == defaultSprite)
            {
                slot.sprite = ingredientSprite;

                switch (ingredientSprite.name)
                {
                    case "보라달빛_0":
                        message = "보라달빛, 은은한 과일향이 나는 리큐르라네.\n 이른 저녁의 향이 느껴진다 하여 보라달빛이라는 이름이 생겼지.";
                        break;
                    case "스위트 허니문_0":
                        message = "스위트 허니문, 이름만큼이나 달콤한 맛이 일품인 시럽이지.\n 자기 주장이 매우 강하기에 조금만 넣어도 예민한 사람은 바로 눈치를 챌 정도라네.";
                        break;
                    case "스칼렛 위고_0":
                        message = "이 와인은 르노타시아에서 한 때 유명했던 괴담에서 등장하는 흡혈귀의 이름을 따서 지어졌다네.\n 첫 맛은 달콤하고, 중간 맛은 묵직하며, 끝 맛은 씁쓸한 세 경험을 동시에 할 수 있는 고급 와인이지.";
                        break;
                    case "찌리리리_0":
                        message = "르노타시아에서 굉장히 인기를 끌고 있는 칵테일용 레몬즙이라네.\n 유치한 이름과는 다르게 전기에 감전된 것만 같은 강렬한 신 맛이 일품이지.";
                        break;
                    case "티어즈 오브 더 씨_0":
                        message = "바다의 눈물이라는 이름이 잘 어울리는 짙은 남색 빛깔이 도는 논알콜 위스키라네. 씁쓸하지만 그다지 불쾌하지 않은 어른의 맛이 난다고들 하지.";
                        break;
                    case "고블린 갱_1":
                        message = "강렬한 이름과 어울리는 강렬한 맛을 가진 보드카라네. \n 분명 논알콜임에도 마치 100도의 불을 목에 넣는 것만 같은 화끈함이 특징이지.";
                        break;
                    case "데스 메디슨_0":
                        message = "에스프레소 농축액을 이용해 만든 리큐르라네.\n 죽음의 약이라는 이름처럼 희석하지 않으면 혀 끝만 대도 온 몸에 소름이 돋을 정도로 쓴 맛이 난다고들 하지.";
                        break;
                    default:
                        message = "이 재료는 특별한 설명이 없군요!";
                        break;
                }

                dialogText.text = message;
                dialogPanel.SetActive(true);
                CancelInvoke(nameof(HideDialog));
                Invoke(nameof(HideDialog), autoHideTime);

                return true;
            }
        }
        failMessage = "선택 가능한 칵테일이 가득 찼다네.";
        return false;
    }


    public void HideDialog()
    {
        dialogPanel.SetActive(false);
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
