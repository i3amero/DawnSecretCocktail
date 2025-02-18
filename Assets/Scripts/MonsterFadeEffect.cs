// 이 스크립트가 정석입니다. 단일책임원칙도 잘지키고, 효과적으로 구현되어있어요.
// 한가지 조금 아쉬운거라면, 스크립트가 SpriteRenderer만 있으면 다른 모든 곳에서 사용 가능 할 듯한데, 
// 써있는건 몬스터에만 적용 가능하다는 것처럼 이름을 붙였다는 거? 그래도 이해하기 쉽게 잘 썼어요.
// 확장성이 충분한데, 그걸 이름을 좀 지엽적으로 지은 특이케이스.
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
        Color color = spriteRenderer.color;
        color.a = 0;
        spriteRenderer.color = color;

        Vector3 startPosition = transform.position - new Vector3(0, moveDistance, 0); // 살짝 아래에서 시작
        Vector3 targetPosition = transform.position; // 원래 위치

        while (timer < fadeDuration)
        {
            color.a = Mathf.Lerp(0, 1, timer / fadeDuration);
            spriteRenderer.color = color;
            transform.position = Vector3.Lerp(startPosition, targetPosition, timer / fadeDuration); // 위로 이동
            timer += Time.deltaTime;
            yield return null;
        }

        color.a = 1;
        spriteRenderer.color = color;
        transform.position = targetPosition; // 정확한 위치로 설정
    }

    // 페이드 아웃 (서서히 사라지기)
    public void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        float timer = 0;
        Color color = spriteRenderer.color;
        color.a = 1;
        spriteRenderer.color = color;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = transform.position - new Vector3(0, moveDistance, 0); // 아래로 이동할 위치


        while (timer < fadeDuration)
        {
            color.a = Mathf.Lerp(1, 0, timer / fadeDuration);
            spriteRenderer.color = color;
            transform.position = Vector3.Lerp(startPosition, targetPosition, timer / fadeDuration); // 아래로 이동
            timer += Time.deltaTime;
            yield return null;
        }

        color.a = 0;
        spriteRenderer.color = color;
        transform.position = targetPosition; // 정확한 위치 설정
    }
}
