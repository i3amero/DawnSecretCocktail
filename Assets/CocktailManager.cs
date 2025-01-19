using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CocktailManager : MonoBehaviour
{
    public SlotManager slotManager; 
    public GameObject popupPanel; 
    public Text popupMessage; 
    public Image cocktailImage; 

    
    public Sprite 트와일라잇;
    public Sprite 엘릭시르;
    public Sprite 아방가르드;
    public Sprite 사해;
    public Sprite 위니스트;
    public Sprite 홀리워터;
    public Sprite 조합실패;

    private Dictionary<string, (string name, Sprite image)> recipes; 

    public void Awake()
    {
        InitializeRecipes();
    }

    private void InitializeRecipes()
    {
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
        if (recipes.ContainsKey(recipeKey))
        {
            var recipe = recipes[recipeKey];
            ShowPopup(true, recipe.name, recipe.image);
        }
        else
        {
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
        popupMessage.text = ""; 
        cocktailImage.sprite = null; 
    }
}