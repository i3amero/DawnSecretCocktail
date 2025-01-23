using UnityEngine;

[CreateAssetMenu(fileName = "MapDatabase", menuName = "Database/MapDatabase", order = 1)]
public class MapDatabase : ScriptableObject
{
    [System.Serializable]
    public class MapData
    {
        public string mapName;
        public float scoreMultiplier;
        public Sprite thumbnail;
        public GameObject mapPrefab;
        // public MonsterSpawnData[] monsterSpawns; // 특정 맵의 몬스터 정보
    }

    public MapData[] maps; // 여러 맵 데이터를 저장
}