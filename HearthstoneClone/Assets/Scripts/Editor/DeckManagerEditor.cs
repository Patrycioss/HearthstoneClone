using System;
using Deck.DeckManagement;
using UnityEditor;
using UnityEngine;

namespace Editor
{
	[UnityEditor.CustomEditor(typeof(DeckManager))]
	public class DeckManagerEditor : UnityEditor.Editor
	{
		private string deckName = "DeckName";
		private bool debugMode = true;
		
		public override async void OnInspectorGUI()
		{
			DeckManager deckManager = (DeckManager) serializedObject.targetObject;
			deckManager.Initialize();

			// Editor functions do not seem to like the async stuff so I'll just do this for development purposes.
			try
			{
				debugMode = GUILayout.Toggle(debugMode, "IsDebug");
				
				if (GUILayout.Button("Save all decks"))
				{
					deckManager.Initialize();
					Debug.Log(await deckManager.SaveAllDecks(false));
				}

				if (GUILayout.Button("Load all decks"))
				{
					deckManager.Initialize();
					Debug.Log(await deckManager.LoadAllDecks(false));
				}
				
				deckName = EditorGUILayout.TextField(deckName);
				
				if (GUILayout.Button("Load Deck"))
				{
					deckManager.Initialize();
					Debug.Log(await deckManager.LoadDeck(deckName, false));
				}
				
				base.OnInspectorGUI();
			}
#pragma warning disable CS0168
			catch (Exception e)
#pragma warning restore CS0168
			{
				// ignored
			}
		}
	}
}