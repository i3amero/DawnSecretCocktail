using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// LoadNextScene()같이 같은 이름의 메소드들이 너무 많아요... 나중에 꼬일 수 있으니까, 이름을 바꿔주는 게 좋을 것 같아요.
/// ShowDialogue()에 너무 많은 기능들이 있어요. 이 기능들을 다른 메소드로 분리해주는 게 좋을 것 같아요.
/// 현준이도 저정도 분량이면 슬슬 뭐가 뭔지 헷갈릴 것 같은 뎀?
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public string nextSceneName;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public CSVReader csvReader;
    public NameInputManager nameInputManager;
    public Image illustImage;

    private int currentIndex = 1;

    void Start()
    {
        csvReader = GetComponent<CSVReader>();
        csvReader.LoadCSV();
        ShowDialogue();
    }

    public void ShowDialogue()
    {
        if (csvReader == null)
        {
            Debug.LogError("csvReader가 초기화되지 않았습니다.");
            return;
        }

        if (currentIndex >= csvReader.dialogueList.Count)
        {
            Debug.Log("대화가 끝났습니다. 다음 씬으로 이동합니다.");
            LoadNextScene();  // 마지막 대화 후 씬 전환 실행
            return;
        }

        CSVReader.DialogueEntry entry = csvReader.GetDialogue(currentIndex);
        if (entry == null)
        {
            Debug.LogError($"잘못된 대화 인덱스: {currentIndex}");
            return;
        }

        // 이름 입력 이벤트 처리
        if (entry.dialogue == "INPUT_NAME")
        {
            nameInputManager.ShowInputPanel();
            return; // 입력이 끝나면 다음 대사가 이어지므로 여기서 종료
        }

        // 플레이어 이름 치환
        string playerName = string.IsNullOrEmpty(PlayerData.Instance.playerName) ? "주인공" : PlayerData.Instance.playerName;
        nameText.text = entry.character.Replace("{Player}", playerName);
        dialogueText.text = entry.dialogue.Replace("{Player}", playerName);

        // 스프라이트 불러오기 및 적용
        if (!string.IsNullOrEmpty(entry.illust))
        {
            Sprite newSprite = Resources.Load<Sprite>("Illust/" + entry.illust);
            if (newSprite != null)
            {
                illustImage.sprite = newSprite;
                illustImage.gameObject.SetActive(true); // 이미지 활성화
            }
            else
            {
                Debug.LogWarning($"스프라이트 '{entry.illust}'을(를) 찾을 수 없습니다.");
                illustImage.gameObject.SetActive(false); // 스프라이트가 없으면 이미지 숨김
            }
        }
        else
        {
            illustImage.gameObject.SetActive(false); // illust 값이 없으면 이미지 숨김
        }
    }

    public void ContinueDialogue()
    {
        currentIndex++;
        ShowDialogue();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
