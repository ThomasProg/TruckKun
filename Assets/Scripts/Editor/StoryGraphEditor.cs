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
using UVNF.Core.Story.Character;

[System.Serializable]
class B
{
	public Dictionary<string, object> scenario;
};

[CustomEditor(typeof(StoryGraph))]
public class StoryGraphEditor : UnityEditor.Editor
{
	[SerializeField]
	string assetDestinationPath = "Assets/JSON/ExampleStory.asset";
	[SerializeField]
	string assetSourcePath = "Assets/JSON/ExampleStory.json";

	Sprite PathToSprite(string spritePath)
	{
		return (Sprite)AssetDatabase.LoadAssetAtPath(spritePath, typeof(Sprite));

	}

	Sprite GetBackgroundSpriteFromKey(string key)
	{
		Dictionary<string, Sprite> strToBackground = new Dictionary<string, Sprite>
		{
			{ "hallway", PathToSprite("Assets/Backgrounds/bg_hallway.png") },
			{ "classroom", PathToSprite("Assets/Backgrounds/bg_class.png") },
			{ "councilroom", PathToSprite("Assets/Backgrounds/bg_hallway.png") },
			{ "summer festval", PathToSprite("Assets/Backgrounds/bg_festival.png") },
			{ "main street of the city", PathToSprite("Assets/Backgrounds/bg_autumntown.png") },
			{ "Night City", PathToSprite("Assets/Backgrounds/bg_winter.png") }
		};

		Sprite sprite;
		if (strToBackground.TryGetValue(key, out sprite))
		{
			return sprite;
		}
		else
		{
			Debug.LogWarning("GetBackgroundSpriteFromKey() : invalid background : " + key);
			return null;

		}
	}

	Sprite GetCharacterSpriteFromKey(string key)
	{
		Dictionary<string, Sprite> strToSprite = new Dictionary<string, Sprite>
		{
			{ "truckkun_seifuku_normal", PathToSprite("Assets/Characters/truckkun/truckkun_vest_normal.png") },
			{ "truckkun_seifuku_laugh", PathToSprite("Assets/Characters/truckkun/truckkun_vest_laugh.png") },
			{ "truckkun_seifuku_blush", PathToSprite("Assets/Characters/truckkun/truckkun_vest_blush.png") },
			{ "truckkun_yukata_annoyed", PathToSprite("Assets/Characters/truckkun/truckkun_yukata_annoyed.png") },
			{ "truckkun_yukata_blush", PathToSprite("Assets/Characters/truckkun/truckkun_yukata_blush.png") },
			{ "truckkun_yukata_normal", PathToSprite("Assets/Characters/truckkun/truckkun_yukata_normal.png") },
			{ "truckkun_yukata_laugh", PathToSprite("Assets/Characters/truckkun/truckkun_yukata_laugh.png") },
			{ "truckkun_winter_normal", PathToSprite("Assets/Characters/truckkun/truckkun_winter_normal.png") },
			{ "truckkun_winter_laugh", PathToSprite("Assets/Characters/truckkun/truckkun_winter_laugh.png") },
			{ "truckkun_winter_blush", PathToSprite("Assets/Characters/truckkun/truckkun_winter_blush.png") }
		};

		return strToSprite[key];
	}


