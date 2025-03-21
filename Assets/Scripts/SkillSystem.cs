﻿//캐릭터 프리팹을 그냥 쌩으로 생성하는데, 당장은 규모가 작으니 상관은 없다만,
//나중에 사이즈 커지면 GC(가비지 컬렉션)이 발생하니, 오브젝트 풀링은 의식하시는게 좋을 듯

using System.Collections;
using TMPro;
using UnityEngine;

// ** 게임 실행 도중 스킬을 입력 받고 스폰된 몬스터 적중 시 데이터를 처리하는 클래스 **
public class SkillSystem : MonoBehaviour
{
    public SkillDatabase skillDatabase; // SkillDatabase 참조
    public MonsterSpawner monsterSpawner; // MonsterSpawner 참조
    public ScoreManager scoreManager; // ScoreManager 참조
    public TutorialDialogueManager tutorialDialogueManager; // TutorialDialogueManager 참조

    // Q, W, E, R 입력 시 생성할 캐릭터 프리팹
    public GameObject qPrefab;
    public GameObject wPrefab;
    public GameObject ePrefab;
    public GameObject rPrefab;

    private string currentCombination = ""; // 현재 입력된 키 조합
    private GameObject Character1; // 생성된 캐릭터
    private GameObject Character2; // 생성된 캐릭터
    public GameObject skillIcon;
    private bool isAnimating = false;
    private float reactionTime; // 반응 시간

    void Start() // 연결을 제대로 했는지 확인
    {
        if (monsterSpawner == null)
        {
            Debug.LogError("MonsterSpawner가 인스펙터에 할당되지 않았습니다!");
        }
        if (skillDatabase == null)
        {
            Debug.LogError("SkillDatabase가 인스펙터에 할당되지 않았습니다!");
        }
    }

