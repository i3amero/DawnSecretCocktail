using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel; // 패널
    public TMP_Text dialogueText;    // 대사를 표시할 Text 컴포넌트

    // 대화창이 닫힐 때 실행할 콜백
    private System.Action onCloseCallback;

    // 대화창이 열려있으면 true, 닫혀있으면 false를 반환
    public bool IsDialogueActive => dialoguePanel != null && dialoguePanel.activeSelf;

    private void Start()
    {
        // 처음에는 패널 숨기기
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (dialoguePanel != null)
        {
            if (dialoguePanel.activeSelf)
            {
                // 화면 클릭 또는 스페이스바 입력 시 대화창 닫기
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    CloseDialogue();
                }
            }
        }
    }

    // 모든 내용을 한 번에 표시
    public void ShowFullDialogue(string message, System.Action onClose = null)
    {
        if (dialogueText != null)
        {
            dialogueText.text = message;
        }

        // 대사창 보여주기
        if(dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        // 콜백을 저장해둠
        onCloseCallback = onClose;
    }

    // 닫기 버튼
    private void CloseDialogue()
    {
        // 대화창 닫기 전에 텍스트를 초기화합니다.
        if (dialogueText != null)
        {
            dialogueText.text = "";
        }

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        // 대화창이 닫히는 시점에 콜백 호출
        onCloseCallback?.Invoke();
    }
}
