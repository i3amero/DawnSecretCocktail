using UnityEngine;
using System.Collections;

public class CreditsManager : MonoBehaviour
{
    public CanvasGroup[] gameTitle; // 크레딧에 표시될 CanvasGroup들
    public GameObject endButton; // 크레딧이 끝난 후 나타날 버튼
    public float fadeDuration = 1.5f;
    public float waitDuration = 2.0f;

    void Start()
    {
        endButton.SetActive(false); // 시작할 때 버튼 숨기기
        StartCoroutine(PlayCredits());
    }

    IEnumerator PlayCredits()
    {
        foreach (CanvasGroup title in gameTitle)
        {
            yield return StartCoroutine(FadeInOut(title));
        }

        // 모든 크레딧이 끝난 후 버튼 활성화
        endButton.SetActive(true);
    }

    IEnumerator FadeInOut(CanvasGroup canvasGroup)
    {
        float time = 0;

        // 페이드 인
        while (time < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(waitDuration);

        // 페이드 아웃
        time = 0;
        while (time < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;
    }
}
