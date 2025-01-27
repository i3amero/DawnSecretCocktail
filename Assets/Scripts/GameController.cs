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
    [SerializeField] private float gameDuration; // 게임 실행 시간
    [SerializeField] private float startDelay;    // 게임 준비 시간
    [SerializeField] private float spawnInterval; // 몬스터 스폰 주기

    public MapDatabase mapDatabase; // MapDatabase 연결
    public MonsterSpawner monsterSpawner; // MonsterSpawner 연결

    public GameState CurrentState { get; private set; } = GameState.Preparation;

    private Coroutine spawnCoroutine;

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

            // MonsterSpawner에 현재 맵 설정
            if (monsterSpawner != null)
            {
                monsterSpawner.SetCurrentMap(selectedMap);
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
                spawnCoroutine = StartCoroutine(SpawnMonstersCoroutine());
                break;
            case GameState.Ended:
                if (spawnCoroutine != null)
                {
                    monsterSpawner.RemoveCurrentMonster();
                    StopCoroutine(spawnCoroutine); // 스폰 코루틴 중지
                    spawnCoroutine = null;
                }
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
            yield return null;
        }

        ChangeState(GameState.Ended);
    }

    private IEnumerator SpawnMonstersCoroutine()
    {
        while (CurrentState == GameState.Running)
        {
            if (monsterSpawner != null)
            {
                monsterSpawner.SpawnMonster();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
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
