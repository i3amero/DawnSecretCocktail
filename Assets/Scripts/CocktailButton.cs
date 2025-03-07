using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 객체지향적에.. 가까운? 코드라고 할 수 있겠네요. 그런데 이러한 코드가 많아지면
/// 유지보스가 굉장히 어려워 지니까, 이런 코드를 작성할 때는 주석을 달아주거나, 처음부터 스크립트의 네이밍을 잘하거나 해야합니다.
/// </summary>
public class CocktailButton : MonoBehaviour
{
    public CocktailPopup cocktailpopup;

    public void OnCharacterButtonClicked()
    {
        cocktailpopup.ShowPopup();
    }
}
