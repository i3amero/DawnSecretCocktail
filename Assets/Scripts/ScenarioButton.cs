using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScenarioButtonManager : MonoBehaviour
{
    public Button[] episodeButtons;
    public TMP_Text[] episodeTexts;

    private string[] characterNames = { "레조나", "데드리프트", "핀투라", "카타르시스", "베네딕트" };

    private void Start()
    {
        UpdateScenarioButtons();
    }

    public void UpdateScenarioButtons()
    {
        for (int i = 0; i < episodeButtons.Length; i++)
        {
            bool isUnlocked = PlayerPrefs.GetInt($"Scenario_{characterNames[i]}", 0) == 1;

            if (isUnlocked)
            {
                episodeButtons[i].interactable = true;
                episodeButtons[i].image.color = Color.white;
            }
            else
            {
                episodeButtons[i].interactable = false;
                episodeButtons[i].image.color = new Color(1, 1, 1, 0.5f);
            }

            episodeTexts[i].text = GetScenarioName(characterNames[i]);
        }
    }

    private string GetScenarioName(string characterName)
    {
        switch (characterName)
        {
            case "레조나": return "그대에게 바치는 비탄의 교향곡";
            case "데드리프트": return "방황하는 머리 잘린 괴수";
            case "핀투라": return "칠흑보다 어두운 검은 안개";
            case "카타르시스": return "영원히 끝나지 않는 커튼";
            case "베네딕트": return "괴이";
            default: return "알 수 없는 에피소드";
        }
    }
}

