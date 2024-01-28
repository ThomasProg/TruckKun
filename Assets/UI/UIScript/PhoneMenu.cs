using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class PhoneMenu : MonoBehaviour
{
  
    public Toggle AutoToggle;
    public Button MainMenuButton;

    private void Start()
    {

        
    }
 
    public void OnClickMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
