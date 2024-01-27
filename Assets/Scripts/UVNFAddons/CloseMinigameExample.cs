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
		SceneManager.UnloadSceneAsync(gameObject.scene);

		GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();

		foreach (GameObject obj in allObjects)
		{
			// Set each GameObject and its children to inactive
			obj.SetActive(true);
		}
	}
}
