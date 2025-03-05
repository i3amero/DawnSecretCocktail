using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// GetScenarioName랑 GetCleanCharacterName왜 여기도 있나요...? 불러오는게 종속성이 높아져서 찝집하면 
/// GetScenarioName랑 GetCleanCharacterName를 포괄하는 클래스를 만들어서 부모 클래스로 만드는 방법도 있을 것 같아요.
/// </summary>
public class ScenarioController : MonoBehaviour
{
    public static ScenarioController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        LoadUnlockedScenarios();
    }

    public bool CheckMatch(string characterSpriteName, string cocktailName)
    {
        string cleanCharacterName = GetCleanCharacterName(characterSpriteName);
        return (GetAllowedCocktails(cleanCharacterName) & GetCocktailByName(cocktailName)) != 0;
    }

    public void UnlockScenario(string characterSpriteName)
    {
        string cleanCharacterName = GetCleanCharacterName(characterSpriteName);
        string episodeName = GetScenarioName(cleanCharacterName);

        PlayerPrefs.SetInt($"Scenario_{cleanCharacterName}", 1);
        PlayerPrefs.Save();
    }

    public bool IsScenarioUnlocked(string characterSpriteName)
    {
        string cleanCharacterName = GetCleanCharacterName(characterSpriteName);

        return PlayerPrefs.GetInt($"Scenario_{cleanCharacterName}", 0) == 1;
    }

    public string GetScenarioName(string characterName)
    {
        switch (characterName)
        {
            case "레조나": return "그대에게 바치는 비탄의 교향곡";
            case "데드리프트": return "방황하는 머리 잘린 괴수";
            case "핀투라": return "칠흑보다 어두운 검은 안개";
            case "카타르시스": return "영원히 끝나지 않는 커튼";
            case "베네딕트": return "괴이";
            default: return "기본 에피소드";
        }
    }

    private void LoadUnlockedScenarios()
    {
        string[] allCharacters = { "레조나", "데드리프트", "핀투라", "카타르시스", "베네딕트" };

        foreach (string character in allCharacters)
        {
            if (IsScenarioUnlocked(character))
            {
                string characterscenario = GetScenarioName(character);
                Debug.Log($"{character}의 시나리오 {characterscenario}가 해제됨!");
            }
            else
            {
                Debug.Log($"{character}의 시나리오는 잠겨 있음.");
            }
        }
    }

    private int GetAllowedCocktails(string characterName)
    {
        switch (characterName)
        {
            case "레조나": return 1; 
            case "데드리프트": return 2;
            case "핀투라": return 4;
            case "카타르시스": return 8;
            case "베네딕트": return 16;
            default: return 0;
        }
    }

    private int GetCocktailByName(string name)
    {
        switch (name)
        {
            case "트와일라잇": return 1;
            case "엘릭시르": return 2;
            case "아방가르드": return 4;
            case "사해": return 8;
            case "위니스트": return 16;
            default: return 0;
        }
    }
    private string GetCleanCharacterName(string characterSpriteName)
    {
        return characterSpriteName.Split('_')[0];
    }
}


