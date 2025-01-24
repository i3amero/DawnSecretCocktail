using System.Collections;
using UnityEngine;

public enum GameState
{
    Preparation,
    Running,
    Ended
}

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public int Score { get; private set; } = 0; // 점수 관리
    [SerializeField] private float gameDuration = 5f; // 게임 실행 시간
    [SerializeField] private float startDelay = 3f;    // 게임 준비 시간

    public MapDatabase mapDatabase; // MapDatabase 연결

    public GameState CurrentState { get; private set; } = GameState.Preparation;

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

    private void Start()
    {
        // 선택된 맵 데이터 불러오기
        int selectedMapID = PlayerPrefs.GetInt("SelectedMapID", 0); // 선택된 Map ID 가져오기
        if (selectedMapID >= 0 && selectedMapID < mapDatabase.maps.Length)
        {
            MapDatabase.MapData selectedMap = mapDatabase.maps[selectedMapID];
            Log($"선택된 맵: {selectedMap.mapName}");

            // 맵 프리팹 생성
            if (selectedMap.mapPrefab != null)
            {
                Instantiate(selectedMap.mapPrefab);
            }
            else
            {
                Debug.LogWarning("선택된 맵에 프리팹이 없습니다!");
            }
        }
        else
        {
            Debug.LogError("유효하지 않은 Map ID입니다!");
        }

        ChangeState(GameState.Preparation);
    }

    private void ChangeState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Preparation:
                Log("게임 준비 상태");
                StartCoroutine(StartGameCoroutine(startDelay));
                break;
            case GameState.Running:
                Log("게임 실행 상태");
                StartCoroutine(GameTimer());
                break;
            case GameState.Ended:
                Log("게임 종료 상태");
                GoToScoreScreen();
                break;
        }
    }

    private IEnumerator StartGameCoroutine(float delay)
    {
        if (delay > 0)
        {
            Log("게임 준비 중...");
            yield return new WaitForSeconds(delay);
        }

        GameController.Instance.ResetScore();
        ChangeState(GameState.Running);
    }

    private IEnumerator GameTimer()
    {
        float elapsedTime = 0f;

        while (elapsedTime < gameDuration)
        {
            elapsedTime += Time.deltaTime;
            //Log($"남은 시간: {gameDuration - elapsedTime:F1}초");
            yield return null;
        }

        ChangeState(GameState.Ended);
    }

    private void GoToScoreScreen()
    {
        Log("점수 계산 화면으로 이동");
        SceneController.Instance.LoadScene("ScoreScene");
    }

    public void AddScore(int points)
    {
        Score += points;
        Debug.Log("Current Score: " + Score);
    }

    public void ResetScore()
    {
        Score = 0;
    }

    private void Log(string message)
    {
        Debug.Log($"[GameController] {message}");
    }
}
