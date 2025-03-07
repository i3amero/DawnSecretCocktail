using UnityEngine;

// 전역 변수 및 게임 진행 정보를 관리하는 싱글톤 클래스 입니다.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("게임 진행 정보")]
    public int remainingDays = 15;
    public bool[] purchasedMasterSkills; // 밤놀이 마스터스킬 구매 여부
    public int unidentifiedEnergy = 0;          // 보유 재화(정체불명의 기운)
    public bool[] purchasedCharacterSkills; // 캐릭터 마스터스킬 구매 여부

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
