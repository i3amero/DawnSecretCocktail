using UnityEngine;

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

        // 게임 진행 씬으로 전환
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
