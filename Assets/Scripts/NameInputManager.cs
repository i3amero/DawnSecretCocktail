using UnityEngine;
using TMPro;

public class NameInputManager : MonoBehaviour
{
    public TMP_InputField nameInputField;  // 플레이어 이름 입력 필드
    public GameObject inputPanel;          // 입력 패널 (UI)

    public DialogueManager dialogueManager;

    // 입력창 띄우기
    public void ShowInputPanel()
    {
        inputPanel.SetActive(true);
    }

    // 플레이어 이름 저장 및 입력창 닫기
    public void SetPlayerName()
    {
        string inputName = nameInputField.text.Trim();
        if (!string.IsNullOrEmpty(inputName))
        {
            PlayerData.Instance.playerName = inputName;
        }
        inputPanel.SetActive(false); // 입력 창 닫기
        dialogueManager.ContinueDialogue(); // 입력 후 대화 계속 진행
    }
}