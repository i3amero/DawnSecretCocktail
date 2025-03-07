// 추가적인 기능 확장을 이 스크립트에 구현한다는 뜻인지까진 모르겠으나,
// 버튼을 눌렀을 때의 다양한 기능의 확장을 원한다면, 본 스크립트를 추상 클래스로 두고,
// 상속받아서 구현하는 방식으로 확장성을 높일 수 있을 것. 단, 이때도 단일 책임 원칙을 잘 지켜야 함.
using UnityEngine;

// ** 종료 버튼을 클릭했을 때 데이터 저장같은 추가적인 기능 확장을 위해 따로 만든 스크립트 **
public class CloseButtonHandler: MonoBehaviour
{
    public string targetScene; // 넘어갈 씬 이름
    public void OnCloseButtonClick()
    {
        Debug.Log("버튼이 클릭되었습니다.");

        if (SceneController.Instance != null)
        {
            SceneController.Instance.LoadSceneWithFadeOut(targetScene);
        }
        else
        {
            Debug.LogError("SceneController Instance is null. Check if SceneController exists in the scene.");
        }
    }

    public void OnGameStartButtonClick() // 무한 모드 게임을 재시작하는 버튼을 누를 때 실행되는 함수
    {
        Debug.Log("버튼이 클릭되었습니다.");

        if (SceneController.Instance != null)
        {
            SceneController.Instance.LoadSceneWithFadeOut(targetScene, () =>
            {
                // 페이드 인까지 끝난 뒤 실행할 로직
                GameController.Instance.gameMode = GameMode.Infinite;
                GameController.Instance.sceneName = "InfiniteResultScene";

                // 새 씬의 UI를 찾기
                GameController.Instance.FindNewSceneUI();
               

                GameController.Instance.InitializeGame();
            });
        }
        else
        {
            Debug.LogError("SceneController Instance is null. Check if SceneController exists in the scene.");
        }
    }

    public void OnTutorialStartButtonClick() // 튜토리얼 게임을 재시작하는 버튼을 누를 때 실행되는 함수
    {
        Debug.Log("버튼이 클릭되었습니다.");

        if (SceneController.Instance != null)
        {
            SceneController.Instance.LoadSceneWithFadeOut(targetScene, () =>
            {
                // 페이드 인까지 끝난 뒤 실행할 로직
                GameController.Instance.gameMode = GameMode.Tutorial;
                GameController.Instance.sceneName = "Tutorial";

                // 새 씬의 UI를 찾기
                GameController.Instance.FindNewSceneUI();
                

                GameController.Instance.InitializeGame();
            });
        }
        else
        {
            Debug.LogError("SceneController Instance is null. Check if SceneController exists in the scene.");
        }
    }
}
