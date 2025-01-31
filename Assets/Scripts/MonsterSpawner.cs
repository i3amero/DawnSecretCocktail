using System.Collections;
using System.Linq;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private MapDatabase.MapData currentMap; // 현재 맵 데이터
    public Transform spawnPoint;           // 스폰 위치

    private GameObject currentMonster;     // 현재 화면에 등장한 몬스터
    private Coroutine removeMonsterCoroutine; // 현재 실행 중인 Coroutine

    public void SetCurrentMap(MapDatabase.MapData mapData)
    {
        currentMap = mapData;
    }

    public void SpawnMonster()
    {
        // 게임이 실행 중인지 확인
        if (GameController.Instance != null && GameController.Instance.CurrentState != GameState.Running)
        {
            Debug.LogWarning("게임이 실행 중이 아니므로 몬스터를 스폰하지 않습니다.");
            return;
        }

        if (currentMonster == null)
        {
            if (currentMap == null || currentMap.monsterDatabase == null)
            {
                Debug.LogWarning("현재 맵 데이터 또는 몬스터 데이터베이스가 없습니다!");
                return;
            }

            // 허용된 몬스터 타입 필터링
            var allowedTypes = currentMap.allowedMonsterTypes;
            var validMonsters = currentMap.monsterDatabase.monsters
                .Where(monster => allowedTypes.Contains(monster.type))
                .ToArray();

            if (validMonsters.Length == 0)
            {
                Debug.LogWarning("허용된 몬스터가 없습니다!");
                return;
            }

            // 랜덤으로 몬스터 선택
            var randomMonster = validMonsters[Random.Range(0, validMonsters.Length)];

            if (randomMonster.prefab != null)
            {
                // 기존 Coroutine 중지
                if (removeMonsterCoroutine != null)
                {
                    StopCoroutine(removeMonsterCoroutine);
                    removeMonsterCoroutine = null;
                }

                // 프리팹이 있는 경우 실제로 생성
                var spawnPosition = spawnPoint != null
                     ? spawnPoint.position
                     : Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                spawnPosition.z = 0; // 2D 게임에서 Z축 고정

                currentMonster = Instantiate(randomMonster.prefab, spawnPosition, Quaternion.identity);

                // MonsterController에 데이터 전달
                var monsterController = currentMonster.GetComponent<MonsterController>();

                if (monsterController != null)
                {
                    monsterController.Initialize(randomMonster);
                    // 새로운 타이머 시작
                    removeMonsterCoroutine = StartCoroutine(RemoveMonsterAfterDuration(randomMonster.spawnDuration));

                }
                else
                {
                    Debug.LogError("MonsterController가 프리팹에 추가되지 않았거나, 초기화되지 않았습니다.");
                }

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

    private IEnumerator RemoveMonsterAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        // 몬스터가 아직 존재하면 제거
        if (currentMonster != null)
        {
            Debug.Log($"시간 초과로 제거된 몬스터: {currentMonster.name}");
            var scoreManager = Object.FindFirstObjectByType<ScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.OnSkillSuccess(0, false); // 시간 초과시 콤보 초기화
            }
            RemoveCurrentMonster();
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

            // 기존 Coroutine 중지
            if (removeMonsterCoroutine != null)
            {
                StopCoroutine(removeMonsterCoroutine);
                removeMonsterCoroutine = null;
            }

            Destroy(currentMonster);
            currentMonster = null;

            // 다음 몬스터 생성
            SpawnMonster();
        }
        else
        {
            Debug.LogWarning("현재 몬스터가 제거 되었습니다.");
        }
    }

    public GameObject GetCurrentMonster()
    {
        return currentMonster;
    }
}
