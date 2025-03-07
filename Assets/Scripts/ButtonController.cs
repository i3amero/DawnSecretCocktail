using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 당장에는 문제가 없는 스크립트인데, 가장 큰 문제는 스크립트들의 구별과 설명이 없어서 어떤 스크립트가 어떤 역할을 하는지 알기 어려워요...
/// 씬 전환이랑 프로그램 종료 기능이 왜 같이 있는지 모르겠기도 하고 ㅎㅎ... 의도는 모르겠으나, 당장에 이 코드로 할 말은 없습니다
/// </summary>
public class SceneChangeButton : MonoBehaviour
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