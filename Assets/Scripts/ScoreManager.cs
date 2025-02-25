using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text comboText;
    public TMP_Text judgmentText;
    public ParticleSystem myParticleSystem;
    private int score = 0;     // 현재 점수
    private int comboCount = 0; // 콤보 카운트
    private int points = 0;   // 추가할 점수
    private float multiplier; // 배율
    private int selectedMapID; // 선택된 맵 ID
    private float mapMultiplier; // 맵 배율

    private const float BASE_SCORE = 100;

    private void Start()
    {
        if(myParticleSystem != null)
        {
            myParticleSystem.Stop();
        }

        // 선택된 맵 데이터 불러오기
        if (GameController.Instance.gameMode == GameMode.Normal) // 일반 모드일 때
        {
            selectedMapID = PlayerPrefs.GetInt("SelectedMapID", -1); // 선택된 Map ID 가져오기
            if(selectedMapID == 2)
            {
                mapMultiplier = 1.5f;
            }
            else if(selectedMapID == 0 || selectedMapID == 1)
            {
                mapMultiplier = 1.0f;
            }
            else
            {
                Debug.LogError("선택된 맵 ID를 찾을 수 없습니다.");
            }
        }
        else if (GameController.Instance.gameMode == GameMode.Infinite)// 무한 모드일 때
        {
            mapMultiplier = 1.0f; // 무한 모드는 1배율로 고정
        }

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

    public void ResetUIForNewGame()
    {
        // 현재 게임 상태가 Preparation이라면 UI를 숨긴다.
        if (GameController.Instance != null &&
            GameController.Instance.CurrentState == GameState.Preparation)
        {
            if (scoreText != null) scoreText.gameObject.SetActive(false);
            if (comboText != null) comboText.gameObject.SetActive(false);
            if (judgmentText != null) judgmentText.gameObject.SetActive(false);
        }

        // 점수/콤보 등 내부 상태도 재설정
        score = 0;
        comboCount = 0;
        UpdateScoreText(score);
        UpdateComboText(comboCount);
    }

    // 게임이 시작될 때 숨겨놨던 UI 표시 (GameController에서 호출)
    public void ShowUI<T>(T uiElement) where T : Component
    {
        if (uiElement != null)
        {
            uiElement.gameObject.SetActive(true);
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
            if (Mathf.Approximately(multiplier, 0.5f)) // BAD일때
            {
                ResetCombo();
                points = Mathf.RoundToInt(BASE_SCORE * multiplier * mapMultiplier); // 추가될 점수 계산, 반올림 처리
                AddScore(points); // 임시 점수 추가
            }
            else // GOOD, GREAT, PERFECT일때
            {
                IncreaseCombo();
                points = Mathf.RoundToInt(BASE_SCORE * multiplier * (1.00f + comboCount * 0.01f) * mapMultiplier); // 추가될 점수 계산, 반올림 처리
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

        if(comboCount >= 2)
        {
            myParticleSystem.Play();
        }
    }

    // 콤보 초기화
    private void ResetCombo()
    {
        comboCount = 0;
        Debug.Log("콤보 초기화");
        UpdateComboText(comboCount);

        myParticleSystem.Stop();
    }

    // UI 업데이트
    private void UpdateScoreText(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + score;
        }
    }

    private void UpdateComboText(int comboCount)
    {
        if (scoreText != null)
        {
            if(comboCount == 0)
            {
                comboText.text = "";
            }
            else
            {
                comboText.text = comboCount + " COMBO";
            }
        }
    }

    private void UpdateJudgmentText(string judgment)
    {
        if (judgmentText != null)
        {
            judgmentText.color = Color.red;
            if (judgment == "PERFECT")
            {
                judgmentText.text = judgment + "!!";
            }
            else if (judgment == "GREAT")
            {
                judgmentText.text = judgment + "!";
            }
            else
            {
                judgmentText.text = judgment;
            }
            
            judgmentText.gameObject.SetActive(true);  // UI를 보이게 함
            StartCoroutine(HideJudgmentAfterTime(1f));  // 1초 후에 숨기기
        }
    }
}
