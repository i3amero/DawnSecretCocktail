using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        // 이미 인스턴스가 존재한다면 자신을 파괴(다른 scene에 SceneController GameObject가 존재할 경우 삭제)
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
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) // 빌드 설정에 있는 씬 수만큼 반복
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i); // 빌드의 i번째 씬 경로
            string name = System.IO.Path.GetFileNameWithoutExtension(path); // 경로에서 씬 이름만 추출
            if (name == sceneName)
            {
                return true;
            }
        }
        return false;
    }

}
