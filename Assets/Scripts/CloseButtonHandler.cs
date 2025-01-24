using UnityEngine;

public class CloseButtonHandler : MonoBehaviour
{
    public void OnCloseButtonClick()
    {
        string targetScene = "SampleScene"; // 넘어갈 씬 이름

        Debug.Log("Close 버튼이 클릭되었습니다.");

        if (SceneController.Instance != null)
        {
            SceneController.Instance.LoadScene(targetScene);
        }
        else
        {
            Debug.LogError("SceneController Instance is null. Check if SceneController exists in the scene.");
        }
    }
}
