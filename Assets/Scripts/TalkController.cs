using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TalkController : MonoBehaviour
{
    [System.Serializable]
    public class CharacterData
    {
        public string characterName;
        public Sprite characterSprite;
        [TextArea(3, 5)]
        public string[] dialogLines;
    }

    public CharacterData[] characters;
    public Image characterImage;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI characterNameText;
    public GameObject dialogPanel;
    public Button nextButton;

    private int currentDialogIndex = 0;
    private CharacterData currentCharacter;

    void Start()
    {
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        currentCharacter = characters[selectedCharacterIndex];

        characterImage.sprite = currentCharacter.characterSprite;
        characterNameText.text = currentCharacter.characterName;
        currentDialogIndex = 0;
        dialogText.text = currentCharacter.dialogLines[currentDialogIndex];

        dialogPanel.SetActive(true);
        nextButton.onClick.AddListener(NextDialog);
    }

    public void NextDialog()
    {
        currentDialogIndex++;

        if (currentDialogIndex < currentCharacter.dialogLines.Length)
        {
            if(currentCharacter.characterName=="데드리프트")
            {
                if (currentDialogIndex == 2 || currentDialogIndex==7)
                {
                    characterNameText.text = "주인공";
                }
                else
                {
                    characterNameText.text = "데드리프트";
                }
            }
            else if (currentCharacter.characterName == "카타르시스")
            {
                if (currentDialogIndex == 2 || currentDialogIndex == 6)
                {
                    characterNameText.text = "주인공";
                }
                else
                {
                    characterNameText.text = "카타르시스";
                }
            }
            else if (currentCharacter.characterName == "레조나")
            {
                if (currentDialogIndex == 2 || currentDialogIndex == 7)
                {
                    characterNameText.text = "주인공";
                }
                else
                {
                    characterNameText.text = "레조나";
                }
            }
            else if (currentCharacter.characterName == "핀투라")
            {
                if (currentDialogIndex == 2 || currentDialogIndex == 7)
                {
                    characterNameText.text = "주인공";
                }
                else
                {
                    characterNameText.text = "핀투라";
                }
            }
            dialogText.text = currentCharacter.dialogLines[currentDialogIndex];
        }
        else
        {
            SceneManager.LoadScene("CocktailMain");
        }
    }
}
