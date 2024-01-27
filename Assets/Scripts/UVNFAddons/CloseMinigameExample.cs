using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseMinigame : MonoBehaviour
{
	[SerializeField]
	Minigame minigame;

    // Start is called before the first frame update
    void Start()
    {
		Invoke("DelayedAction", 2f);
	}

	void DelayedAction()
	{
		minigame.FinishMinigame();
	}
}
