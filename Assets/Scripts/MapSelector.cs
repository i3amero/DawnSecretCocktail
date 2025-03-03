using UnityEngine;

// ** 맵의 정보를 저장하고 게임 씬(GamePlay)으로 전환 **
public class MapSelector : MonoBehaviour
{
    public MapDatabase mapDatabase; // MapDatabase 연결
    public int mapID; // 버튼에 연결된 Map ID 

    public void OnMapSelected()
    {
        string targetScene = "GamePlay"; // 넘어갈 씬 이름

        // 선택된 맵 데이터를 확인
        MapDatabase.MapData selectedMap = mapDatabase.maps[mapID];
        Debug.Log($"Selected Map: {selectedMap.mapName}, Multiplier: {selectedMap.scoreMultiplier}");

        // 데이터를 저장하여 다음 씬에 전달
        PlayerPrefs.SetInt("SelectedMapID", mapID);
        PlayerPrefs.Save(); // 저장 강제 실행 (데이터 유실 방지)

        // 게임 진행 씬으로 전환
        if (SceneController.Instance != null)
        {
            GameController.Instance.gameMode = GameMode.Normal;
            GameController.Instance.sceneName = "ScoreScene";

            // 재시작 시 GameController 초기화
            SceneController.Instance.LoadSceneWithFadeOut(targetScene, () =>
            {
                // 페이드 인까지 끝난 뒤 실행할 로직
                

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
