using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheaterMenu : MonoBehaviour
{
    public void LoadScenarioScene()
    {
        SceneManager.LoadScene("ScenarioScene");
    }
}
