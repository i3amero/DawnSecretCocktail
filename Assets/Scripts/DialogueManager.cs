using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image characterImage;
    public Sprite smileSprite;
    public Sprite neutralSprite;
    public Sprite happySprite;
    public CSVReader csvReader;
    public NameInputManager nameInputManager;

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
        nameText.text = entry.character;
        string replacedDialogue = entry.dialogue.Replace("{Player}", playerName);
        string replaceName = entry.character.Replace("{Player}", playerName);
        dialogueText.text = replacedDialogue;
        nameText.text = replaceName;

        // 표정 변경
        switch (entry.expression)
        {
            case "Smile":
                characterImage.sprite = smileSprite;
                break;
            case "Neutral":
                characterImage.sprite = neutralSprite;
                break;
            case "Happy":
                characterImage.sprite = happySprite;
                break;
            default:
                Debug.LogWarning($"알 수 없는 표정: {entry.expression}");
                break;
        }
    }
    public void ContinueDialogue()
    {
        currentIndex++;
        ShowDialogue();
    }

}
