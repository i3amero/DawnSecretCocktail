using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public MonsterDatabase.MonsterData MonsterData { get; private set; }

    public void Initialize(MonsterDatabase.MonsterData data)
    {
        MonsterData = data;
    }
}
