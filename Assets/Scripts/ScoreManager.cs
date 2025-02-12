using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text comboText;
    public TMP_Text judgmentText;
    private int score = 0;     // 현재 점수
    private int comboCount = 0; // 콤보 카운트
    private int points = 0;   // 추가할 점수
    private float multiplier; // 배율

    private const float BASE_SCORE = 100;

    private void Start()
    {
        // 처음에는 점수 UI를 숨김
        if (scoreText != null && comboText != null && GameController.Instance.CurrentState == GameState.Preparation)
        {
            scoreText.gameObject.SetActive(false); // 초기에는 UI 숨기기
            comboText.gameObject.SetActive(false);
            judgmentText.gameObject.SetActive(false);
        }

        // GameController의 점수를 불러와 UI 업데이트 - 0점으로 시작
        if (GameController.Instance != null)
        {
            UpdateScoreText(GameController.Instance.Score);
        }
        else
        {
            Debug.LogError("GameController 인스턴스를 찾을 수 없습니다.");
        }
    }

    // 게임이 시작될 때 점수 UI 표시 (GameController에서 호출)
    public void ShowScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(true);
        }
    }

    public void ShowComboUI()
    {
        if (comboText != null)
        {
            comboText.gameObject.SetActive(true);
        }
    }

    // 판정에 따른 배율 계산
    private float GetJudgmentMultiplier(float reactionTime)
    {
        if (reactionTime <= 2f) // PERFECT
        {
            Debug.Log("PERFECT");
            UpdateJudgmentText("PERFECT");
            return 2.0f;
        }
        if (reactionTime <= 5f) // GREAT
        {
            Debug.Log("GREAT");
            UpdateJudgmentText("GREAT");
            return 1.5f;
        }
        if (reactionTime <= 8f) // GOOD
        {
            Debug.Log("GOOD");
            UpdateJudgmentText("GOOD");
            return 1.0f;
        }
        else // BAD
        {
            Debug.Log("BAD");
            UpdateJudgmentText("BAD");
            return 0.5f; 
        }
    }

    // 스킬 성공 여부에 따라 처리
    public void OnSkillSuccess(float reactionTime, bool isSuccess)
    {
        if (isSuccess) // 스킬 성공
        {
            multiplier = GetJudgmentMultiplier(reactionTime);
            if (multiplier == 0.5f) // BAD일때
            {
                ResetCombo();
                points = Mathf.RoundToInt(BASE_SCORE * multiplier); // 추가될 점수 계산, 반올림 처리
                AddScore(points); // 임시 점수 추가
            }
            else // GOOD, GREAT, PERFECT일때
            {
                IncreaseCombo();
                points = Mathf.RoundToInt(BASE_SCORE * multiplier * (1.00f + comboCount * 0.01f)); // 추가될 점수 계산, 반올림 처리
                AddScore(points); // 임시 점수 추가
            }
        }
        else // 스킬 실패
        {
            Debug.Log("FAIL");
            UpdateJudgmentText("FAIL");
            ResetCombo();
        }
    }

    private IEnumerator HideJudgmentAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (judgmentText != null)
        {
            judgmentText.gameObject.SetActive(false);
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
        UpdateComboText(comboCount);
    }

    // 콤보 초기화
    private void ResetCombo()
    {
        comboCount = 0;
        Debug.Log("콤보 초기화");
        UpdateComboText(comboCount);
    }

    // UI 업데이트
    private void UpdateScoreText(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void UpdateComboText(int comboCount)
    {
        if (scoreText != null)
        {
            comboText.text = "Combo: " + comboCount;
        }
    }

    private void UpdateJudgmentText(string judgment)
    {
        if (judgmentText != null)
        {
            judgmentText.text = judgment;
            judgmentText.gameObject.SetActive(true);  // UI를 보이게 함
            StartCoroutine(HideJudgmentAfterTime(1f));  // 1초 후에 숨기기
        }
    }
}
