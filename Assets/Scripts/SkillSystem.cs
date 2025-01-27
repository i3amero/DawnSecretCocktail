using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    public SkillDatabase skillDatabase; // SkillDatabase 참조
    private string currentCombination = ""; // 현재 입력된 키 조합
    private MonsterSpawner monsterSpawner; // MonsterSpawner 참조

    void Start()
    {
        // MonsterSpawner 참조 가져오기
        monsterSpawner = Object.FindFirstObjectByType<MonsterSpawner>();
        if (monsterSpawner == null)
        {
            Debug.LogError("MonsterSpawner가 씬에 존재하지 않습니다!");
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
            if (Input.GetKeyDown(KeyCode.Q)) AddKeyToCombination("Q");
            if (Input.GetKeyDown(KeyCode.W)) AddKeyToCombination("W");
            if (Input.GetKeyDown(KeyCode.E)) AddKeyToCombination("E");
            if (Input.GetKeyDown(KeyCode.R)) AddKeyToCombination("R");

            CheckSkillCombination();
        }
    }

    private void AddKeyToCombination(string key)
    {
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

        // 현재 몬스터가 있는지 확인
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

                var scoreManager = Object.FindFirstObjectByType<ScoreManager>();
                if (scoreManager != null)
                {
                    scoreManager.OnSkillSuccess(true); // 스킬 성공
                }
                monsterSpawner.RemoveCurrentMonster();
            }
            else
            {
                // 스킬 실패
                Debug.LogWarning($"스킬 실패! {monsterController.MonsterData.name}에 유효한 스킬은 {monsterController.MonsterData.validSkills}입니다.");

                var scoreManager = Object.FindFirstObjectByType<ScoreManager>();
                if (scoreManager != null)
                {
                    scoreManager.OnSkillSuccess(false); // 스킬 실패
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

    private void ResetCombination()
    {
        currentCombination = "";
    }
}
