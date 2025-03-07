using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class UIFadeEffect : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public float fadeDuration;
    public float moveDistance;

    private RectTransform rectTransform;
    private Vector2 originalPos;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();  // RectTransform 가져오기
        originalPos = rectTransform.anchoredPosition;  // 처음 위치 저장
    }

    public IEnumerator FadeIn()
    {
        // 시작 상태
        canvasGroup.alpha = 0f;

        // 현재 anchoredPosition에서 moveDistance만큼 아래로 내린 상태를 시작점으로 삼음
        Vector2 startPos = originalPos - new Vector2(0, moveDistance);
        Vector2 targetPos = originalPos;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;

            // 알파값 서서히 0 -> 1
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            // 위치 서서히 아래 -> 원래 위치
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);

            timer += Time.deltaTime;
            yield return null;
        }

        // 최종 값 보정
        canvasGroup.alpha = 1f;
        rectTransform.anchoredPosition = targetPos;
    }

    public IEnumerator FadeOut()
    {
        // 시작 상태
        canvasGroup.alpha = 1f;

        // 현재 anchoredPosition에서 moveDistance만큼 아래로 이동할 목표
        Vector2 startPos = originalPos;
        Vector2 targetPos = originalPos - new Vector2(0, moveDistance);

        float timer = 0f;
        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;

            // 알파값 서서히 1 -> 0
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            // 위치 서서히 현재 -> 아래
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);

            timer += Time.deltaTime;
            yield return null;
        }

        // 최종 값 보정
        canvasGroup.alpha = 0f;
        rectTransform.anchoredPosition = targetPos;
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }
}
