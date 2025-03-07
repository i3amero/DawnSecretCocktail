using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterButtonController : MonoBehaviour
{
    public Button[] characterButtons;

    void Start()
    {
        for (int i = 0; i < characterButtons.Length; i++)
        {
            int isDisabled = PlayerPrefs.GetInt("CharacterDisabled_" + i, 0);
            if (isDisabled == 1)
            {
                characterButtons[i].interactable = false; // 선택한 캐릭터 버튼 비활성화
            }
        }
    }
    public void SelectCharacter(int characterIndex)
    {
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
        PlayerPrefs.SetInt("CharacterDisabled_" + characterIndex, 1);
        PlayerPrefs.Save();
    }
    public void ResetCharacterButtons()
    {
        for (int i = 0; i < characterButtons.Length; i++)
        {
            PlayerPrefs.SetInt("CharacterDisabled_" + i, 0);
            characterButtons[i].interactable = true;
        }
        PlayerPrefs.Save();
    }
}
