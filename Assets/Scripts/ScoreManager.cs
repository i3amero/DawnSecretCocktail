﻿using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text energyText;
    public TMP_Text bestScoreText;
    public TMP_Text comboText;
    public TMP_Text judgmentText;
    public TMP_Text killCountText;
    public ParticleSystem myParticleSystem;
    public int tutorialKillCount = 0; // 튜토리얼 킬 카운트
    private int score = 0;     // 현재 점수
    private int comboCount = 0; // 콤보 카운트
    private int points = 0;   // 추가할 점수
    private int bestScore = 0;  // 최고 기록 변수 추가
    private float multiplier; // 배율
    private int selectedMapID; // 선택된 맵 ID
    private float mapMultiplier; // 맵 배율

    private const float BASE_SCORE = 100;

    private void Start()
    {
        // 최고 기록 불러오기 (기본값 0)
        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        if (myParticleSystem != null)
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
        else
        {
            mapMultiplier = 1.0f; // 무한 모드와 튜토리얼은 1배율로 고정
        }

        // 처음에는 점수 UI를 숨김
        if (scoreText != null && comboText != null && killCountText != null && GameController.Instance.CurrentState == GameState.Preparation)
        {
            scoreText.gameObject.SetActive(false); // 초기에는 UI 숨기기
            comboText.gameObject.SetActive(false);
            killCountText.gameObject.SetActive(false);
            judgmentText.gameObject.SetActive(false);
        }

        if(killCountText != null && GameController.Instance.gameMode == GameMode.Tutorial)
        {
            UpdateKillCountText("0");
        }
    
        // GameController의 점수를 불러와 UI 업데이트 - 0점으로 시작
        if (GameController.Instance != null)
        {
            UpdateScoreText(GameController.Instance.Score);
            if(GameController.Instance.gameMode == GameMode.Normal && energyText != null)
            {
                UpdateEnergyText(GameController.Instance.Score);
            }
        }
        else
        {
            Debug.LogError("GameController 인스턴스를 찾을 수 없습니다.");
        }

        if(bestScoreText != null)
        {
            UpdateBestScoreText(bestScore.ToString());
        }
    }

    public void ReinitializeScoreSettings()
    {
        /*
        // 게임 모드에 따라 mapMultiplier를 재설정
        if (GameController.Instance.gameMode == GameMode.Normal)
        {
            // 일반 모드에서는 PlayerPrefs에 저장된 선택된 Map ID에 따라 배율을 설정
            selectedMapID = PlayerPrefs.GetInt("SelectedMapID", -1);
            if (selectedMapID == 2)
            {
                mapMultiplier = 1.5f;
            }
            else if (selectedMapID == 0 || selectedMapID == 1)
            {
                mapMultiplier = 1.0f;
            }
            else
            {
                Debug.LogError("선택된 맵 ID를 찾을 수 없습니다.");
            }
        }
        else if (GameController.Instance.gameMode == GameMode.Infinite)
        {
            // 무한 모드에서는 1배율로 고정
            mapMultiplier = 1.0f;
        }
        else if (GameController.Instance.gameMode == GameMode.Tutorial)
        {
            // 튜토리얼 모드에서는 점수 계산을 하지 않으므로 배율 0
            mapMultiplier = 0f;
        }

        // 내부 점수와 콤보 등의 변수 재설정
        score = 0;
        comboCount = 0;
        points = 0;
        bestScore = PlayerPrefs.GetInt("BestScore", 0); // 최고 기록 불러오기

        */
        // UI 업데이트 (각 텍스트가 null이 아닌지 확인)
        if (scoreText != null)
        {
            UpdateScoreText(score);
        }
        if (comboText != null)
        {
            UpdateComboText(comboCount);
        }
        if (killCountText != null)
        {
            UpdateKillCountText("0");
        }
        if (bestScoreText != null)
        {
            UpdateBestScoreText(bestScore.ToString());
        }
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
        if (reactionTime <= 0.75f) // PERFECT
        {
            Debug.Log("PERFECT");
            if(GameController.Instance.gameMode == GameMode.Infinite
                || GameController.Instance.gameMode == GameMode.Normal)
            {
                UpdateJudgmentText("PERFECT");
            }
            return 2.0f;
        }
        if (reactionTime <= 1.25f) // GREAT
        {
            Debug.Log("GREAT");
            if (GameController.Instance.gameMode == GameMode.Infinite
                || GameController.Instance.gameMode == GameMode.Normal)
            {
                UpdateJudgmentText("GREAT");
            }
            return 1.5f;
        }
        if (reactionTime <= 1.75f) // GOOD
        {
            Debug.Log("GOOD");
            if (GameController.Instance.gameMode == GameMode.Infinite
                || GameController.Instance.gameMode == GameMode.Normal)
            {
                UpdateJudgmentText("GOOD");
            }            
            return 1.0f;
        }
        else // BAD
        {
            Debug.Log("BAD");
            if (GameController.Instance.gameMode == GameMode.Infinite
                || GameController.Instance.gameMode == GameMode.Normal)
            {
                UpdateJudgmentText("BAD");
            }              
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
            if (GameController.Instance.gameMode == GameMode.Infinite
                || GameController.Instance.gameMode == GameMode.Normal)
            {
                UpdateJudgmentText("FAIL");
            }
                
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

        // 최고 기록 업데이트: 현재 점수가 기존 최고 기록보다 높으면 저장
        if(GameController.Instance.gameMode == GameMode.Infinite) // 일반 모드일 때만 최고 기록 갱신
        {
            if (score > bestScore)
            {
                bestScore = score;
                PlayerPrefs.SetInt("BestScore", bestScore);
                PlayerPrefs.Save();
                Debug.Log($"최고 기록 갱신: {bestScore}");
            }

            UpdateBestScoreText(bestScore.ToString());
        }
        
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

    public void UpdateKillCountText(string killCount)
    {
        if (killCountText != null)
        {
            killCountText.text = killCount + " / 3";
        }
    }

    private void UpdateBestScoreText(string bestScore)
    {
        if (bestScoreText != null)
        {
            bestScoreText.text = "BEST: " + bestScore;
        }
    }

    public void UpdateEnergyText(int energy)
    {
        if (energyText != null)
        {
            energyText.text = "획득 기운: " + energy;
        }
    }
}
