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
    public static GameController Instance { get; private set; } // 읽기 전용 프로퍼티
    public int Score { get; private set; } = 0; // 점수 관리(다른 Scene에서 접근 가능)
    [SerializeField] private float gameDuration; // 게임 실행 시간
    [SerializeField] private float startDelay;    // 게임 준비 시간
    [SerializeField] private float spawnInterval; // 몬스터 스폰 주기

    public MapDatabase mapDatabase; // MapDatabase 연결
    public MonsterSpawner monsterSpawner; // MonsterSpawner 연결

    public GameState CurrentState { get; private set; } = GameState.Preparation; // 변수 초기화, 다른 스크립트에서 읽기 가능

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

    private void Start() // 맵 데이터 불러오기 및 게임 상태 변경
    {
        // 선택된 맵 데이터 불러오기
        int selectedMapID = PlayerPrefs.GetInt("SelectedMapID", -1); // 선택된 Map ID 가져오기
        if (selectedMapID >= 0 && selectedMapID < mapDatabase.maps.Length) // 유효한 Map ID인지 확인
        {
            MapDatabase.MapData selectedMap = mapDatabase.maps[selectedMapID]; // 선택된 맵 데이터 가져오기
            Log($"선택된 맵: {selectedMap.mapName}");

            // 맵 프리팹 생성
            if (selectedMap.mapPrefab != null) // 선택된 맵에 프리팹이 있는지 확인 후 생성
            {
                Instantiate(selectedMap.mapPrefab);
            }
            else
            {
                Debug.LogWarning("선택된 맵에 프리팹이 없습니다!");
            }

            // MonsterSpawner에 현재 맵 설정(맵에 맞는 몬스터 생성)
            if (monsterSpawner != null)
            {
                monsterSpawner.SetCurrentMap(selectedMap);
            }
        }
        else
        {
            Debug.LogError("유효하지 않은 Map ID입니다!");
        }

        ChangeState(GameState.Preparation); // 게임 준비 상태로 변경
    }

    private void ChangeState(GameState newState) // 게임 상태를 변경하고 그에 맞는 로직 실행
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Preparation:
                Log("게임 준비 상태");
                StartCoroutine(StartGameCoroutine(startDelay)); // 게임 준비 때는 일정 시간 후 게임 시작
                break;
            case GameState.Running:
                Log("게임 실행 상태");
                StartCoroutine(GameTimer()); // 게임 실행 중에는 지정된 시간 만큼 타이머 시작, 타이머가 끝나면 게임 종료
                StartCoroutine(monsterSpawner.SpawnMonster()); // 타이머가 도는 동안 몬스터 스폰 코루틴 시작
                break;
            case GameState.Ended:
                monsterSpawner.RemoveCurrentMonster(); // 게임 종료 시 현재 몬스터 제거
                Log("게임 종료 상태");
                GoToScoreScreen(); // 게임 종료 시 점수 계산 화면으로 이동
                break;
        }
    }

    private IEnumerator StartGameCoroutine(float delay) // 게임 준비 시간 후 게임 시작
    {
        if (delay > 0)
        {
            Log("게임 준비 중...");
            yield return new WaitForSeconds(delay); // 지정된 시간만큼 대기
        }

        GameController.Instance.ResetScore(); // 게임 시작 시 점수 초기화
        ChangeState(GameState.Running); // 게임 실행 상태로 변경
    }

    private IEnumerator GameTimer() // 게임을 지정된 시간만큼 실행
    {
        float elapsedTime = 0f; // 경과 시간

        while (elapsedTime < gameDuration) // 지정된 시간만큼 반복
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ChangeState(GameState.Ended); // 지정된 시간이 다 지나면 게임 종료 상태로 변경
    }

    private void GoToScoreScreen()
    {
        Log("점수 계산 화면으로 이동");
        SceneController.Instance.LoadScene("ScoreScene");
    }

    public void AddScore(int points) // ScoreManager에서 호출, 점수 추가
    {
        Score += points;
        Debug.Log("Current Score: " + Score); // 사용한 스킬로 몇점이 추가 되었는지 출력
    }

    public void ResetScore() // 게임 시작 시 호출, 점수 초기화
    {
        Score = 0;
    }

    private void Log(string message)
    {
        Debug.Log($"[GameController] {message}");
    }
}
