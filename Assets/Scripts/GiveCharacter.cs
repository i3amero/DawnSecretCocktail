using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GiveCharacter : MonoBehaviour
{
    public Sprite characterSprite;
    public GameObject popupPanel;
    public TMP_Text popupText;
    public TMP_Text resultText; 
    public Button confirmButton;
    public Button cancelButton;
    public Button closeButton;
    public Image cocktailImageSlot;

    private string characterSpriteName;
    private string displayCharacterName;
    private string currentCocktailName;

    private void Start()
    {
        popupPanel.SetActive(false);
        closeButton.gameObject.SetActive(false);
        resultText.gameObject.SetActive(false);

        characterSpriteName = characterSprite.name;
        displayCharacterName = GetCleanName(characterSpriteName);
    }

    private string GetCurrentCocktailName()
    {
        if (cocktailImageSlot.sprite != null)
        {
            return GetCleanName(cocktailImageSlot.sprite.name);
        }
        return "None";
    }

    public void OnCharacterButtonClicked()
    {
        currentCocktailName = GetCurrentCocktailName();
        string episodeName = ScenarioController.Instance.GetScenarioName(displayCharacterName);

        popupText.text = $"{displayCharacterName}에게 {currentCocktailName}을(를) 주시겠습니까?";
        resultText.text = "";
        popupPanel.SetActive(true);

        confirmButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);
        resultText.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(ConfirmGiveCocktail);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => popupPanel.SetActive(false));
    }

    private void ConfirmGiveCocktail()
    {
        confirmButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);

        if (ScenarioController.Instance.CheckMatch(characterSpriteName, currentCocktailName))
        {
            string episodeName = ScenarioController.Instance.GetScenarioName(displayCharacterName);

            ScenarioController.Instance.UnlockScenario(characterSpriteName);
            resultText.text = $"{episodeName}이(가) 해제되었습니다!";
            resultText.color = Color.green;
        }
        else
        {
            resultText.text = $"아무일도 일어나지 않음.";
            resultText.color = Color.red;
        }

        popupText.gameObject.SetActive(false);
        resultText.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(LoadNextScene);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("CocktailMain");
    }

    private string GetCleanName(string name)
    {
        return name.Split('_')[0];
    }
}
