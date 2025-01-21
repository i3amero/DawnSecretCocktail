using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    public SkillDatabase skillDatabase; // SkillDatabase 참조
    private string currentCombination = ""; // 현재 입력된 키 조합
    private float lastInputTime; // 마지막 키 입력 시간
    public float inputTimeout = 1.0f; // 키 입력 제한 시간

    void Update()
    {
        HandleInput(); // 플레이어 키 입력 처리
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

        // 입력 시간이 초과되면 조합 초기화
        if (Time.time - lastInputTime > inputTimeout)
        {
            ResetCombination();
        }
    }

    private void AddKeyToCombination(string key)
    {
        if (currentCombination.Length < 2)
        {
            currentCombination += key;
            lastInputTime = Time.time; // 입력 시간 갱신
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
