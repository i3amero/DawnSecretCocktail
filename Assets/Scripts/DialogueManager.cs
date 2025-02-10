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

    private int currentIndex = 0;

    void Start()
    {
        csvReader = GetComponent<CSVReader>();
        ShowDialogue();
    }

    public void ShowDialogue()
    {
        CSVReader.DialogueEntry entry = csvReader.GetDialogue(currentIndex);
        if (entry == null) return;

        nameText.text = entry.character;
        dialogueText.text = entry.dialogue;

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
        }
    }

    public void NextDialogue()
    {
        currentIndex++;
        ShowDialogue();
    }
}
