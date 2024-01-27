using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Sprite MenuButtonUp;
    public Sprite MenuButtonDown;
    public GameObject Buttons;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Menu()
    {
        if (this.gameObject.GetComponent<Image>().sprite.name == "MenuButtonUp")
        {
            this.gameObject.GetComponent<Image>().sprite = MenuButtonDown;
            Buttons.SetActive(true);
        }   
else if (this.gameObject.GetComponent<Image>().sprite.name == "MenuButtonDown")
        {
            this.gameObject.GetComponent<Image>().sprite = MenuButtonUp;
            Buttons.SetActive(false);
        }
    }
}
