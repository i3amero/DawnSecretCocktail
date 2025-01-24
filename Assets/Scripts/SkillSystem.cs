using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    public SkillDatabase skillDatabase; // SkillDatabase 참조
    private string currentCombination = ""; // 현재 입력된 키 조합

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
