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
    public GameObject QWERPanel; // 인스펙터에서 패널 오브젝트 할당

    [SerializeField] private float gameDuration; // 게임 실행 시간
    [SerializeField] private float startDelay;    // 게임 준비 시간
    private int selectedMapID; // 선택된 맵 ID

    public MapDatabase mapDatabase; // MapDatabase 연결
    public MonsterSpawner monsterSpawner; // MonsterSpawner 연결
    public ScoreManager scoreManager; // ScoreManager 연결
    public TutorialDialogueManager tutorialDialogueManager; // TutorialDialogueManager 연결
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

    public void FindNewSceneUI() // 게임이 재시작 될 때 UI 찾기
    {
        // 1. 몬스터 스포너 찾기
        MonsterSpawner foundSpawner = Object.FindFirstObjectByType<MonsterSpawner>();
        if (foundSpawner != null)
        {
            monsterSpawner = foundSpawner;
        }
        else
        {
            Debug.LogWarning("[GameController] MonsterSpawner를 찾지 못했습니다.");
        }

        // 2. 스코어 매니저 찾기
        ScoreManager foundScoreManager = Object.FindFirstObjectByType<ScoreManager>();
        if (foundScoreManager != null)
        {
            scoreManager = foundScoreManager;
        }
        else
        {
            Debug.LogWarning("[GameController] ScoreManager를 찾지 못했습니다.");
        }

        // 3. 튜토리얼 다이얼로그 매니저 찾기
        if (gameMode == GameMode.Tutorial)
        {
            TutorialDialogueManager foundTutorialDialogueManager = Object.FindFirstObjectByType<TutorialDialogueManager>();
            if (foundTutorialDialogueManager != null)
            {
                tutorialDialogueManager = foundTutorialDialogueManager;
            }
            else
            {
                Debug.LogWarning("[GameController] tutorialDialogueManager를 찾지 못했습니다.");
            }
        }

        // 4. QWER 패널 찾기
        GameObject foundQwerPanelObj = FindQWERPanel();
        if (foundQwerPanelObj != null)
        {
            // QWERPanel을 찾았으므로, 원하는 방식으로 사용합니다.
            // 예를 들어, 클래스 내에 GameObject 변수 qwerPanel이 있다면:
            QWERPanel = foundQwerPanelObj;
        }

        // 5. 카운트다운 텍스트 찾기 (오브젝트 이름으로 찾는 예시)
        GameObject countdownObj = GameObject.Find("CountdownText");
        if (countdownObj != null)
        {
            countdownText = countdownObj.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogWarning("[GameController] CountdownText 오브젝트를 찾지 못했습니다.");
        }

        // 6. 노말 게임에서 남은 시간 텍스트 찾기 
        if (gameMode == GameMode.Normal)
        {
            GameObject timeObj = GameObject.Find("TimeText");
            if (timeObj != null)
            {
                timeText = timeObj.GetComponent<TMP_Text>();
            }
            else
            {
                Debug.LogWarning("[GameController] TimeText 오브젝트를 찾지 못했습니다.");
            }
        }
    }

    private GameObject FindQWERPanel()
    {
        GameObject foundQwerPanel = null;
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "QWERPanel")
            {
                foundQwerPanel = obj;
                break;
            }
        }
        if (foundQwerPanel == null)
        {
            Debug.LogWarning("QWERPanel 오브젝트를 찾지 못했습니다.");
        }
        return foundQwerPanel;
    }

    public void InitializeGame() // 게임을 다시 시작 하기 위한 함수
    {
        if (timeText != null)
        {
            timeText.gameObject.SetActive(false); // 초기에는 UI 숨기기
        }


        // 선택된 맵 데이터 불러오기
        if (gameMode == GameMode.Normal) // 일반 모드일 때
        {
            selectedMapID = PlayerPrefs.GetInt("SelectedMapID", -1); // 선택된 Map ID 가져오기
        }
        else// 무한 모드나 튜토리얼일 때
        {
            selectedMapID = 2; // 3번 맵으로 고정
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

        // ScoreManager도 새 게임에 맞춰 UI를 리셋
        if (scoreManager != null)
        {
            scoreManager.ReinitializeScoreSettings();
        }


    }

    public void ChangeState(GameState newState) // 게임 상태를 변경하고 그에 맞는 로직 실행
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Preparation:
                Log("게임 준비 상태");
                if (gameMode == GameMode.Tutorial)
                {
                    if (tutorialDialogueManager != null)
                    {
                        tutorialDialogueManager.ShowFullDialogue(
                            "오셨군요. 그럼 저희 업무가 어떤 식으로 진행되는 지 설명해드리겠습니다.\n\n\n" +
        "<color=#FF9999>스페이스바를 누르거나 화면 아무곳을 클릭하면 대화창이 닫히고 진행됩니다.</color>",
                            () => {
                            // 대화창이 닫히면 게임 상태를 Running으로 변경
                            ChangeState(GameState.Running);
                        });
                    }
                    else
                    {
                        Log("튜토리얼 대화창 매니저를 찾지 못했습니다.");
                    }
                }
                else
                {
                    StartCoroutine(CountdownCoroutine()); // 게임 준비 때는 일정 시간 후 게임 시작
                }
                break;
            case GameState.Running:
                Log("게임 실행 상태");
                if (gameMode == GameMode.Normal) // 일반 모드일 때
                {
                    StartCoroutine(GameTimer()); // 게임 실행 중에는 지정된 시간 만큼 타이머 시작, 타이머가 끝나면 게임 종료
                    StartCoroutine(monsterSpawner.SpawnMonster()); // 타이머가 도는 동안 몬스터 스폰 코루틴 시작
                }
                else if(gameMode == GameMode.Infinite)// 무한 모드일 때
                {
                    // 점수, 콤보 UI 표시
                    if (scoreManager != null)
                    {
                        scoreManager.ShowUI(scoreManager.scoreText);
                        scoreManager.ShowUI(scoreManager.comboText);
                        scoreManager.ShowUI(scoreManager.bestScoreText);
                    }
                    if (QWERPanel != null)
                    {
                        QWERPanel.SetActive(true);
                    }
                    StartCoroutine(monsterSpawner.SpawnMonster()); // 무한 모드는 타이머가 없음(스킬 입력 실패 시 종료)
                }
                else if (gameMode == GameMode.Tutorial)
                {
                    if (scoreManager != null)
                    {
                        scoreManager.ShowUI(scoreManager.killCountText);
                    }
                    if (QWERPanel != null)
                    {
                        QWERPanel.SetActive(true);
                    }
                    StartCoroutine(monsterSpawner.SpawnMonster()); // 튜토리얼은 타이머가 없음(몬스터 3마리 처치 시 종료)
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
        else
        {
            Log("TimeText를 찾지 못했습니다.");
        }

        if(QWERPanel != null)
        {
            QWERPanel.SetActive(true);
        }
        else
        {
            Log("QWERPanel을 찾지 못했습니다.");
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
