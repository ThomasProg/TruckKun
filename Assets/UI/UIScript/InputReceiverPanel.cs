using UnityEngine;
using UnityEngine.UI;

public class InputReceiverPanel : MonoBehaviour
{
    public static InputReceiverPanel Instance { get; private set; }

    public bool WasClickedThisFrame { get; private set; }
    public bool WasClickedLastFrame { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(this);
        }

        WasClickedThisFrame = false;
        WasClickedLastFrame = false;
    }

    private void Update()
    {
        WasClickedLastFrame = WasClickedThisFrame;
        WasClickedThisFrame = false;
    }

    public void OnClick()
    {
        Debug.Log("On Click");
        WasClickedThisFrame = true;
    }
}
