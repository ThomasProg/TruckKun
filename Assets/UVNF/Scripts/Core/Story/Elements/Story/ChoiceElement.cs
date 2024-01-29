using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using UVNF.Core.UI;
using UVNF.Extensions;

namespace UVNF.Core.Story.Dialogue
{
    public class ChoiceElement : StoryElement
    {
        public override string ElementName => "Choice";

        public override Color32 DisplayColor => _displayColor;
        private Color32 _displayColor = new Color32().Story();

        public override StoryElementTypes Type => StoryElementTypes.Story;

        // Japanese
        public List<string> Choices = new List<string>();
		// English
		public List<string> ChoicesEnglish = new List<string>();

		public bool ShuffleChocies = true;
        public bool HideDialogue = false;


        public bool IsJapanese
        {
            get
            {
                return true;
            }
        }

#if UNITY_EDITOR
        // Isn't used since CustomNodeEditors is changing this node's display
        public override void DisplayLayout(Rect layoutRect, GUIStyle label)
        {
            for (int i = 0; i < Choices.Count; i++)
            {
                GUILayout.Label("Choice " + (i + 1), label);
                Choices[i] = GUILayout.TextField(Choices[i]);
                if (i < ChoicesEnglish.Count)
				    ChoicesEnglish[i] = GUILayout.TextField(ChoicesEnglish[i]);
				if (GUILayout.Button("-"))
                {
                    RemoveChoice(i);
                    return;
                }
            }

			if (GUILayout.Button("+"))
                AddChoice();

            ShuffleChocies = GUILayout.Toggle(ShuffleChocies, "Shuffle Choices");
            HideDialogue = GUILayout.Toggle(HideDialogue, "Hide Dialogue");
        }

        public void AddChoice()
        {
            Choices.Add(string.Empty);
			ChoicesEnglish.Add(string.Empty);
			AddDynamicOutput(typeof(NodePort), ConnectionType.Override, TypeConstraint.Inherited, "Choice" + (Choices.Count - 1));
        }

        public void RemoveChoice(int index)
        {
            Choices.RemoveAt(index);
			ChoicesEnglish.RemoveAt(index);
			RemoveDynamicPort(DynamicPorts.ElementAt(index));
        }
#endif

        public override IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas)
        {
            List<string> choiceList;

            if (IsJapanese)
            {
                choiceList = Choices;
                if (ShuffleChocies)
                {
                    choiceList.Shuffle();
                }
            }
            else
            {
				choiceList = ChoicesEnglish;
				if (ShuffleChocies)
				{
					choiceList.Shuffle();
				}
			}

            canvas.DisplayChoice(choiceList.ToArray(), HideDialogue);
            while (canvas.ChoiceCallback == -1) yield return null;

            if (DynamicPorts.ElementAt(canvas.ChoiceCallback).IsConnected)
            {
                int choice = canvas.ChoiceCallback;
                canvas.ResetChoice();
                managerCallback.AdvanceStoryGraph(DynamicPorts.ElementAt(choice).Connection.node as StoryElement);
            }
        }
    }
}