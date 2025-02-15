using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Flags]
public enum IngredientMask
{
    None = 0,
    SweetHoneyMoon = 1 << 0,
    ScarletWego = 1 << 1,
    GoblinGang = 1 << 2,
    PurpleMoonLight = 1<< 3,
    ThunderBolt = 1<< 4,
    DeathMedicine = 1<< 5,
    TearsOfTheSea = 1<< 6
}
public enum CocktailResult
{
    없음,
    Twilight,
    Elixir,
    AvantGarde,
    DeadSea,
    Winist,
    HolyWater,
    MakeFail
}

public class CocktailControllerer : MonoBehaviour
{
    public SlotController slotController; 
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

    public void MakeCocktail()
    {
        string[] ingredients = slotController.GetSlotIngredients();

        IngredientMask combinedMask = IngredientMask.None;
        foreach(string ingredient in ingredients)
        {
            combinedMask |= IngredientStringToMask(ingredient);
        }

        CocktailResult result = GetCocktailResult(combinedMask);

        switch(result)
        {
            case CocktailResult.Twilight:
                ShowPopup(true, "트와일라잇 조합 성공!", 트와일라잇);
                break;
            case CocktailResult.Elixir:
                ShowPopup(true, "엘릭시르 조합 성공!", 엘릭시르);
                break;
            case CocktailResult.AvantGarde:
                ShowPopup(true, "아방가르드 조합 성공!", 아방가르드);
                break;
            case CocktailResult.DeadSea:
                ShowPopup(true, "사해 조합 성공!", 사해);
                break;
            case CocktailResult.Winist:
                ShowPopup(true, "위니스트 조합 성공!", 위니스트);
                break;
            case CocktailResult.HolyWater:
                ShowPopup(true, "홀리 워터 조합 성공!", 홀리워터);
                break;
            default:
                ShowPopup(false, "조합 실패...", 조합실패);
                break;
        }
    }

    private CocktailResult GetCocktailResult(IngredientMask combinedMask)
    {
        if (combinedMask == (IngredientMask.SweetHoneyMoon | IngredientMask.ScarletWego))
            return CocktailResult.Twilight;
        if (combinedMask == (IngredientMask.GoblinGang | IngredientMask.PurpleMoonLight))
            return CocktailResult.Elixir;
        if (combinedMask == (IngredientMask.ThunderBolt | IngredientMask.PurpleMoonLight | IngredientMask.SweetHoneyMoon))
            return CocktailResult.AvantGarde;
        if (combinedMask == (IngredientMask.DeathMedicine | IngredientMask.TearsOfTheSea))
            return CocktailResult.DeadSea;
        if (combinedMask == (IngredientMask.TearsOfTheSea | IngredientMask.ScarletWego | IngredientMask.PurpleMoonLight))
            return CocktailResult.Winist;
        if (combinedMask == (IngredientMask.GoblinGang | IngredientMask.TearsOfTheSea | IngredientMask.DeathMedicine))
            return CocktailResult.HolyWater;


        return CocktailResult.MakeFail;
    }

    private IngredientMask IngredientStringToMask(string ingredient)
    {
        return ingredient switch
        {
            "스위트 허니문_0" => IngredientMask.SweetHoneyMoon,
            "스칼렛 위고_0" => IngredientMask.ScarletWego,
            "고블린 갱_1" => IngredientMask.GoblinGang,
            "보라달빛_0" => IngredientMask.PurpleMoonLight,
            "찌리리리_0" => IngredientMask.ThunderBolt,
            "데스 메디슨_0" => IngredientMask.DeathMedicine,
            "티어즈 오브 더 씨_0" => IngredientMask.TearsOfTheSea,
            _ => IngredientMask.None
        };
    }
    private void ShowPopup(bool success, string message, Sprite image)
    {
        popupPanel.SetActive(true);

        if (success)
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

        if(cocktailImage != null)
        {
            if(CocktailSaver.Instance != null)
            {
                CocktailSaver.Instance.SetCocktailImage(cocktailImage.sprite);
            }
            else
            {
                Debug.Log("칵테일세이버.인스턴스가 null");
            }
        }
        else
        {
            Debug.Log("이미지가 null");
        }
        //CocktailSaver.Instance.SetCocktailImage(cocktailImage.sprite);

        SceneManager.LoadScene("CharacterScene");
    }
}