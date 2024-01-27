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
		public string backgroundName;
        public bool Fade = true;
        public float FadeTime = 1f;

#if UNITY_EDITOR
        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
		{
			background = (Sprite) EditorGUILayout.ObjectField("Background", background, typeof(Sprite), true);

            Fade = GUILayout.Toggle(Fade, "Fade");
            if (Fade)
                FadeTime = EditorGUILayout.FloatField("Fade out time", FadeTime);
        }
#endif

		public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
		{
            if (Fade)
                canvas.ChangeBackground(background, FadeTime);
            else
                canvas.ChangeBackground(background);
            return null;
        }
	}
}