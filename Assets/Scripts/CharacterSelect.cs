using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    public void SelectCharacter(int characterIndex)
    {
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
        SceneManager.LoadScene("TalkScene");
    }
}
