using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    public TMP_Text popupMessage; 
    public Image cocktailImage; 

    
    public Sprite 트와일라잇;
    public Sprite 엘릭시르;
    public Sprite 아방가르드;
    public Sprite 사해;
    public Sprite 위니스트;
    public Sprite 홀리워터;
    public Sprite 조합실패;
    public GameObject dialogPanel;
    public TMP_Text closetext;

    public void MakeCocktail()
    {
        if (dialogPanel.activeSelf)
        {
            return;
        }

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
                ShowPopup(true, "트와일라잇 완성!\n\n 첫 맛은 달콥쌉싸름한 첫사랑의 향이, 끝맛은 진주빛의 황혼을 연상케 하는 와인 칵테일입니다.\n 호불호가 거의 없는 맛이기에 칵테일을 처음 접하는 분들에게 무난하게 추천됩니다.", 트와일라잇);
                closetext.text = "제공";
                break;
            case CocktailResult.Elixir:
                ShowPopup(true, "엘릭시르 조합 성공!\n\n 이름이 왜 엘릭시르인지 모르겠습니다만, 아무튼 그렇습니다. \n물론 회복 효과는 절대 기대하시면 안됩니다! 대신 우왁스러운 맛과 향으로 한 모금만 맛봐도 바로 정신이 들게 해줄 수는 있죠.\n 손님의 목 뒤 장기 하나하나의 경로를 선명하게 체감할 수 있는 기회는 보너스고요.", 엘릭시르);
                closetext.text = "제공";
                break;
            case CocktailResult.AvantGarde:
                ShowPopup(true, "아방가르드 조합 성공!\n\n 무지개를 형상화한 것만 같은 맛이 나는 이 칵테일은, 예술가들이 새벽에 아이디어를 떠올릴 때의 동반자로 마시고는 하는 녀석입니다.\n 다만 예술가 픽이 다 그렇듯이, 호불호가 심하게 갈리는 맛이기에 초보들에게는 비추천되는 메뉴입니다.", 아방가르드);
                closetext.text = "제공";
                break;
            case CocktailResult.DeadSea:
                ShowPopup(true, "사해 조합 성공!\n\n 대체 이런 조합을 누가 처음 시도했을까요?\n 비주얼적으로나, 맛으로나 이 세상에 존재해서는 안 되는 레시피입니다만 놀랍게도 이미 판매가 이뤄지고 있고 마니아층도 있다고 하네요.\n 애호가들에 따르면 모든 것을 깨달은 어른의 맛이라나 뭐라나. 한 잔(200ml)당 고카페인 함유 320mg으로, 그날 잠은 다 잘수 있습니다.", 사해);
                closetext.text = "제공";
                break;
            case CocktailResult.Winist:
                ShowPopup(true, "위니스트 조합 성공!\n\n 어라, 이 메뉴를 알고 계셨나요? 우리 스피크이지의 특별 메뉴입니다!\n 한 잔만 마셔도 모든 피로가 해소되고 정신이 맑아지는 효능이 있습니다. (MSG 2% 첨가됨)", 위니스트);
                closetext.text = "제공";
                break;
            case CocktailResult.HolyWater:
                ShowPopup(true, "홀리 워터 조합 성공!\n\n 이럴수가! 괴랄한 맛의 대표주자 3인방을 넣고 섞었더니 모든 것의 시작으로 돌아가버렸습니다! \n도대체 이게 어떻게 가능한거죠? 튜닝의 끝은 역시 순정인건가요?! 물론 맛도 일반적인 물입니다!", 홀리워터);
                closetext.text = "제공";
                break;
            default:
                ShowPopup(false, "제조 실패\n\n 아무리 스피크이지가 특별한 칵테일을 취급한다지만, 이건 절대로 마실 수 없겠는걸요.\n 천하의 바텐더 베네딕트도 가끔씩은 헷갈리는 법입니다. \n다른 조합을 시도해봅시다.", 조합실패);
                closetext.text = "닫기";
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
            popupMessage.text = $"{message}";
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
            popupMessage.text = $"{message}";
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
        if(cocktailImage.sprite == 조합실패)
        {
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            SceneManager.LoadScene("CocktailGive");
        }
    }
}