using UnityEngine;

[CreateAssetMenu(fileName = "MapDatabase", menuName = "Database/MapDatabase", order = 1)]
public class MapDatabase : ScriptableObject
{
    [System.Serializable]
    public class MapData
    {
        public string mapName;                     // 맵 이름
        public float scoreMultiplier;             // 점수 배율
        public Sprite thumbnail;                  // 맵 썸네일
        public GameObject mapPrefab;              // 맵 프리팹
        public MonsterDatabase monsterDatabase;   // 해당 맵에서 사용할 몬스터 데이터베이스
        public MonsterDatabase.MonsterType[] allowedMonsterTypes; // 허용된 몬스터 타입
    }

    public MapData[] maps; // 여러 맵 데이터를 저장
}
