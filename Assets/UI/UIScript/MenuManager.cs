using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public Sprite MenuButtonUp;
    public Sprite MenuButtonDown;
    public GameObject MemoOb;
    public GameObject CardOb;
    public GameObject PhoneOb;
    public GameObject Buttons;
    public AudioClip ClickClip;
    public AudioClip BagClip;
    public AudioClip PhoneClip;
    public AudioClip MemoClip;
    public AudioClip CardClip;
    private AudioSource SeSource;
    void Start()
    {
        SeSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SeSource.PlayOneShot(ClickClip);
        }
    }
    public void Menu()
    {
        if (this.gameObject.GetComponent<Image>().sprite.name == "MenuButtonUp")
        {
            SeSource.PlayOneShot(BagClip);
            this.gameObject.GetComponent<Image>().sprite = MenuButtonDown;
            Buttons.SetActive(true);
        }   
else if (this.gameObject.GetComponent<Image>().sprite.name == "MenuButtonDown")
        {
            SeSource.PlayOneShot(BagClip);
            this.gameObject.GetComponent<Image>().sprite = MenuButtonUp;
            Buttons.SetActive(false);
            CardOb.SetActive(false);
            MemoOb.SetActive(false);
            PhoneOb.SetActive(false);
        }
    }
    public void Memo()
    {
        SeSource.PlayOneShot(MemoClip);
        MemoOb.SetActive(true);
        PhoneOb.SetActive(false);
        CardOb.SetActive(false);

    }
    public void PhoneOption()
    {
        SeSource.PlayOneShot(PhoneClip);
        PhoneOb.SetActive(true);
        MemoOb.SetActive(false);
        CardOb.SetActive(false);
    }   
    public void Card()
    {
        SeSource.PlayOneShot(CardClip);
        CardOb.SetActive(true);
        MemoOb.SetActive(false);
        PhoneOb.SetActive(false);
    }
}
