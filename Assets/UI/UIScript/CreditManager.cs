using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditManager : MonoBehaviour
{
    public GameObject CreditMenu;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Credit()
    {
        CreditMenu.SetActive(true);
    }
    public void ExitCredit()
    {
        CreditMenu.SetActive(false);
    }
}
