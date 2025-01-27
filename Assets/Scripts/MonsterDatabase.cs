using UnityEngine;

[CreateAssetMenu(fileName = "MonsterDatabase", menuName = "Database/MonsterDatabase", order = 1)]
public class MonsterDatabase : ScriptableObject
{
    public enum MonsterType { Bamsoneem, BamNori } // 몬스터 타입
    public MonsterData[] monsters; // 몬스터 데이터 배열

    [System.Serializable]
    public class MonsterData
    {
        public string name;                  // 몬스터 이름
        public MonsterType type;            // 몬스터 타입 (밤손님 / 밤놀이)
        public GameObject prefab;           // 몬스터 프리팹
        public float spawnDuration;         // 생성 후 지속 시간
        public string validSkills;        // 적중 가능한 스킬 목록 (스킬 이름)
    }
}
