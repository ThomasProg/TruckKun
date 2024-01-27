using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Minigame : MonoBehaviour
{
	public bool isOver = false;

	public void FinishMinigame()
	{
		isOver = true;
	}
}
