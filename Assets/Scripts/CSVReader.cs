using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    public TextAsset csvFile; // 유니티에서 CSV 파일을 드래그해서 연결
    private List<DialogueEntry> dialogueList = new List<DialogueEntry>();

    [System.Serializable]
    public class DialogueEntry
    {
        public string character;
        public string dialogue;
        public string expression;
    }

    void Start()
    {
        LoadCSV();
    }

    void LoadCSV()
    {
        string[] lines = csvFile.text.Split('\n');
        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            if (parts.Length < 3) continue;

            DialogueEntry entry = new DialogueEntry();
            entry.character = parts[0].Trim();
            entry.dialogue = parts[1].Trim();
            entry.expression = parts[2].Trim();

            dialogueList.Add(entry);
        }
    }

    public DialogueEntry GetDialogue(int index)
    {
        if (index < dialogueList.Count)
            return dialogueList[index];
        return null;
    }
}
