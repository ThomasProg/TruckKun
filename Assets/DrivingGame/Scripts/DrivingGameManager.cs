using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DrivingGameManager : MonoBehaviour
{
    public Minigame minigame;
    public DrivingSpawnManager spawnManager;
    public TruckScript truck;
    
    public int maxHitPoint = 3;
    public int currentHitPoint;
    public HorizontalLayoutGroup hitPointLayoutGroup;
    
    public int maxPickup = 5;
    public int currentPickup;
    public TextMeshProUGUI pickupDisplay;

    public GameObject endingDisplay;
    public GameObject winDisplay;
    public GameObject loseDisplay;

    private bool isEndingReached;
    
    void Start()
    {
        InitializeGame();
        spawnManager.InitializeGame();
    }

    public void InitializeGame()
    {
        currentHitPoint = maxHitPoint;
        currentPickup = 0;
        UpdateDisplay();
        isEndingReached = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }
    
    private void ResetGame()
    {
        HideEnding();
        spawnManager.Stop();
        
        InitializeGame();
        spawnManager.InitializeGame();
        truck.Reset();
    }

    public void GetPickup()
    {
        currentPickup += 1;
        if (currentPickup >= maxPickup)
        {
            ShowEnding(true);
        }
        UpdateDisplay();
    }

    public void GetHitObstacle()
    {
        currentHitPoint -= 1;
        if (currentHitPoint <= 0)
        {
            ShowEnding(false);
        }
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        pickupDisplay.text = currentPickup + "/" + maxPickup;

        for (var i = 0; i < hitPointLayoutGroup.transform.childCount; i++)
        {
            var heartFill = hitPointLayoutGroup.transform.GetChild(i).GetChild(0).GetComponent<Image>();
            heartFill.color = i < currentHitPoint ? Color.magenta : Color.black;
        }
    }

    public void ShowEnding(bool isWin)
    {
        if (isEndingReached) return;
        isEndingReached = true;
        endingDisplay.SetActive(true);
        winDisplay.SetActive(isWin);
        loseDisplay.SetActive(!isWin);

        if (isWin)
        {
            Invoke("CloseMinigame", 5f);
        }
    }

    public void HideEnding()
    {
        endingDisplay.SetActive(false);
    }

    public void CloseMinigame()
    {
        minigame.isOver = true;
    }
}
