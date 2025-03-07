using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 전역 싱글톤이 너무 많아용! 이렇게 많이 쓰면 나중에 유지보수가 어려워질 수 있어요.
/// 메모리 낭비와 가비지 컬렉션은 덤. 디스코드에 코드 올릴테니까, 그거 최대한 활용하면 좋을 거 같아요.
/// </summary>
public class CocktailSaver : MonoBehaviour
{
   public static CocktailSaver Instance { get; private set; }
   private Sprite cocktailSprite;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCocktailImage(Sprite sprite)
    {
        cocktailSprite = sprite;
    }
    public Sprite GetCocktailImage()
    {
        return cocktailSprite;
    }
}
