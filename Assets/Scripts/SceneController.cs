using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        // 이미 인스턴스가 존재한다면 자신을 파괴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 인스턴스 설정 및 씬 전환 시 파괴되지 않도록 유지
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        if (IsSceneInBuildSettings(sceneName)) // 빌드 설정에 씬이 포함되어 있는지 확인
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning($"'{sceneName}' 씬이 빌드 설정에 포함되지 않았습니다.");
        }
    }

    private bool IsSceneInBuildSettings(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName)
            {
                return true;
            }
        }
        return false;
    }

}
