using UnityEngine;
using System.Collections;
using TMPro;

public class MonsterFadeEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float fadeDuration; // 페이드 인/아웃 지속 시간
    public float moveDistance; // 등장/사라질 때 이동할 거리

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 페이드 인 (서서히 나타나기)
    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        float timer = 0;
        if (spriteRenderer == null) yield break;  // 혹시 모를 null 체크
        Color color = spriteRenderer.color;
        color.a = 0;
        spriteRenderer.color = color;

        Vector3 startPosition = transform.position - new Vector3(0, moveDistance, 0); // 살짝 아래에서 시작
        Vector3 targetPosition = transform.position; // 원래 위치

        while (timer < fadeDuration)
        {
            // ** 매 프레임마다 null 체크 **
            if (this == null || spriteRenderer == null)
                yield break;

            color.a = Mathf.Lerp(0, 1, timer / fadeDuration);
            spriteRenderer.color = color;
            transform.position = Vector3.Lerp(startPosition, targetPosition, timer / fadeDuration); // 위로 이동
            timer += Time.deltaTime;
            yield return null;
        }

        if (spriteRenderer != null)
        {
            color.a = 1;
            spriteRenderer.color = color;
            transform.position = targetPosition; // 정확한 위치로 설정
        }
            
    }

    // 페이드 아웃 (서서히 사라지기)
    public void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        float timer = 0;
        if (spriteRenderer == null) yield break;  // 혹시 모를 null 체크
        Color color = spriteRenderer.color;
        color.a = 1;
        spriteRenderer.color = color;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = transform.position - new Vector3(0, moveDistance, 0); // 아래로 이동할 위치


        while (timer < fadeDuration)
        {
            // ** 매 프레임마다 null 체크 **
            if (this == null || spriteRenderer == null)
                yield break;

            color.a = Mathf.Lerp(1, 0, timer / fadeDuration);
            spriteRenderer.color = color;
            transform.position = Vector3.Lerp(startPosition, targetPosition, timer / fadeDuration); // 아래로 이동
            timer += Time.deltaTime;
            yield return null;
        }

        if (spriteRenderer != null)
        {
            color.a = 0;
            spriteRenderer.color = color;
            transform.position = targetPosition; // 정확한 위치 설정
        }
         
    }
}