    void Update()
    {
        // 게임이 실행 중일 때만 입력 처리
        if (GameController.Instance != null && GameController.Instance.CurrentState == GameState.Running)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        // 대화창이 열려있으면 스킬 입력 무시
        if (tutorialDialogueManager != null && tutorialDialogueManager.IsDialogueActive)
        {
            return;
        }

        if (isAnimating) // 캐릭터가 페이드 인/아웃 시 입력 무시
        {
            return;
        }

        if (Input.anyKeyDown)
        {
            if (!monsterSpawner.isMonsterReady) // 몬스터가 준비되지 않았다면 입력 무시
            {
                Debug.Log("몬스터가 준비되지 않음. 입력 무시.");
                return;
            }

            // Q, W, E, R 키 입력 시 조합에 추가
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if(SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlaySelectSound(); // Q 키 입력 시 선택 사운드 재생
                }
                AddKeyToCombination("Q");               
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlaySelectSound(); // W 키 입력 시 선택 사운드 재생
                }
                AddKeyToCombination("W");
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlaySelectSound(); // E 키 입력 시 선택 사운드 재생
                }
                AddKeyToCombination("E");
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlaySelectSound(); // R 키 입력 시 선택 사운드 재생
                }
                AddKeyToCombination("R");
            }
        }
    }

    private void AddKeyToCombination(string key)
    {
        // 중복 키 입력 시 조합 초기화
        if (currentCombination.Length > 0 && currentCombination[currentCombination.Length - 1].ToString() == key)
        {
            ResetCombination();
            StartCoroutine(RemoveOneCharacter());
            Debug.Log("조합 초기화: 중복 키 입력");
            return;
        }
        
        if (currentCombination.Length < 2)
        {
            if(key == "Q")
            {
                currentCombination += key;
                if(currentCombination.Length == 2)
                {
                    CheckValidSkillCombination();
                }
                StartCoroutine(ShowCharacterPrefab(qPrefab));  // Q 캐릭터 생성
            }
            if (key == "W")
            {
                currentCombination += key;
                if (currentCombination.Length == 2)
                {
                    CheckValidSkillCombination();
                }
                StartCoroutine(ShowCharacterPrefab(wPrefab));  // W 캐릭터 생성
            }
            if (key == "E")
            {
                currentCombination += key;
                if (currentCombination.Length == 2)
                {
                    CheckValidSkillCombination();
                }
                StartCoroutine(ShowCharacterPrefab(ePrefab));  // E 캐릭터 생성
            }
            if (key == "R")
            {
                currentCombination += key;
                if (currentCombination.Length == 2)
                {
                    CheckValidSkillCombination();
                }
                StartCoroutine(ShowCharacterPrefab(rPrefab));  // R 캐릭터 생성
            }
        }
    }

    private void CheckValidSkillCombination()
    {
        foreach (var combo in skillDatabase.skillCombinations)
        {
            if (combo.combination1 == currentCombination || combo.combination2 == currentCombination)
            {
                IsSkillValid(combo.resultingSkill);
                return;
            }

        }
    }

    private void IsSkillValid(Skill skill)
    {
        if (monsterSpawner != null && monsterSpawner.GetCurrentMonster() != null)
        {
            monsterSpawner.CancelMonsterRemoval(); // 스킬 키를 두개 입력받으면 자동 몬스터 제거 취소

            var currentMonster = monsterSpawner.GetCurrentMonster();
            var monsterController = currentMonster.GetComponent<MonsterController>();

            if (monsterController != null && monsterController.MonsterData != null)
            {
                if (monsterController.MonsterData.validSkills == skill.name)
                {
                    // 성공
                    reactionTime = Time.time - monsterController.spawnTime; // 반응 시간 계산
                }
            }
        }
    }

    // Q, W, E, R 키를 누를 때마다 해당 프리팹을 생성
    private IEnumerator ShowCharacterPrefab(GameObject prefab)
    {
        isAnimating = true; // 애니메이션 시작

        if (prefab == null)
        {
            Debug.LogError("프리팹이 없습니다.");
            yield break;
        }

        float scaleFactor = 0.8f;     // 전체 크기 조절
        float xscaleFactor = 0.3f;    // 가로 비율
        float yscaleFactor = 0.3f;   // 세로 비율

        if (Character1 == null)
        {
            var spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 3, 0, 0));
            spawnPosition.z = 0; // 2D 게임에서 Z축 고정

            // 원하는 위치와 회전값으로 Instantiate
            Character1 = Instantiate(prefab, spawnPosition, Quaternion.identity);

            // 원하는 최종 스케일 계산
            float finalX = xscaleFactor * scaleFactor;
            float finalY = yscaleFactor * scaleFactor;
            float finalZ = 1f; // 2D면 1, 3D라면 scaleFactor 등 상황에 맞게

            MonsterFadeEffect fadeEffect = Character1.GetComponent<MonsterFadeEffect>();
            if (fadeEffect != null)
            {
                StartCoroutine(fadeEffect.FadeIn()); // 서서히 등장
            }

            Character1.transform.localScale = new Vector3(finalX, finalY, finalZ);
        }
        else
        {
            var spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 2 / 3, 0, 0));
            spawnPosition.z = 0; // 2D 게임에서 Z축 고정

            // 원하는 위치와 회전값으로 Instantiate
            Character2 = Instantiate(prefab, spawnPosition, Quaternion.identity);

            // 원하는 최종 스케일 계산
            float finalX = xscaleFactor * scaleFactor;
            float finalY = yscaleFactor * scaleFactor;
            float finalZ = 1f; // 2D면 1, 3D라면 scaleFactor 등 상황에 맞게

            Character2.transform.localScale = new Vector3(finalX, finalY, finalZ);

            MonsterFadeEffect fadeEffect = Character2.GetComponent<MonsterFadeEffect>();
            if (fadeEffect != null)
            {
                yield return StartCoroutine(fadeEffect.FadeIn());
            }

            StartCoroutine(RemoveCharacters());
            CheckSkillCombination();
        }

        isAnimating = false; // 애니메이션 끝
    }

    public void StartRemoveOneCharacter()
    {
        StartCoroutine(RemoveOneCharacter());
    }

    private IEnumerator RemoveOneCharacter()
    {
        isAnimating = true; // 애니메이션 시작

        // 먼저 Character1이 아직 존재하는지 확인
        if (Character1 == null)
        {
            Debug.LogWarning("Character1이 이미 파괴되었습니다.");
            isAnimating = false;
            yield break;
        }

        MonsterFadeEffect fadeEffect1 = Character1.GetComponent<MonsterFadeEffect>();
        // 페이드 효과 코루틴을 시작
        Coroutine fade1 = null;

        if (fadeEffect1 != null)
        {
            fade1 = StartCoroutine(fadeEffect1.FadeOut());
        }
        // 두 코루틴이 모두 끝날 때까지 대기
        if (fade1 != null)
        {
            yield return fade1;
        }

        // 페이드 아웃 후 다시 한 번 null 체크
        if (Character1 != null)
        {
            Destroy(Character1);
        }

        isAnimating = false; // 애니메이션 끝
    }

    private IEnumerator RemoveCharacters()
    {
        isAnimating = true; // 애니메이션 시작

        // 둘 다 null이면 바로 종료
        if (Character1 == null && Character2 == null)
        {
            isAnimating = false;
            yield break;
        }

        MonsterFadeEffect fadeEffect1 = null;
        MonsterFadeEffect fadeEffect2 = null;

        if (Character1 != null)
        {
            fadeEffect1 = Character1.GetComponent<MonsterFadeEffect>();
        }
        if (Character2 != null)
        {
            fadeEffect2 = Character2.GetComponent<MonsterFadeEffect>();
        }


        // 페이드 효과 코루틴을 동시에 시작
        Coroutine fade1 = null;
        Coroutine fade2 = null;

        if (fadeEffect1 != null)
        {
            fade1 = StartCoroutine(fadeEffect1.FadeOut());
        }
        if (fadeEffect2 != null)
        {
            fade2 = StartCoroutine(fadeEffect2.FadeOut());
        }

        // 두 코루틴이 모두 끝날 때까지 대기
        if (fade1 != null)
        {
            yield return fade1;
        }
        if (fade2 != null)
        {
            yield return fade2;
        }

        if (Character1 != null) Destroy(Character1);
        if (Character2 != null) Destroy(Character2);

        isAnimating = false; // 애니메이션 끝
    }

    private void CheckSkillCombination()
    {
        foreach (var combo in skillDatabase.skillCombinations)
        {
            if (combo.combination1 == currentCombination || combo.combination2 == currentCombination)
            {
                StartCoroutine(ExecuteSkillCoroutine(combo.resultingSkill)); // 조합에 맞는 스킬 실행
                ResetCombination(); // 조합 초기화
                return;
            }
        }
    }

   

    private IEnumerator ExecuteSkillCoroutine(Skill skill)
    {
        Debug.Log($"스킬 발동: {skill.name}");
        
        // 현재 몬스터가 있는지 확인 후 스킬 발동 결과 처리
        if (monsterSpawner != null && monsterSpawner.GetCurrentMonster() != null)
        {
            var currentMonster = monsterSpawner.GetCurrentMonster();
            var monsterController = currentMonster.GetComponent<MonsterController>();

            // MonsterController와 MonsterData가 유효한지 확인
            if (monsterController == null)
            {
                Debug.LogError("MonsterController가 null입니다.");
                yield break;
            }

            if (monsterController.MonsterData == null)
            {
                Debug.LogError("MonsterController의 MonsterData가 null입니다.");
                yield break;
            }
            // *****************************************


            if (monsterController != null && monsterController.MonsterData.validSkills == skill.name)
            {
                // 스킬 성공: 몬스터 제거 및 점수 추가
                Debug.Log($"스킬 성공! {monsterController.MonsterData.name} 제거");
                Debug.Log($"스킬 적중시간 : {reactionTime}");

                if(SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlaySuccessSound(); // 성공 사운드 재생
                }

                if (scoreManager != null)
                {
                    scoreManager.OnSkillSuccess(reactionTime, true); // 스킬 성공, reactionTime에 따라 ScoreManager에서 점수 추가
                    if(scoreManager.tutorialKillCount < 3 && GameController.Instance.gameMode == GameMode.Tutorial)
                    {
                        scoreManager.tutorialKillCount++; // 튜토리얼에서 처치한 몬스터 수 증가
                        scoreManager.UpdateKillCountText(scoreManager.tutorialKillCount.ToString()); // 처치한 몬스터 수 UI 업데이트
                    }
                }

                /*
                // 스킬 효과 프리팹(스킬 아이콘)이 있다면, 코루틴을 통해 생성 후 대기
                if (skill.effectPrefab != null)
                {
                    yield return StartCoroutine(SpawnSkillIconCoroutine(skill.effectPrefab));
                }

                yield return new WaitForSeconds(0.5f); // 스킬이 발동 된 후 잠시 대기
                */

                if (GameController.Instance.gameMode == GameMode.Tutorial && scoreManager.tutorialKillCount == 3) // 튜토리얼에서 몬스터를 3마리 잡았을 경우
                {
                    // 현재 화면의 요소들을 제거 
                    if (monsterSpawner != null && tutorialDialogueManager != null)
                    {
                        tutorialDialogueManager.ShowFullDialogue("잘하셨습니다!",
                                () =>
                                {
                                    monsterSpawner.DestroyCurrentMonsterCompletely(); // 대화창이 닫히면 튜토리얼 종료
                                });
                    }
                }
                else if (GameController.Instance.gameMode == GameMode.Tutorial && scoreManager.tutorialKillCount < 3)
                {
                    if (monsterSpawner != null && tutorialDialogueManager != null)
                    {
                        int remainingCount = 3 - scoreManager.tutorialKillCount;
                        tutorialDialogueManager.ShowFullDialogue($"잘하셨습니다! 앞으로 {remainingCount}번만 더 대응해 보시죠.",
                                () =>
                                {
                                    monsterSpawner.RemoveCurrentMonster(); // 대화창이 닫히면 다음 현상으로 넘어가기
                                    currentMonster = null;
                                });
                    }
                }
                else // 무한,일반 모드일 때
                {
                    monsterSpawner.RemoveCurrentMonster();
                    currentMonster = null;
                }
            }
            else
            {
                // 스킬 실패: 콤보 초기화
                Debug.LogWarning($"스킬 실패! {monsterController.MonsterData.name}에 유효한 스킬은 {monsterController.MonsterData.validSkills}입니다.");

                if(SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlayFailSound(); // 실패 사운드 재생
                }

                var scoreManager = Object.FindFirstObjectByType<ScoreManager>();
                if (scoreManager != null)
                {
                    scoreManager.OnSkillSuccess(0, false); // 스킬 실패, ScoreManager에서 콤보 초기화
                }

                /*
                // 스킬 효과 프리팹(스킬 아이콘)이 있다면, 코루틴을 통해 생성 후 대기
                if (skill.effectPrefab != null)
                {
                    yield return StartCoroutine(SpawnSkillIconCoroutine(skill.effectPrefab));
                }

                yield return new WaitForSeconds(0.5f); // 스킬이 발동 된 후 잠시 대기
                */

                if (GameController.Instance.gameMode == GameMode.Infinite) // 무한 모드일 때
                {
                    yield return new WaitForSeconds(0.8f); // 게임 종료전 잠시 대기
                    GameController.Instance.ChangeState(GameState.Ended); // 게임 종료 상태로 변경
                }
                else if(GameController.Instance.gameMode == GameMode.Normal)// 일반 모드일 때
                {
                    monsterSpawner.RemoveCurrentMonster(); // 다음 현상으로 넘어가기
                }
                else if(GameController.Instance.gameMode == GameMode.Tutorial)
                {
                    if (tutorialDialogueManager != null)
                    {
                        tutorialDialogueManager.ShowFullDialogue("흠.. 다시 해보시겠습니까?",
                            () =>
                        {
                            monsterSpawner.RemoveCurrentMonster(); // 대화창이 닫히면 다음 현상으로 넘어가기
                        });
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("현재 몬스터가 없습니다!");
        }
    }

    private IEnumerator SpawnSkillIconCoroutine(GameObject effectPrefab)
    {

        // ScreenToWorldPoint를 사용할 때, z값은 카메라와의 거리입니다.
        // 카메라가 z=-10에 있다면, 10을 넣어 월드 좌표로 변환한 뒤 z값을 0으로 고정합니다.
        Vector3 screenPos = new Vector3(Screen.width / 2, Screen.height * 2 / 5, 0);
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(screenPos);
        spawnPosition.z = 0; // 2D 게임에서 Z축 고정

        // 스킬 아이콘 인스턴스 생성 -> 추후 몬스터가 제거 되었을 때 같이 제거
        skillIcon = Instantiate(effectPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("스킬 아이콘 생성됨");

        // 코루틴을 종료 (추가 효과나 딜레이가 필요하면 여기서 yield return 문을 추가)
        yield break;
    }

    public bool IsSkillCombinationEmpty()
    {
        if (currentCombination == "")
        {
            return true;
        }
        else
        {
            return false; 
        }
    }

    public void ResetCombination()
    {
        currentCombination = "";
    }
}
