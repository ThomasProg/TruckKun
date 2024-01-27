using System.Collections;
using UnityEngine;
using UnityEditor;
using UVNF.Core.UI;
using UVNF.Extensions;
using UnityEngine.SceneManagement;

namespace UVNF.Core.Story.Utility
{
	public class LoadScene : StoryElement
	{
		public override string ElementName => "Load Scene";

		public override Color32 DisplayColor => _displayColor;
		private Color32 _displayColor = new Color32().Utility();

		public override StoryElementTypes Type => StoryElementTypes.Utility;

		public string scene = "Scene Name";

#if UNITY_EDITOR
		public override void DisplayLayout(Rect layoutRect, GUIStyle label)
		{
			scene = EditorGUILayout.TextField(scene);
		}
#endif

		public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
		{
			SceneManager.LoadScene(scene);

			return null;
		}
	}
}