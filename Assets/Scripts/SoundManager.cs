using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource audioSource;

    [Header("Audio Clips")]
    public AudioClip clickSound;  // 버튼 클릭 사운드
    public AudioClip failSound;   // 실패 사운드
    public AudioClip successSound;// 성공 사운드
    public AudioClip selectSound; // 선택 사운드

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 삭제되지 않음
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // AudioSource 컴포넌트를 붙여서 초기화
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;  // 재생을 자동으로 시작하지 않도록
    }

    // 버튼 클릭 시
    public void PlayClickSound()
    {
        PlayOneShot(clickSound);
    }

    // 실패 시
    public void PlayFailSound()
    {
        PlayOneShot(failSound);
    }

    // 성공 시
    public void PlaySuccessSound()
    {
        PlayOneShot(successSound);
    }

    // 선택 시
    public void PlaySelectSound()
    {
        PlayOneShot(selectSound);
    }

    // 공통 함수
    private void PlayOneShot(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("재생할 AudioClip이 없습니다!");
        }
    }
}
