using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class ShopButton : MonoBehaviour
{
    public GameObject purchasePanel;
    public GameObject failPanel;
    public TMP_Text purchaseMessage;
    public Button yesButton;
    public Button noButton;
    public Image[] characterImages;
    public TMP_Text[] characterMessages;
    public Button[] optionButtons;
    public TMP_Text Leftenergy;  //보유 재화 가져올 변수입니다.

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
        int energy = GameManager.Instance.unidentifiedEnergy;
        Leftenergy.text=($"에너지: {energy}"); //에너지 텍스트필드에 출력되는 메세지 입니다.
        LoadScenarioStates();
        for (int i = 0; i < optionButtons.Length; i++)
        {
            Debug.Log($"Button {i}: interactable = {optionButtons[i].interactable}");
        }
    }

    private void LoadScenarioStates()
    {
        for (int i = 0; i < characterImages.Length; i++)
        {
            string characterName = characterImages[i].gameObject.name;
            characterName = characterName.Split('_')[0];
            bool isUnlocked = PlayerPrefs.GetInt($"NightScenario_{characterName}", 0) == 1;

            if (isUnlocked)
            {
                characterImages[i].color = Color.black; 
                characterMessages[i].text = "구매 완료";
                characterMessages[i].color = Color.red;
                optionButtons[i].interactable = false; 
            }
            else
            {
                Debug.Log("해제안됨");
                if (characterScenarios.ContainsKey(characterName))
                {
                    characterMessages[i].text = ($"<5000 에너지 필요> \n{characterScenarios[characterName]}");
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
        string characterName = characterImages[index].gameObject.name;
        characterName = characterName.Split('_')[0];
        bool isUnlocked = PlayerPrefs.GetInt($"NightScenario_{characterName}", 0) == 1;
        if (purchasePanel.activeSelf)
        {
            return;
        }


        selectedIndex = index;
        selectedCharacter = characterMessages[index].gameObject.name;
        purchasePanel.SetActive(true);
        purchaseMessage.text = "이 에피소드를 구매하시겠습니까?";
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(PurchaseScenario);
    }

    public void PurchaseScenario()
    {
        int energy = GameManager.Instance.unidentifiedEnergy;
        if (selectedCharacter != "" && selectedIndex != -1)
        {
            if (energy >= 5000)
            {
                GameManager.Instance.unidentifiedEnergy -= 5000;
                string characterName = characterImages[selectedIndex].gameObject.name;
                characterName = characterName.Split('_')[0];
                PlayerPrefs.SetInt($"NightScenario_{characterName}", 1);
                PlayerPrefs.Save();

                characterImages[selectedIndex].color = Color.black;
                characterMessages[selectedIndex].text = "구매 완료";
                characterMessages[selectedIndex].color = Color.red;
                optionButtons[selectedIndex].interactable = false;
                purchasePanel.SetActive(false);
            }
            else
            {
                purchasePanel.SetActive(false);
                failPanel.SetActive(true);
            }
            
        }
    }

    public void HidePurchaseDialog()
    {
        purchasePanel.SetActive(false);
    }
    public void HideFailDialog()
    {
        failPanel.SetActive(false);
    }    
}
