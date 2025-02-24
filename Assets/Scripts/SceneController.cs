using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// DontDestroyOnLoad 조심하세요~ 절대 안지워지는지라, 웬만하면 첫 씬에 배정합니다. 그리고 다회차때문에 씬에서 계속해서
/// 이 스크립트를 불러오면 메모리에 계속해서 쌓이게 됩니다.
/// (사실 이 스크립트에선 15~19행때문에 그럴 일은 없지만...)
/// </summary>
public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        if (IsSceneInBuildSettings(sceneName))
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
            string name = System.IO.Path.GetFileNameWithoutExtension(path); // 경로에서 씬 이름만 추출
            if (name == sceneName)
            {
                return true;
            }
        }
        return false;
    }

}
