﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewStartClick : MonoBehaviour
{
    public string sceneName; // 변경할 씬의 이름

    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(ChangeScene);
            button.onClick.AddListener(ExitGame); // 게임 종료 기능 추가
        }
    }

    public void ChangeScene()
    {

        if (!string.IsNullOrEmpty(sceneName))
        {
            //이부분, 이렇게 관리할거라면 PlayerPrefs관리 스크립트를 별도로 만드는게 나을 듯 합니다.(물론 이번엔 힘들었겠지만)
            PlayerPrefs.SetInt($"NightScenario_카타르시스", 0);
            PlayerPrefs.SetInt($"CocktailScenario_카타르시스", 0);
            PlayerPrefs.SetInt($"NightScenario_데드리프트", 0);
            PlayerPrefs.SetInt($"CocktailScenario_데드리프트", 0);
            PlayerPrefs.SetInt($"NightScenario_핀투라", 0);
            PlayerPrefs.SetInt($"CocktailScenario_핀투라", 0);
            PlayerPrefs.SetInt($"NightScenario_레조나", 0);
            PlayerPrefs.SetInt($"CocktailScenario_레조나", 0);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("씬 이름이 설정되지 않았습니다.");
        }
    }
    public void ExitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}
