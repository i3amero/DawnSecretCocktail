using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    private int score = 0;     // 현재 점수
    private int comboCount = 0; // 콤보 카운트

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

    // 스킬 성공 여부에 따라 처리
    public void OnSkillSuccess(bool isSuccess)
    {
        if (isSuccess)
        {
            Debug.Log("스킬 성공!");
            AddScore(100); // 임시 점수 추가
            IncreaseCombo();
        }
        else
        {
            Debug.Log("스킬 실패!");
            ResetCombo();
        }
    }

    // 점수 추가
    private void AddScore(int points)
    {
        score += points;
        GameController.Instance.AddScore(points);
        UpdateScoreText(score);
    }

    // 콤보 증가
    private void IncreaseCombo()
    {
        comboCount++;
        Debug.Log($"콤보 증가: {comboCount}");
    }

    // 콤보 초기화
    private void ResetCombo()
    {
        comboCount = 0;
        Debug.Log("콤보 초기화");
    }

    private void UpdateScoreText(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
