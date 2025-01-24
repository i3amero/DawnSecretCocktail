using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;

    private void Start()
    {
        // GameController의 점수를 불러와 UI 업데이트
        if (GameController.Instance != null)
        {
            UpdateScoreText(GameController.Instance.Score);
        }
        else
        {
            Debug.LogError("GameController 인스턴스를 찾을 수 없습니다.");
        }
    }

    private void Update()
    {
        if (GameController.Instance != null && GameController.Instance.CurrentState == GameState.Running)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameController.Instance.AddScore(10); // GameController로 점수 추가
                UpdateScoreText(GameController.Instance.Score);
            }
        }
    }

    private void UpdateScoreText(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
