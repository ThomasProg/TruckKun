using Palmmedia.ReportGenerator.Core.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using UVNF.Entities.Containers;
using Newtonsoft.Json;
using FullSerializer;
using UVNF.Core.Story.Utility;
using UVNF.Core.Story.Dialogue;
using UVNF.Core.Story;
using static XNode.Node;
using XNode;
using Unity.VisualScripting;
using UVNF.Core.Story.Other;
using static BayatGames.SaveGameFree.Examples.ExampleSaveCustom;
using Panda.Examples.PlayTag;

[System.Serializable]
class B
{
	public Dictionary<string, object> scenario;
};

[CustomEditor(typeof(StoryGraph))]
public class StoryGraphEditor : Editor
{
	[SerializeField]
	private string subFolder = "JSON";

	void ExportJson(StoryGraph storyGraph, string subFolder)
	{
		// Add a custom button to the inspector
		if (GUILayout.Button("Export to JSON"))
		{
			string json2 = JsonUtility.ToJson(storyGraph.nodes[0], true);
			

			string json = JsonUtility.ToJson(storyGraph, true);
			Debug.Log(json);

			string jsonFolder = Path.Combine(Application.dataPath, subFolder);

			if (!Directory.Exists(jsonFolder))
			{
				Directory.CreateDirectory(jsonFolder);
			}

			string filePath = Path.Combine(jsonFolder, storyGraph.name + ".json");

			try
			{
				// Write the string data to the file
				File.WriteAllText(filePath, json);

				Debug.Log("String saved to file: " + filePath);
			}
			catch (Exception e)
			{
				Debug.LogError("Failed to save string to file: " + e.Message);
			}
		}
	}

	void ImportScene(StoryGraph storyGraph, string sceneStr)
	{
		object[] events = JsonConvert.DeserializeObject<object[]>(sceneStr);

		List<StoryElement> nodes = new List<StoryElement>();
		Dictionary<int, StoryElement> idToNode = new Dictionary<int, StoryElement>();
		Dictionary<StoryElement, int> nodeToNext_id = new Dictionary<StoryElement, int>();

		StartElement startElement = storyGraph.AddNode<StartElement>();
		startElement.IsRoot = true;
		nodes.Add(startElement);

		foreach (object e in events)
		{
			Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.ToString());

			// Parse Node

			StoryElement newNode = null;

			object backgroundObject;
			if (dic.TryGetValue("Background", out backgroundObject))
			{
				
				string id = backgroundObject as string;
				Debug.LogWarning("TODO : conversion to ChangeBackground()");
				//newNode = ScriptableObject.CreateInstance<ChangeBackground>(); // @TODO : set background object
				newNode = storyGraph.AddNode<ChangeBackground>();
			}

			object speaker;
			if (dic.TryGetValue("Speaker", out speaker))
			{
				// is Choice
				//object choices;
				//if (dic.TryGetValue("choices", out choices))
				//{
				//	ChoiceElement choiceElem = storyGraph.AddNode<ChoiceElement>();
				//	newNode = choiceElem;

				//	object[] choicesList = JsonConvert.DeserializeObject<object[]>(choices.ToString());

				//	foreach (object choice in choicesList)
				//	{
				//		object next_id;
				//		if (dic.TryGetValue("next_id", out next_id))
				//		{
				//			choiceElem.AddChoice();
				//			choiceElem.Choices[choiceElem.Choices.Count - 1].
				//			//dialogue.Dialogue = next_id.ToString();
				//		}

				//		object jp;
				//		if (dic.TryGetValue("JP", out jp))
				//		{
				//			//dialogue.Dialogue = jp.ToString();
				//		}

				//		object en;
				//		if (dic.TryGetValue("EN", out en))
				//		{
				//			// @TODO 
				//		}
				//	}

				//	//object jp;
				//	//if (dic.TryGetValue("JP", out jp))
				//	//{
				//	//	dialogue.Dialogue = jp.ToString();

				//	//object en;
				//	//if (dic.TryGetValue("EN", out en))
				//	//{
				//	//	// @TODO 
				//	//}
				//}
				//// is Dialogue
				//else
				{
					DialogueElement dialogue = storyGraph.AddNode<DialogueElement>();
					newNode = dialogue;

					object jp;
					if (dic.TryGetValue("JP", out jp))
					{
						dialogue.Dialogue = jp.ToString();
					}

					object en;
					if (dic.TryGetValue("EN", out en))
					{
						// @TODO 
					}
				}
			}

			if (newNode)
			{
				EditorUtility.SetDirty(newNode);

				// Redirectors
				object idObject;
				if (dic.TryGetValue("id", out idObject))
				{
					int id = JsonConvert.DeserializeObject<int>(idObject.ToString());
					try
					{
						idToNode.Add(id, newNode);
					}
					catch(ArgumentException)
					{
						Debug.LogError("The id must be unique! " + id + " is assigned to multiple items.");
					}
				}

				object next_idObject;
				if (dic.TryGetValue("next_id", out next_idObject))
				{
					int next_id = JsonConvert.DeserializeObject<int>(next_idObject.ToString());
					nodeToNext_id.Add(newNode, next_id);
				}
				// End Redirectors

				// to add to graph
				nodes.Add(newNode);
			}

			// End Parse Node
		}

