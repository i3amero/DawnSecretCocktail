using System.Collections;
using System.Linq;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public ScoreManager scoreManager; // scoreManager 참조
    public SkillSystem skillSystem; // skillSystem 참조
    private MapDatabase.MapData currentMap; // 현재 맵 데이터
    public Transform spawnPoint;           // 스폰 위치

    private GameObject currentMonster;     // 현재 화면에 등장한 몬스터
    private Coroutine removeMonsterCoroutine; // 현재 실행 중인 Coroutine
    public bool isMonsterReady = false; // 몬스터 준비 상태 체크

    public void SetCurrentMap(MapDatabase.MapData mapData) // 맵에 맞는 몬스터 생성
    {
        currentMap = mapData;
    }

    public IEnumerator SpawnMonster() // GameContoller에서 몬스터 스폰주기 마다 호출 -> 몬스터를 생성하고, 몬스터 등장유지시간 후 제거
    {
        // 게임이 실행 중인지 확인
        if (GameController.Instance != null && GameController.Instance.CurrentState != GameState.Running)
        {
            Debug.LogWarning("게임이 실행 중이 아니므로 몬스터를 스폰하지 않습니다.");
            yield break;
        }

        if (currentMonster == null) // 현재 등장한 몬스터가 없으면
        {
            if (currentMap == null || currentMap.monsterDatabase == null)
            {
                Debug.LogWarning("현재 맵 데이터 또는 몬스터 데이터베이스가 없습니다!");
                yield break;
            }

            // 허용된 몬스터 타입 필터링
            var allowedTypes = currentMap.allowedMonsterTypes; // 맵에서 출현할 수 있는 몬스터 타입 배열
            var validMonsters = currentMap.monsterDatabase.monsters // 맵의 몬스터들 중
                .Where(monster => allowedTypes.Contains(monster.type)) // 허용된 몬스터 타입에 해당하는 몬스터 필터링
                .ToArray(); // 배열로 변환

            if (validMonsters.Length == 0)
            {
                Debug.LogWarning("허용된 몬스터가 없습니다!");
                yield break;
            }

            // 랜덤으로 몬스터 선택
            var randomMonster = validMonsters[Random.Range(0, validMonsters.Length)];

            if (randomMonster.prefab != null)
            {
                // 프리팹이 있는 경우 실제로 생성
                var spawnPosition = spawnPoint != null // 스폰 위치가 지정되어 있으면                                                       // 그 위치에 생성
                     ? spawnPoint.position // 그 위치에 생성
                     : Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height * 2 / 3, 0)); // 아니면 화면 중앙에 생성
                spawnPosition.z = 0; // 2D 게임에서 Z축 고정

                // spawnPosition에 랜덤으로 선택된 몬스터 prefab을 이용하여 생성, 회전은 기본값(없음)으로 설정
                currentMonster = Instantiate(randomMonster.prefab, spawnPosition, Quaternion.identity);

                // 생성된 몬스터에 페이드인 효과 추가
                MonsterFadeEffect fadeEffect = currentMonster.GetComponent<MonsterFadeEffect>();
                if (fadeEffect != null)
                {
                    yield return StartCoroutine(fadeEffect.FadeIn()); // 서서히 등장
                }

                // 생성된 몬스터가 MonsterController 스크립트를 가지고 있으면, monsterController 변수에 할당
                MonsterController monsterController = currentMonster.GetComponent<MonsterController>();

                if (monsterController != null)
                {
                    // monsterController에 randomMonster 데이터를 가져와서 초기화
                    monsterController.Initialize(randomMonster);
                    // 새로운 타이머 시작
                    removeMonsterCoroutine = StartCoroutine(RemoveMonsterAfterDuration(randomMonster.spawnDuration));

                }
                else
                {
                    Debug.LogError("MonsterController가 프리팹에 추가되지 않았거나, 초기화되지 않았습니다.");
                }

                // 페이드인 완료 후 몬스터 준비 상태 설정
                isMonsterReady = true; // 몬스터가 준비된 상태

                // 로그로 몬스터 정보 출력
                Debug.Log($"currentMonster: {randomMonster.name} (타입: {randomMonster.type}, 지속 시간: {randomMonster.spawnDuration}초)");

            }
            else
            {
                // 프리팹이 없는 경우 로그로만 처리
                Debug.LogWarning($"[몬스터 로그] {randomMonster.name} (타입: {randomMonster.type})가 등장했지만 프리팹이 없습니다!");
            }
        }
    }

    private IEnumerator RemoveMonsterAfterDuration(float duration) // 시간 초과로 몬스터가 제거되는 경우
    {
        yield return new WaitForSeconds(duration);

        // 몬스터가 아직 존재하면 제거 -> 스킬로 제거 실패 
        if (currentMonster != null)
        {
            Debug.Log($"시간 초과로 제거된 몬스터: {currentMonster.name}");
            // scoreManager에게 스킬로 제거 실패했다고 알려주기
            if (scoreManager != null)
            {
                scoreManager.OnSkillSuccess(0, false); // 시간 초과시 콤보 초기화
            }

            if(!skillSystem.IsSkillCombinationEmpty()) // 현재 캐릭터가 나와있다면
            {
                skillSystem.StartRemoveOneCharacter(); // 시간 초과시 현재 캐릭터 제거
            }
            
            RemoveCurrentMonster(); // 몬스터 제거
        }
    }

    public void RemoveCurrentMonster()
    {
        // 게임이 실행 중인지 확인
        if (GameController.Instance != null && GameController.Instance.CurrentState != GameState.Running)
        {
            Debug.LogWarning("게임이 종료됩니다. 현재 몬스터를 제거합니다.");
            return;
        }

        if (currentMonster != null)
        {
            Debug.Log($"제거된 몬스터: {currentMonster.name}");

            // 기존 Coroutine 중지 -> 스킬로 제거 되었을 경우
            if (removeMonsterCoroutine != null)
            {
                StopCoroutine(removeMonsterCoroutine);
                removeMonsterCoroutine = null;
            }

            // 페이드아웃 실행 후 제거하도록 변경
            StartCoroutine(FadeOutAndDestroy());
        }
        else
        {
            Debug.LogWarning("현재 몬스터가 생성 되어있지 않습니다.");
        }
    }

    // 페이드아웃 후 몬스터 제거 (코루틴)
    private IEnumerator FadeOutAndDestroy()
    {
        isMonsterReady = false; // 몬스터 제거 중 상태로 변경
        skillSystem.ResetCombination(); // 조합 초기화

        MonsterFadeEffect fadeEffect = currentMonster.GetComponent<MonsterFadeEffect>();
        if (fadeEffect != null)
        {
            yield return StartCoroutine(fadeEffect.FadeOut()); // 페이드아웃이 끝날 때까지 대기
        }

        Destroy(currentMonster);
        if(skillSystem.skillIcon != null)
        {
            Destroy(skillSystem.skillIcon); // 스킬 아이콘 제거
        }
        
        currentMonster = null;

        // 다음 몬스터 생성
        StartCoroutine(SpawnMonster());
    }

    public GameObject GetCurrentMonster()
    {
        return currentMonster;
    }
}
