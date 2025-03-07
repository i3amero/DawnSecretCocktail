using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel; // 패널
    public TMP_Text dialogueText;    // 대사를 표시할 Text 컴포넌트
    public UIFadeEffect uiFadeEffect; // 대화창에 부착된 페이드 효과 스크립트

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

        if (uiFadeEffect != null)
        {
            uiFadeEffect.StartFadeIn();
        }

        // 콜백을 저장해둠
        onCloseCallback = onClose;
    }

    // 닫기 버튼
    // 대화창 닫기 호출 시
    private void CloseDialogue()
    {
        if (uiFadeEffect != null)
        {
            // 페이드 아웃 효과 후 대화창 비활성화 처리
            StartCoroutine(CloseDialogueWithFade());
        }
        else
        {
            // 페이드 효과 없이 즉시 닫기
            HideDialogue();
        }
    }

    private IEnumerator CloseDialogueWithFade()
    {
        uiFadeEffect.StartFadeOut();
        // 페이드 효과가 진행되는 동안 대기 (fadeDuration 만큼)
        yield return new WaitForSeconds(uiFadeEffect.fadeDuration);

        HideDialogue();
    }

    private void HideDialogue()
    {
        if (dialogueText != null)
        {
            dialogueText.text = "";
        }
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        onCloseCallback?.Invoke();
    }
}