		// Link nodes
		Vector2 position = Vector2.zero;
		for (int i = 0; i < nodes.Count; i++)
		{
			StoryElement node = nodes[i];
			int next_id;
			if (nodeToNext_id.TryGetValue(node, out next_id))
			{
				position.y += 500;
				StoryElement nextNode = idToNode[next_id];

				// Get output and input ports
				NodePort fromPort = node.GetOutputPort("NextNode");
				NodePort toPort = nextNode.GetInputPort("PreviousNode");

				// Make sure the ports are not null
				if (fromPort != null && toPort != null)
				{
					// Connect the nodes
					fromPort.Connect(toPort);
					EditorUtility.SetDirty(storyGraph);
				}
			}
			else if (i < nodes.Count - 1)
			{
				StoryElement nextNode = nodes[i+1];

				// Get output and input ports
				NodePort fromPort = node.GetOutputPort("NextNode");
				NodePort toPort = nextNode.GetInputPort("PreviousNode");

				// Make sure the ports are not null
				if (fromPort != null && toPort != null)
				{
					// Connect the nodes
					fromPort.Connect(toPort);
					EditorUtility.SetDirty(storyGraph);
				}
			}

			node.position = position;
			position.x += 500;
		}

		// Generate the graph from the input
		foreach (StoryElement node in nodes)
		{
			AssetDatabase.AddObjectToAsset(node, storyGraph);
		}

		storyGraph.RefreshStories();
	}

	void ImportJsonNextFrame(StoryGraph storyGraph, string subFolder)
	{
		// Add a custom button to the inspector
		if (GUILayout.Button("Import to JSON"))
		{
			EditorApplication.delayCall += () => ImportJson(storyGraph, subFolder);

		}
	}

	void ImportJson(StoryGraph s, string subFolder)
	{
		// Create an instance of the ScriptableObject
		StoryGraph storyGraph = ScriptableObject.CreateInstance<StoryGraph>();

		// Specify the asset path where you want to save it
		string assetPath = "Assets/ExampleStory.asset";

		// Create the asset at the specified path
		AssetDatabase.CreateAsset(storyGraph, assetPath);



		string jsonFolder = Path.Combine(Application.dataPath, subFolder);

		if (!Directory.Exists(jsonFolder))
		{
			Directory.CreateDirectory(jsonFolder);
			Debug.LogError("Folder doesn't exist: " + jsonFolder);
		}

		string filePath = Path.Combine(jsonFolder, /*storyGraph.name*/ "ExampleStory.json");
		string fileContents = File.ReadAllText(filePath);

		object scenarioStr;
		if (JsonConvert.DeserializeObject<Dictionary<string, object>>(fileContents).TryGetValue("scenario", out scenarioStr))
		{
			object scenesStr;
			if (JsonConvert.DeserializeObject<Dictionary<string, object>>(scenarioStr.ToString()).TryGetValue("scenes", out scenesStr))
			{
				//storyGraph.nodes.Clear();
				storyGraph.Clear();
				object[] scenes = JsonConvert.DeserializeObject<object[]>(scenesStr.ToString());
				foreach (object scene in scenes)
				{
					ImportScene(storyGraph, scene.ToString());
					EditorUtility.SetDirty(storyGraph);
				}
			}
		}

		// Save the changes
		EditorUtility.SetDirty(storyGraph);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		// Import the asset to apply the changes

		//AssetDatabase.ImportAsset(storyGraph);
		//AssetDatabase.Refresh();
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		StoryGraph storyGraph = (StoryGraph)target;

		GUILayout.Space(10);

		ExportJson(storyGraph, subFolder);

		GUILayout.Space(10);

		ImportJsonNextFrame(storyGraph, subFolder);
	}
}
