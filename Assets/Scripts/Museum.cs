using UnityEngine;
using UnityEngine.UI;

public class PanelToggle : MonoBehaviour
{
    public GameObject panel; 
    public Button museumButton; 

    void Start()
    {
        // 버튼에 클릭 이벤트 추가
        if (museumButton != null)
            museumButton.onClick.AddListener(TogglePanel);

        // 시작할 때 Panel을 비활성화
        if (panel != null)
            panel.SetActive(false);
    }

    public void TogglePanel()
    {
        // 패널이 비활성화된 경우 활성화, 활성화된 경우 비활성화
        if (panel != null)
        {
            panel.SetActive(!panel.activeSelf);
        }
    }
    public void ClosePanel()
    {
        // 패널이 비활성화된 경우 활성화, 활성화된 경우 비활성화
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}
