using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ShopButton : MonoBehaviour
{
    public GameObject purchasePanel;
    public TMP_Text purchaseMessage;
    public Button yesButton;
    public Button noButton;
    public Image[] characterImages;
    public TMP_Text[] characterMessages;
    public Button[] optionButtons;

    private string selectedCharacter = "";
    private int selectedIndex = -1;

    private Dictionary<string, string> characterScenarios = new Dictionary<string, string>()
    {
        { "레조나", "그대에게 바치는 비탄의 교향곡" },
        { "데드리프트", "방황하는 머리 없는 괴물" },
        { "핀투라", "칠흑보다 어두운 검은 안개" },
        { "카타르시스", "영원히 끝나지 않는 커튼콜" }
    };

    private void Start()
    {
        LoadScenarioStates();
    }

    private void LoadScenarioStates()
    {
        for (int i = 0; i < characterImages.Length; i++)
        {
            string characterName = characterImages[i].gameObject.name;
            characterName = characterName.Split('_')[0];
            Debug.Log(characterName);
            bool isUnlocked = PlayerPrefs.GetInt("Scenario_" + characterName, 0) == 1;

            if (isUnlocked)
            {
                characterImages[i].color = Color.black; 
                characterMessages[i].text = "구매 완료";
                characterMessages[i].color = Color.red;
                optionButtons[i].interactable = false; 
            }
            else
            {
                if (characterScenarios.ContainsKey(characterName))
                {
                    characterMessages[i].text = characterScenarios[characterName];
                }
                else
                {
                    characterMessages[i].text = "알 수 없음";
                }

                characterMessages[i].color = Color.white;
                optionButtons[i].interactable = true; 
            }
        }
    }

    public void OnScenarioButtonClicked(int index)
    {
        selectedIndex = index;
        selectedCharacter = characterMessages[index].gameObject.name;
        bool isUnlocked = PlayerPrefs.GetInt("Scenario_" + selectedCharacter, 0) == 1;
        if(purchasePanel.activeSelf)
        {
            return;
        }

        if (isUnlocked)
        {
            return;
        }

        purchaseMessage.text = "이 에피소드를 구매하시겠습니까?";
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(PurchaseScenario);
        purchasePanel.SetActive(true);
    }

    public void PurchaseScenario()
    {
        if (selectedCharacter != "" && selectedIndex != -1)
        {
            PlayerPrefs.SetInt("Scenario_" + selectedCharacter, 1);
            PlayerPrefs.Save();

            characterImages[selectedIndex].color = Color.black;
            characterMessages[selectedIndex].text = "구매 완료";
            characterMessages[selectedIndex].color = Color.red;
            optionButtons[selectedIndex].interactable = false; 
            purchasePanel.SetActive(false);
        }
    }

    public void HideDialog()
    {
        purchasePanel.SetActive(false);
    }
}
