using UnityEngine;

// ** 종료 버튼을 클릭했을 때 데이터 저장같은 추가적인 기능 확장을 위해 따로 만든 스크립트 **
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
