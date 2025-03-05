using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class ScenarioButtonManager : MonoBehaviour
{
    [Header("Popup UI Elements")]
    public GameObject panel; 
    public TMP_Text panelTitle; 
    public Button optionButton1;
    public Button optionButton2;
    public TMP_Text optionButtonText1;
    public TMP_Text optionButtonText2;

    private string scene1;
    private string scene2;

    private Dictionary<string, string[]> characterData = new Dictionary<string, string[]>()
    {
        { "레조나", new string[] { "그대에게 바치는 비탄의 교향곡", "레조나 칵테일 에피소드", "LezonaNightScene", "LezonaCocktailScene" } },
        { "데드리프트", new string[] { "방황하는 머리 잘린 괴수", "데드리프트 칵테일 에피소드", "DeadliftNightScene", "DeadliftCocktailScene" } },
        { "핀투라", new string[] { "칠흑보다 어두운 검은 안개", "핀투라 칵테일 에피소드", "PinturaNightScene", "PinturaCocktailScene" } },
        { "카타르시스", new string[] { "영원히 끝나지 않는 커튼", "카타르시스 칵테일 에피소드", "KatarsisNightScene", "KatarsisCocktailScene" } }
    };

    public void ShowPanel(string characterName)
    {
        if (panel.activeSelf)
        {
            return;
        }

        if (characterData.TryGetValue(characterName, out string[] data))
        {
            panelTitle.text = $"{characterName} 에피소드"; 
            optionButtonText1.text = data[0];
            optionButtonText2.text = data[1];
            scene1 = data[2];
            scene2 = data[3];

            
            bool isUnlocked = PlayerPrefs.GetInt($"Scenario_{characterName}", 0) == 1;

           
            optionButton1.interactable = isUnlocked;
            optionButton2.interactable = isUnlocked;

            
            optionButton1.image.color = isUnlocked ? Color.white : new Color(1, 1, 1, 0.5f);
            optionButton2.image.color = isUnlocked ? Color.white : new Color(1, 1, 1, 0.5f);

            panel.SetActive(true); 
        }
        else
        {
            Debug.LogError($"[CharacterPanelManager] {characterName}에 대한 데이터가 없습니다.");
        }
    }

    public void LoadScene1()
    {
        if (!string.IsNullOrEmpty(scene1))
        {
            SceneManager.LoadScene(scene1);
          
        }
    }

    public void LoadScene2()
    {
        if (!string.IsNullOrEmpty(scene2))
        {
            SceneManager.LoadScene(scene2);
            
        }
    }

    public void HideDialog()
    {
        panel.SetActive(false);
    }
}
