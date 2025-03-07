using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FinalButtonController : MonoBehaviour
{
    public GameObject FinalButton;
    public TMP_Text LeftDay;
    void Start()
    {
        int daysLeft = GameManager.Instance.remainingDays;
        LeftDay.text= ($"\"그 날\"까지 {daysLeft}일");

        bool isUnlocked1 = PlayerPrefs.GetInt("NightScenario_카타르시스", 0) == 1;
        bool isUnlocked2 = PlayerPrefs.GetInt("NightScenario_데드리프트", 0) == 1;
        bool isUnlocked3 = PlayerPrefs.GetInt("NightScenario_핀투라", 0) == 1;
        bool isUnlocked4 = PlayerPrefs.GetInt("NightScenario_레조나", 0) == 1;

        if (isUnlocked1 && isUnlocked2 && isUnlocked3 && isUnlocked4)
        {
            FinalButton.SetActive(true);
        }
        else
        {
            FinalButton.SetActive(false);
        }
    }

   
}
