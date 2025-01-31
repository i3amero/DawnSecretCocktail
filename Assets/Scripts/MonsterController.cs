using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float spawnTime { get; private set; } // 몬스터 스폰 시간
    public MonsterDatabase.MonsterData MonsterData { get; private set; }

    public void Initialize(MonsterDatabase.MonsterData data)
    {
        spawnTime = Time.time; // 현재 시간 저장
        MonsterData = data;
    }
}
