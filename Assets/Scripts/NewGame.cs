using UnityEngine;
using UnityEngine.UI;

public class ObjectDeleter : MonoBehaviour
{
    public Button deleteButton; // 삭제 버튼

    void Start()
    {

    }
    public void DestroySingletons()
    {
        // 싱글톤 오브젝트 삭제
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            Destroy(audioManager.gameObject);
        }

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            Destroy(gameManager.gameObject);
        }
    }
}
