using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreditsManager : MonoBehaviour
{
    public CanvasGroup gameTitle1;
    public CanvasGroup gameTitle2;
    public CanvasGroup gameTitle3;
    public CanvasGroup gameTitle4;
    public CanvasGroup gameTitle5;
    public CanvasGroup gameTitle6;
    public CanvasGroup gameTitle7;
    public CanvasGroup gameTitle8;
    public CanvasGroup gameTitle9;
    public CanvasGroup gameTitle10;
    public CanvasGroup gameTitle11;
    public float fadeDuration = 1.5f;
    public float waitDuration = 2.0f;
    public float scrollSpeed = 50f;

    void Start()
    {
        StartCoroutine(PlayCredits());
    }

    IEnumerator PlayCredits()
    {
        yield return StartCoroutine(FadeInOut(gameTitle1));
        yield return StartCoroutine(FadeInOut(gameTitle2));
        yield return StartCoroutine(FadeInOut(gameTitle3));
        yield return StartCoroutine(FadeInOut(gameTitle4));
        yield return StartCoroutine(FadeInOut(gameTitle5));
        yield return StartCoroutine(FadeInOut(gameTitle6));
        yield return StartCoroutine(FadeInOut(gameTitle7));
        yield return StartCoroutine(FadeInOut(gameTitle8));
        yield return StartCoroutine(FadeInOut(gameTitle9));
        yield return StartCoroutine(FadeInOut(gameTitle10));
        yield return StartCoroutine(FadeInOut(gameTitle11));

    }

    IEnumerator FadeInOut(CanvasGroup canvasGroup)
    {
        // 페이드 인
        float time = 0;
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
