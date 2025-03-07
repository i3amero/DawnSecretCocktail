using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogoFader : MonoBehaviour
{
    public Image logoImage; // 로고 UI Image
    public float fadeDuration = 2.0f; // 페이드 인/아웃 시간
    public float displayTime = 1.0f; // 유지 시간
    public string nextSceneName = "NextScene"; // 다음 씬 이름

    private void Start()
    {
        StartCoroutine(PlayLogoSequence());
    }

    IEnumerator PlayLogoSequence()
    {
        yield return StartCoroutine(Fade(0, 1, fadeDuration)); // Fade In
        yield return new WaitForSeconds(displayTime); // 잠시 유지
        yield return StartCoroutine(Fade(1, 0, fadeDuration)); // Fade Out
        SceneManager.LoadScene(nextSceneName); // 다음 씬으로 전환
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float time = 0;
        Color color = logoImage.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            logoImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        logoImage.color = new Color(color.r, color.g, color.b, endAlpha); // 최종 설정
    }
}
