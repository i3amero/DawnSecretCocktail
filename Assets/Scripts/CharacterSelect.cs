using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 대강 짐작으로는 어딘가 오브젝트에 상속시켜서 뭔가 조건이 발동하면(아마... 버튼 클릭 시 겠죠?)
/// 그러면 당장에는 문제가 없겠는데, PlayerPrefs를 쓴거라 데이터가 씬 전환되도 안 날라가는거지, 
/// PlayerPrefs안쓰면 씬 바뀌면서 데이터가 날라갔을거라는 점 인지하면 될 것 같아요
/// </summary>
public class CharacterSelect : MonoBehaviour
{
    public void SelectCharacter(int characterIndex)
    {
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
        SceneManager.LoadScene("TalkScene");
    }
}
