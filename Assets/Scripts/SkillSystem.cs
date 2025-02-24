using UnityEngine;
/// <summary>
/// 깔끔하네용
/// </summary>
// ** 게임 실행 도중 스킬을 입력 받고 스폰된 몬스터 적중 시 데이터를 처리하는 클래스 **
public class SkillSystem : MonoBehaviour
{
    public SkillDatabase skillDatabase; // SkillDatabase 참조
    public MonsterSpawner monsterSpawner; // MonsterSpawner 참조
    public ScoreManager scoreManager; // ScoreManager 참조
    private string currentCombination = ""; // 현재 입력된 키 조합

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
        if (Input.anyKeyDown)
        {
            // Q, W, E, R 키 입력 시 조합에 추가
            if (Input.GetKeyDown(KeyCode.Q)) AddKeyToCombination("Q");
            if (Input.GetKeyDown(KeyCode.W)) AddKeyToCombination("W");
            if (Input.GetKeyDown(KeyCode.E)) AddKeyToCombination("E");
            if (Input.GetKeyDown(KeyCode.R)) AddKeyToCombination("R");

            // 조합이 두 글자 쌓였을 때만 검사
            if (currentCombination.Length == 2)
            {
                CheckSkillCombination();
            }
        }
    }

    private void AddKeyToCombination(string key)
    {
        // 중복 키 입력 시 조합 초기화
        if (currentCombination.Length > 0 && currentCombination[currentCombination.Length - 1].ToString() == key)
        {
            ResetCombination();
            Debug.Log("조합 초기화: 중복 키 입력");
            return;
        }
        
        if (currentCombination.Length < 2)
        {
            currentCombination += key;
        }
    }

    private void CheckSkillCombination()
    {
        foreach (var combo in skillDatabase.skillCombinations)
        {
            if (combo.combination1 == currentCombination || combo.combination2 == currentCombination)
            {
                ExecuteSkill(combo.resultingSkill); // 조합에 맞는 스킬 실행
                ResetCombination(); // 조합 초기화
                return;
            }
        }
    }

    private void ExecuteSkill(Skill skill)
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
                return;
            }

            if (monsterController.MonsterData == null)
            {
                Debug.LogError("MonsterController의 MonsterData가 null입니다.");
                return;
            }
            // *****************************************


            if (monsterController != null && monsterController.MonsterData.validSkills == skill.name)
            {
                // 스킬 성공: 몬스터 제거 및 점수 추가
                Debug.Log($"스킬 성공! {monsterController.MonsterData.name} 제거");
                float reactionTime = Time.time - monsterController.spawnTime; // 반응 시간 계산
                Debug.Log($"스킬 적중시간 : {reactionTime}");

                if (scoreManager != null)
                {
                    scoreManager.OnSkillSuccess(reactionTime, true); // 스킬 성공, reactionTime에 따라 ScoreManager에서 점수 추가
                }
                monsterSpawner.RemoveCurrentMonster();
            }
            else
            {
                // 스킬 실패: 콤보 초기화
                Debug.LogWarning($"스킬 실패! {monsterController.MonsterData.name}에 유효한 스킬은 {monsterController.MonsterData.validSkills}입니다.");

                var scoreManager = Object.FindFirstObjectByType<ScoreManager>();
                if (scoreManager != null)
                {
                    scoreManager.OnSkillSuccess(0, false); // 스킬 실패, ScoreManager에서 콤보 초기화
                }
            }
        }
        else
        {
            Debug.LogWarning("현재 몬스터가 없습니다!");
        }


        if (skill.effectPrefab != null)
        {
            Instantiate(skill.effectPrefab, transform.position, Quaternion.identity);
        }
    }

    public void ResetCombination()
    {
        currentCombination = "";
    }
}