	void ExportJson(StoryGraph storyGraph, string subFolder)
	{
		// Add a custom button to the inspector
		if (GUILayout.Button("Export to JSON"))
		{
			//string str = JsonConvert.SerializeObject<Dictionary<string, object>>(e.ToString());

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

	void ImportScene(StoryGraph storyGraph, string sceneStr, Vector2 position)
	{
		object[] events = JsonConvert.DeserializeObject<object[]>(sceneStr);

		List<StoryElement> nodes = new List<StoryElement>();
		Dictionary<int, StoryElement> idToNode = new Dictionary<int, StoryElement>();
		Dictionary<StoryElement, int> nodeToNext_id = new Dictionary<StoryElement, int>();

		Dictionary<StoryElement, List<int>> choicesToNext_id = new Dictionary<StoryElement, List<int>>();

		StartElement startElement = storyGraph.AddNode<StartElement>();
		startElement.IsRoot = true;
		nodes.Add(startElement);

		foreach (object e in events)
		{
			Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.ToString());

			// Parse Node

			List<StoryElement> newNodes = new List<StoryElement>();

			// ChangeBackground
			object backgroundObject;
			if (dic.TryGetValue("Background", out backgroundObject))
			{
				string backgroundID = backgroundObject as string;
				ChangeBackground changeBackground = storyGraph.AddNode<ChangeBackground>();
				changeBackground.backgroundName = backgroundID;

				changeBackground.background = GetBackgroundSpriteFromKey(backgroundID);

				//ExitSceneElement exitScene = storyGraph.AddNode<ExitSceneElement>();
				//exitScene.CharacterName = "megane";
				//newNodes.Add(exitScene);

				newNodes.Add(changeBackground);

				//EnterSceneElement enterScene = storyGraph.AddNode<EnterSceneElement>();
				//enterScene.CharacterName = "megane";
				//newNodes.Add(enterScene);
			}

			// EnterScene
			object enterScene;
			if (dic.TryGetValue("EnterCharacter", out enterScene))
			{
				string spriteID = enterScene as string;
				EnterSceneElement sceneElement = storyGraph.AddNode<EnterSceneElement>();

				sceneElement.CharacterName = "megane";
				sceneElement.Character = GetCharacterSpriteFromKey(spriteID);

				newNodes.Add(sceneElement);
			}

			// ExitScene
			object exitScene;
			if (dic.TryGetValue("ExitCharacter", out exitScene))
			{
				string spriteID = exitScene as string;
				ExitSceneElement sceneElement = storyGraph.AddNode<ExitSceneElement>();

				sceneElement.CharacterName = "megane";
				//sceneElement.Character = GetCharacterSpriteFromKey(spriteID);

				newNodes.Add(sceneElement);
			}

			// ChangeSpriteElement
			object characterObject;
			if (dic.TryGetValue("Character", out characterObject))
			{
				string spriteID = characterObject as string;
				ChangeSpriteElement changeSpriteElement = storyGraph.AddNode<ChangeSpriteElement>();
				changeSpriteElement.CharacterName = "megane";
				changeSpriteElement.NewSprite = GetCharacterSpriteFromKey(spriteID);

				newNodes.Add(changeSpriteElement);
			}

			object speaker;
			if (dic.TryGetValue("Speaker", out speaker))
			{
				// ChoiceElement
				object choices;
				if (dic.TryGetValue("choices", out choices))
				{
					ChoiceElement choiceElem = storyGraph.AddNode<ChoiceElement>();
					newNodes.Add(choiceElem);

					object[] choicesList = JsonConvert.DeserializeObject<object[]>(choices.ToString());

					List<int> next_idList = new List<int>();
					foreach (object choice in choicesList)
					{
						choiceElem.AddChoice();

						Dictionary<string, object> choiceAttributes = JsonConvert.DeserializeObject<Dictionary<string, object>>(choice.ToString());

						object next_id;
						if (choiceAttributes.TryGetValue("next_id", out next_id))
						{
							next_idList.Add(JsonConvert.DeserializeObject<int>(next_id.ToString()));
							//choiceElem.AddChoice();
							//dialogue.Dialogue = next_id.ToString();
						}
						else
						{
							Debug.LogError("Missing next_id in choices");
						}

						object jp;
						if (choiceAttributes.TryGetValue("JP", out jp))
						{
							choiceElem.Choices[choiceElem.Choices.Count - 1] = jp.ToString();
							//dialogue.Dialogue = jp.ToString();
						}

						object en;
						if (choiceAttributes.TryGetValue("EN", out en))
						{
							// @TODO 
						}
					}

					choicesToNext_id.Add(choiceElem, next_idList);
				}
				// DialogueElement
				else
				{
					DialogueElement dialogue = storyGraph.AddNode<DialogueElement>();
					newNodes.Add(dialogue);

					object speakerObj;
					if (dic.TryGetValue("Speaker", out speakerObj))
					{
						dialogue.CharacterName = speakerObj.ToString();
					}

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

			foreach (StoryElement newNode in newNodes)
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
		for (int i = 0; i < nodes.Count; i++)
		{
			StoryElement node = nodes[i];
			int next_id;

			// If there is a next_id
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
			// else
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

		// Choice Nodes
		foreach (KeyValuePair<StoryElement, List<int>> pair in choicesToNext_id)
		{
			pair.Key.ClearDynamicPorts();
			int i = 0;
			foreach (int index in pair.Value)
			{
				// Get output and input ports
				string portName = "Choice" + i;
				NodePort fromPort = pair.Key.AddDynamicOutput(typeof(NodePort), ConnectionType.Override, TypeConstraint.Inherited, portName);
				StoryElement next;
				if (idToNode.TryGetValue(index, out next))
				{
					NodePort toPort = next.GetInputPort("PreviousNode");

					// Make sure the ports are not null
					if (fromPort != null && toPort != null)
					{
						// Connect the nodes
						fromPort.Connect(toPort);
						EditorUtility.SetDirty(storyGraph);
					}
				}
				else
				{
					Debug.LogError("Choice leading to id " + index + " , but this id doesn't exist.");
				}

				i++;
			}
		}

		// Generate the graph from the input
		foreach (StoryElement node in nodes)
		{
			AssetDatabase.AddObjectToAsset(node, storyGraph);
		}

		storyGraph.RefreshStories();
	}

	void ImportJsonNextFrame(StoryGraph storyGraph)
	{
		// Add a custom button to the inspector
		if (GUILayout.Button("Import to JSON"))
		{
			EditorApplication.delayCall += () => ImportJson(storyGraph, assetDestinationPath, assetSourcePath);

		}
	}

	void ImportJson(StoryGraph s, string assetDestinationPath , string assetSourcePath)
	{
		// Create an instance of the ScriptableObject
		StoryGraph storyGraph = ScriptableObject.CreateInstance<StoryGraph>();

		// Create the asset at the specified path
		AssetDatabase.CreateAsset(storyGraph, assetDestinationPath);



		string jsonFolder = assetSourcePath;

		if (!Directory.Exists(Path.GetDirectoryName(jsonFolder)))
		{
			//Directory.CreateDirectory(jsonFolder);
			Debug.LogError("Folder doesn't exist: " + Path.GetDirectoryName(jsonFolder));
		}

		string fileContents = File.ReadAllText(assetSourcePath);

		object scenarioStr;
		if (JsonConvert.DeserializeObject<Dictionary<string, object>>(fileContents).TryGetValue("scenario", out scenarioStr))
		{
			object scenesStr;
			if (JsonConvert.DeserializeObject<Dictionary<string, object>>(scenarioStr.ToString()).TryGetValue("scenes", out scenesStr))
			{
				//storyGraph.nodes.Clear();
				storyGraph.Clear();
				object[] scenes = JsonConvert.DeserializeObject<object[]>(scenesStr.ToString());
				Vector2 pos = new Vector2();
				foreach (object scene in scenes)
				{
					ImportScene(storyGraph, scene.ToString(), pos);
					EditorUtility.SetDirty(storyGraph);
					pos.y += 500;
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

		ExportJson(storyGraph, assetSourcePath);

		GUILayout.Space(10);

		GUILayout.BeginHorizontal();
		GUILayout.Label("From : ");
		// Specify the asset path where you want to save it
		assetSourcePath = GUILayout.TextField(assetSourcePath);

		GUILayout.EndHorizontal();

		GUILayout.Space(5);

		GUILayout.BeginHorizontal();
		GUILayout.Label("To : ");
		// Specify the asset path where you want to save it
		assetDestinationPath = GUILayout.TextField(assetDestinationPath);

		GUILayout.EndHorizontal();

		ImportJsonNextFrame(storyGraph);
	}
}
