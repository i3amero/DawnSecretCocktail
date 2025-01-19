using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CocktailManager : MonoBehaviour
{
    public SlotManager slotManager; // 슬롯에서 재료를 가져오는 관리 스크립트
    public GameObject popupPanel; // 결과를 표시할 팝업 패널
    public Text popupMessage; // 팝업 메시지 텍스트
    public Image cocktailImage; // 결과 이미지를 표시할 UI 이미지

    // Inspector에서 연결할 Sprite
    public Sprite 트와일라잇;
    public Sprite 엘릭시르;
    public Sprite 아방가르드;
    public Sprite 사해;
    public Sprite 위니스트;
    public Sprite 홀리워터;
    public Sprite 조합실패; // 실패 시 표시할 기본 이미지 (선택 사항)

    private Dictionary<string, (string name, Sprite image)> recipes; // 레시피 데이터

    public void Awake()
    {
        InitializeRecipes();
    }

    private void InitializeRecipes()
    {
        // Sprite를 Inspector에서 직접 연결
        recipes = new Dictionary<string, (string name, Sprite image)>
        {
            { "스위트 허니문_0,스칼렛 위고_0", ("트와일라잇", 트와일라잇) },
            { "고블린 갱_0,보라달빛_0", ("엘릭시르", 엘릭시르) },
            { "보라달빛_0,스위트 허니문_0,찌리리리_0", ("아방가르드", 아방가르드) },
            { "데스 메디슨_0,티어즈 오브 더 씨_0", ("사해", 사해) },
            { "보라달빛_0,스칼렛 위고_0,티어즈 오브 더 씨_0", ("위니스트", 위니스트) },
            { "고블린 갱_0,데스 메디슨_0,티어즈 오브 더 씨_0", ("홀리 워터", 홀리워터) }
        };
    }

    public void MakeCocktail()
    {
        string[] ingredients = slotManager.GetSlotIngredients();
        System.Array.Sort(ingredients); 
        string recipeKey = string.Join(",", ingredients); 
        Debug.Log($"슬롯에서 반환된 재료: {string.Join(", ", ingredients)}");
        //Debug.Log($"생성된 Key: {recipeKey}");
        // 레시피 확인
        if (recipes.ContainsKey(recipeKey))
        {
            var recipe = recipes[recipeKey];
            ShowPopup(true, recipe.name, recipe.image);
        }
        else
        {
            // 실패 시 기본 실패 이미지와 메시지 표시
            ShowPopup(false, "조합에 실패하였습니다...", 조합실패);
        }
    }

    private void ShowPopup(bool success, string message, Sprite image)
    {
        popupPanel.SetActive(true);

        if (success)
        {
            popupMessage.text = $"{message} 조합 성공!";
            if (image != null)
            {
                cocktailImage.gameObject.SetActive(true);
                cocktailImage.sprite = image;
            }
            else
            {
                cocktailImage.gameObject.SetActive(false);
            }
        }
        else
        {
            popupMessage.text = message;
            if (image != null)
            {
                cocktailImage.gameObject.SetActive(true);
                cocktailImage.sprite = image;
            }
            else
            {
                cocktailImage.gameObject.SetActive(false);
            }
        }
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
        popupMessage.text = ""; // 메시지 초기화
        cocktailImage.sprite = null; // 이미지 초기화
    }
}