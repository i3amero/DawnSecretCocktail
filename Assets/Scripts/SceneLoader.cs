﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneWithBGMStop(string sceneName)
    {
        // AudioManager가 존재하면 제거
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            Destroy(audioManager.gameObject);
        }
        if(GameManager.Instance.remainingDays==0)
        {
            SceneController.Instance.LoadSceneWithFadeOut("GameOver");
        }

        // 씬 전환
        if (SceneController.Instance != null)
        {
            SceneController.Instance.LoadSceneWithFadeOut(sceneName);
        }
    }
}
