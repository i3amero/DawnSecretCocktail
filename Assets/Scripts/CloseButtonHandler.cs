// 추가적인 기능 확장을 이 스크립트에 구현한다는 뜻인지까진 모르겠으나,
// 버튼을 눌렀을 때의 다양한 기능의 확장을 원한다면, 본 스크립트를 추상 클래스로 두고,
// 상속받아서 구현하는 방식으로 확장성을 높일 수 있을 것. 단, 이때도 단일 책임 원칙을 잘 지켜야 함.
using UnityEngine;

// ** 종료 버튼을 클릭했을 때 데이터 저장같은 추가적인 기능 확장을 위해 따로 만든 스크립트 **
public class CloseButtonHandler: MonoBehaviour
{
    public void OnCloseButtonClick()
    {
        string targetScene = "CocktailScene"; // 넘어갈 씬 이름

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
