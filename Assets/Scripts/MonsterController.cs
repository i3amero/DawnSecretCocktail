using UnityEngine;

// ** 스폰된 몬스터의 정보를 담을 클래스 **
public class MonsterController : MonoBehaviour
{
    public float spawnTime { get; private set; } // 몬스터 스폰 시간 저장 -> 몬스터가 스폰 된 후 제거하기 까지 시간 측정할 때 활용 
    public MonsterDatabase.MonsterData MonsterData { get; private set; }

    public void Initialize(MonsterDatabase.MonsterData data)
    {
        spawnTime = Time.time; // 현재 시간 저장
        MonsterData = data; // 생성된 몬스터 데이터 저장
    }
}
