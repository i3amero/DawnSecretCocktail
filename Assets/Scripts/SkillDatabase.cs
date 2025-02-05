using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SkillDatabase", menuName = "Database/Skill Database")]
public class SkillDatabase : ScriptableObject
{
    public List<Skill> skills; // 스킬 데이터 목록
    public List<SkillCombination> skillCombinations; // 키 조합 목록
}

[System.Serializable]
public class Skill
{
    public string name;          // 스킬 이름
    [TextArea(2, 5)]             // 설명이 길어서 여러 줄로 표시
    public string description;   // 스킬 설명
    public GameObject effectPrefab; // 스킬 효과 프리팹
    public float cooldown;       // 스킬 쿨타임
}

[System.Serializable]
public class SkillCombination
{
    public string combination1; // 키 조합 (예: "QW", "WE")
    public string combination2; // QW == WQ
    public Skill resultingSkill; // 해당 조합의 결과 스킬
}
