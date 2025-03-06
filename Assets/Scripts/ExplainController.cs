using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplainController : MonoBehaviour
{
    public GameObject SchoolExplain;
    public GameObject MuseumExplain;
    public GameObject TheaterExplain;
    public GameObject MarketExplain;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Popup1()
    {
        SchoolExplain.SetActive(true);
    }

    public void Popup2()
    {
        MuseumExplain.SetActive(true);
    }

    public void Popup3()
    {
        TheaterExplain.SetActive(true);
    }

    public void Popup4()
    {
        MarketExplain.SetActive(true);
    }

    public void Disable1()
    {
        SchoolExplain.SetActive(false);
    }

    public void Disable2()
    {
        MuseumExplain.SetActive(false);
    }

    public void Disable3()
    {
        TheaterExplain.SetActive(false);
    }

    public void Disable4()
    {
        MarketExplain.SetActive(false);
    }
}
