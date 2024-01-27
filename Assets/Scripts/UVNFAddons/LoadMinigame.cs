using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UVNF.Core.UI;
using UVNF.Extensions;
using UnityEngine.SceneManagement;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace UVNF.Core.Story.Utility
{
	public class LoadMinigame : StoryElement
	{
		public override string ElementName => "Load Minigame";
		public override Color32 DisplayColor => _displayColor;
		private Color32 _displayColor = new Color32().Utility();
		public override StoryElementTypes Type => StoryElementTypes.Utility;

		// Default Minigames have been added because the nodes don't seem to be refreshed when new parameters are added
		// The strings are the names of the scenes
		[SerializeField]
		private string[] minigames = { "TestMiniGame", "Crane", "DrivingGame", "Minigame3", "Minigame4", "Minigame5" };

		// Use index to make sure that even if we rename a scene, it updates the nodes easily
		[SerializeField]
		private int selectedOptionIndex = 0;

		private string SelectedMinigame { get => minigames[selectedOptionIndex]; }


#if UNITY_EDITOR
		public override void DisplayLayout(Rect layoutRect, GUIStyle label)
		{
			// Display the dropdown button
			if (EditorGUILayout.DropdownButton(new GUIContent(SelectedMinigame), FocusType.Passive))
			{
				// Create the dropdown menu
				GenericMenu menu = new GenericMenu();

				for (int i = 0; i < minigames.Length; i++)
				{
					int index = i; // Need to use a temporary variable to capture the correct index
					menu.AddItem(new GUIContent(minigames[i]), i == selectedOptionIndex, () => OnOptionSelected(index));
				}

				menu.ShowAsContext();
			}
		}

		void OnOptionSelected(int index)
		{
			selectedOptionIndex = index;
		}
#endif

		public List<Canvas> deactivatedCanvas = new();
		public List<Camera> deactivatedCamera = new();
		
		public void HideMainScene()
		{
			// Find all GameObjects in the scene
			GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();

			foreach (GameObject obj in allObjects)
			{
				var objCanvas = obj.GetComponent<Canvas>();
				if (objCanvas != null)
				{
					objCanvas.enabled = false;
					deactivatedCanvas.Add(objCanvas);
				}

				var objCamera = obj.GetComponent<Camera>();
				if (objCamera != null)
				{
					objCamera.enabled = false;
					deactivatedCamera.Add(objCamera);
				}
			}
		}

		public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
		{
			// The LoadSceneAsync method returns an AsyncOperation object
			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SelectedMinigame, LoadSceneMode.Additive);

			// Allow the new scene to load in the background
			while (!asyncOperation.isDone)
			{
				float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // 0.9 is the completion value
				Debug.Log("Loading progress: " + (progress * 100) + "%");

				yield return null; // Wait for the next frame
			}

			// The scene is now loaded additively
			Debug.Log("Scene loaded additively: " + SelectedMinigame);

			HideMainScene();

			GameObject minigameGameObject = GameObject.Find("Minigame");

			Minigame minigame = null;
			if (minigameGameObject != null)
			{
				minigame = minigameGameObject.GetComponent<Minigame>();

				if (minigame == null)
				{
					Debug.LogError("Minigame component not found on " + minigameGameObject.name);
				}
			}
			else
			{
				Debug.LogError("Minigame GameObject not found in the scene.");
			}

			// Block until the minigame is over
			yield return new WaitUntil(() => minigame.isOver);

			SceneManager.UnloadSceneAsync(minigame.gameObject.scene);
			
			foreach(var objCanvas in deactivatedCanvas)
			{
				objCanvas.enabled = true;
			}

			foreach (var objCamera in deactivatedCamera)
			{
				objCamera.enabled = true;
			}
		}
	}
}