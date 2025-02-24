using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
/// <summary>
/// 정보와 관련된 변수들은 클래스에 할당해서 캡슐화 하는게 좋을 거 같습니다.
/// OnCharacterButtonClicked()/ConfirmGiveCocktail()=>이름에 비해 역할이 너무 많아요. 이름을 적절히 바꾸거나, 역할을 나눠주는게 좋을 거 같아요.
/// LoadNextScene()=>역할이 상당히 구체적이라, LoadCocktailMainScene()으로 바꾸는게 좋을 거 같아요. 이름은 항상 직관적으로!
/// </summary>
//아래 클래스(경우에 따라선 struct)에 정보 저장해서 캡슐화해 관리하기! 전역이면 그냥 static쓰면 됩니다.
public class CharacterInfo
{
    //기타 정보들...
}
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
