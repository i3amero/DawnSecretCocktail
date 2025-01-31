using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    private int score = 0;     // 현재 점수
    private int comboCount = 0; // 콤보 카운트
    private int points = 0;   // 추가할 점수
    private float multiplier; // 배율

    private const float BASE_SCORE = 100;

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

    // 판정에 따른 배율 계산
    private float GetJudgmentMultiplier(float reactionTime)
    {
        if (reactionTime <= 2f) // PERFECT
        {
            Debug.Log("PERFECT");
            return 2.0f;
        }
        if (reactionTime <= 5f) // GREAT
        {
            Debug.Log("GREAT");
            return 1.5f;
        }
        if (reactionTime <= 8f) // GOOD
        {
            Debug.Log("GOOD");
            return 1.0f;
        }
        else // BAD
        {
            Debug.Log("BAD");
            return 0.5f; 
        }
    }

    // 스킬 성공 여부에 따라 처리
    public void OnSkillSuccess(float reactionTime, bool isSuccess)
    {
        if (isSuccess)
        {
            multiplier = GetJudgmentMultiplier(reactionTime);
            if (multiplier == 0.5f) // BAD일때
            {
                ResetCombo();
                points = Mathf.RoundToInt(BASE_SCORE * multiplier); // 반올림 처리
                AddScore(points); // 임시 점수 추가
            }
            else // GOOD, GREAT, PERFECT일때
            {
                IncreaseCombo();
                points = Mathf.RoundToInt(BASE_SCORE * multiplier * (1.00f + comboCount * 0.01f)); // 반올림 처리
                AddScore(points); // 임시 점수 추가
            }
        }
        else
        {
            ResetCombo();
        }
    }

    // 점수 추가    
    private void AddScore(int points)
    {
        score += points;
        GameController.Instance.AddScore(points);
        Debug.Log($"증가된 점수: {points}");
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
