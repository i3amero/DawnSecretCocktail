using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public float transitionDuration; // 전환 효과 지속 시간
    private CanvasGroup fadeCanvasGroup; // Canvas Group
    private RectTransform canvasTransform; // UI 전체 크기 조정
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

    private void FindCanvasComponents()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>(); // 현재 씬의 Canvas 찾기

        if (canvas != null)
        {
            fadeCanvasGroup = canvas.GetComponent<CanvasGroup>();
            canvasTransform = canvas.GetComponent<RectTransform>();

            if (fadeCanvasGroup == null)
            {
                Debug.LogWarning("CanvasGroup이 없습니다. Canvas에 추가하세요.");
            }
        }
        else
        {
            Debug.LogError("Canvas를 찾을 수 없습니다.");
        }
    }

    public void LoadSceneWithZoomOut(string sceneName)
    {
        StartCoroutine(ZoomOutAndLoadScene(sceneName));
    }

    private IEnumerator ZoomOutAndLoadScene(string sceneName)
    {
        FindCanvasComponents(); // 씬 변경할 때마다 Canvas 다시 찾기

        if (canvasTransform == null || fadeCanvasGroup == null)
        {
            Debug.LogError("Canvas 또는 CanvasGroup이 연결되지 않았습니다.");
            yield break;
        }

        float timer = 0f;
        fadeCanvasGroup.alpha = 1;

        while (timer < transitionDuration)
        {
            fadeCanvasGroup.alpha = 1 - (timer / transitionDuration); // 점점 투명
            timer += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
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
