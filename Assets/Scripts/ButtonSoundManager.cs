using UnityEngine;

public class ButtonSoundManager : MonoBehaviour
{
    public static ButtonSoundManager Instance;
    private AudioSource audioSource;
    public AudioClip clickSound; // 버튼 클릭 사운드

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

        audioSource = gameObject.AddComponent<AudioSource>(); // AudioSource 추가
    }

    public void PlayClickSound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound); // 클릭 효과음 재생
        }
    }
}
