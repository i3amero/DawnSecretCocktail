// 데이터 저장용 클래스라면, MonoBehaviour가 없어도 될 것 같음.
// 그냥... 기우일 수 있는데, MonoBehaviour을 제거하고 데이터 저장용으로 쓴다면, 지금 써있는 주석에 따르면
// 무조건 new키워드로 인스턴스해서 써야함. 아니면 클래스는 참조타입이라 static처럼 움직임
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
