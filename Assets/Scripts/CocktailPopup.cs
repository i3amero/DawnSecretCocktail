using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CocktailPopup : MonoBehaviour
{
    public GameObject popupPanel;
    public Button confirmButton;
    public Button cancelButton;
    public string nextSceneName = "CocktailScene";

    void Start()
    {
        popupPanel.SetActive(false);
        cancelButton.onClick.AddListener(OnCancel);
    }

    public void ShowPopup()
    {
        popupPanel.SetActive(true);
        popupPanel.transform.SetAsLastSibling();
    }


    private void OnCancel()
    {
        popupPanel.SetActive(false);
    }
}
