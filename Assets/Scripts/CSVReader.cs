using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    public TextAsset csvFile; // 유니티에서 CSV 파일을 드래그해서 연결
    public List<DialogueEntry> dialogueList = new List<DialogueEntry>();

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

    public void LoadCSV()
    {
        if (csvFile == null)
        {
            Debug.LogError("CSV 파일이 연결되지 않았습니다.");
            return;
        }

        string[] lines = csvFile.text.Split('\n');
        if (lines.Length == 0)
        {
            Debug.LogError("CSV 파일이 비어 있습니다.");
            return;
        }

        dialogueList.Clear(); // 기존 데이터 초기화

        foreach (string line in lines)
        {
            string[] parts = line.Split(']');
            if (parts.Length < 3) continue;

            DialogueEntry entry = new DialogueEntry
            {
                character = parts[0].Trim().Trim('"'),
                dialogue = parts[1].Trim().Trim('"'),
                expression = parts[2].Trim().Trim('"')
            };

            dialogueList.Add(entry);
        }

        Debug.Log($"CSV 로드 완료! 총 {dialogueList.Count}개의 대화가 추가됨.");
    }



    public DialogueEntry GetDialogue(int index)
    {
        if (index < dialogueList.Count)
            return dialogueList[index];
        return null;
    }
}
