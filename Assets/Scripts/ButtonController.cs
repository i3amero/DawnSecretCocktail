using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeButton : MonoBehaviour
{
    public string sceneName; // ������ ���� �̸�

    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(ChangeScene);
            button.onClick.AddListener(ExitGame); // ���� ���� ��� �߰�
        }
    }

    public void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("�� �̸��� �������� �ʾҽ��ϴ�.");
        }
    }

    public void ExitGame()
    {
        Debug.Log("���� ����");
        Application.Quit();
    }
}