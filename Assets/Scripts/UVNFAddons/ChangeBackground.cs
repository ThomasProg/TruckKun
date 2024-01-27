using System.Collections;
using UnityEngine;
using UnityEditor;
using UVNF.Core.UI;
using UVNF.Extensions;
using UnityEngine.SceneManagement;

namespace UVNF.Core.Story.Utility
{
	public class ChangeBackground : StoryElement
	{
		public override string ElementName => "Change Background";

		public override Color32 DisplayColor => _displayColor;
		private Color32 _displayColor = new Color32().Utility();

		public override StoryElementTypes Type => StoryElementTypes.Utility;

		public Sprite background;

#if UNITY_EDITOR
		public override void DisplayLayout(Rect layoutRect, GUIStyle label)
		{
			background = (Sprite) EditorGUILayout.ObjectField("Background", background, typeof(Sprite), true);
		}
#endif

		public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
		{
			//SceneManager.LoadScene(scene);

			// TODO : Set background

			return null;
		}
	}
}