// PlayerPrefs를 다루는(전체 세이브,로드 기능 등등 구현) 전역 스크립트를 만드는 것도 좋을 듯함 
using System.Collections;
using TMPro;
using UnityEngine;

public enum GameMode
{
    Normal,
    Tutorial,
    Infinite
}

public enum GameState
{
    Preparation,
    Running,
    Ended
}

// 새벽 컨텐츠 게임 진행 컨트롤러 입니다.
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; } // 읽기 전용 프로퍼티
    public int Score { get; private set; } = 0; // 점수 관리(다른 Scene에서 접근 가능)
    public TMP_Text countdownText; // 카운트다운 표시 텍스트
    public TMP_Text timeText;// 남은 시간 표시 텍스트
    [SerializeField] private float gameDuration; // 게임 실행 시간
    [SerializeField] private float startDelay;    // 게임 준비 시간
    [SerializeField] private float spawnInterval; // 몬스터 스폰 주기
    private int selectedMapID; // 선택된 맵 ID

    public MapDatabase mapDatabase; // MapDatabase 연결
    public MonsterSpawner monsterSpawner; // MonsterSpawner 연결
    public ScoreManager scoreManager; // ScoreManager 연결
    public string sceneName; // 게임이 종료된 후 이동할 씬 이름
    public GameMode gameMode; // 게임 모드 설정

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
        if(timeText != null)
        {
            timeText.gameObject.SetActive(false); // 초기에는 UI 숨기기
        }
        // 선택된 맵 데이터 불러오기
        if(gameMode == GameMode.Normal) // 일반 모드일 때
        {
            selectedMapID = PlayerPrefs.GetInt("SelectedMapID", -1); // 선택된 Map ID 가져오기
        }
        else if(gameMode == GameMode.Infinite)// 무한 모드일 때
        {
            selectedMapID = 2; // 무한 모드는 3번 맵으로 고정
        }
        
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

    public void ChangeState(GameState newState) // 게임 상태를 변경하고 그에 맞는 로직 실행
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Preparation:
                Log("게임 준비 상태");
                StartCoroutine(CountdownCoroutine()); // 게임 준비 때는 일정 시간 후 게임 시작
                break;
            case GameState.Running:
                Log("게임 실행 상태");
                if (gameMode == GameMode.Normal) // 일반 모드일 때
                {
                    StartCoroutine(GameTimer()); // 게임 실행 중에는 지정된 시간 만큼 타이머 시작, 타이머가 끝나면 게임 종료
                    StartCoroutine(monsterSpawner.SpawnMonster()); // 타이머가 도는 동안 몬스터 스폰 코루틴 시작
                }
                else if(gameMode == GameMode.Infinite) // 무한 모드일 때
                {
                    // 점수, 콤보 UI 표시
                    if (scoreManager != null)
                    {
                        scoreManager.ShowUI(scoreManager.scoreText);
                        scoreManager.ShowUI(scoreManager.comboText);
                    }
                    StartCoroutine(monsterSpawner.SpawnMonster()); // 무한 모드는 타이머가 없음(fail 되면 바로 종료)
                }
                break;
            case GameState.Ended:
                monsterSpawner.RemoveCurrentMonster(); // 게임 종료 시 현재 몬스터 제거
                Log("게임 종료 상태");
                GoToScoreScreen(sceneName); // 게임 종료 시 점수 계산 화면으로 이동
                break;
        }
    }

    private IEnumerator CountdownCoroutine()
    {
        countdownText.gameObject.SetActive(true); // 텍스트 UI 활성화

        for (int i = (int)startDelay; i > 0; i--)
        {
            countdownText.text = i.ToString(); // 3, 2, 1 표시
            yield return new WaitForSeconds(1f); // 1초 대기
        }

        countdownText.text = "시작!"; // 마지막에 "시작!" 표시
        yield return new WaitForSeconds(1f); // 1초 대기 후

        countdownText.gameObject.SetActive(false); // 카운트다운 UI 숨기기

        GameController.Instance.ResetScore(); // 게임 시작 시 점수 초기화
        ChangeState(GameState.Running); // 게임 실행 상태로 변경
    }

    private IEnumerator GameTimer() // 게임을 지정된 시간만큼 실행
    {
        float elapsedTime = 0f; // 경과 시간
        int lastDisplayedTime = Mathf.CeilToInt(gameDuration); // 초기 남은 시간

        // 점수, 콤보, 남은 시간 UI 표시
        if (scoreManager != null)
        {
            scoreManager.ShowUI(scoreManager.scoreText);
            scoreManager.ShowUI(scoreManager.comboText);
        }

        if (timeText != null)
        {
            timeText.gameObject.SetActive(true);
        }

        UpdateTimeText(lastDisplayedTime);

        while (elapsedTime < gameDuration) // 지정된 시간만큼 반복
        {
            elapsedTime += Time.deltaTime;

            // 남은 시간을 정수로 계산 (올림하면 0초가 되기 전에 1초로 표시됨)
            int currentRemainingTime = Mathf.CeilToInt(gameDuration - elapsedTime);

            // 남은 시간이 변경되었을 때만 UI 업데이트
            if (currentRemainingTime != lastDisplayedTime)
            {
                UpdateTimeText(currentRemainingTime);
                lastDisplayedTime = currentRemainingTime;
            }

            yield return null;
        }

        ChangeState(GameState.Ended); // 지정된 시간이 다 지나면 게임 종료 상태로 변경
    }

    private void GoToScoreScreen(string sceneName)
    {
        Log("점수 계산 화면으로 이동");
        SceneController.Instance.LoadScene(sceneName);
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

    private void UpdateTimeText(int time)
    {
        if (timeText != null)
        {
            timeText.text = "남은 시간: " + time + "초";
        }
    }

    private void Log(string message)
    {
        Debug.Log($"[GameController] {message}");
    }
}
