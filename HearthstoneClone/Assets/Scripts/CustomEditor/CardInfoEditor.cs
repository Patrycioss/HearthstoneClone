using System;
using CardManagement.CardComposition;
using UnityEditor;
using UnityEngine;

namespace CustomEditor
{
	[UnityEditor.CustomEditor(typeof(CardInfo))]
	public class CardInfoEditor : Editor
	{
		private CardInfo cardInfo;
		
		public override void OnInspectorGUI()
		{
			GUILayout.Label("Base Information", EditorStyles.boldLabel);
			
			SerializedProperty cardName = serializedObject.FindProperty("CardName");
			EditorGUILayout.PropertyField(cardName);
			
			SerializedProperty manaCost = serializedObject.FindProperty("Cost");
			EditorGUILayout.PropertyField(manaCost);
			
			SerializedProperty cardType = serializedObject.FindProperty("Type");
			EditorGUILayout.PropertyField(cardType);

			GUILayout.Space(10);

			GUILayout.Label("Description", EditorStyles.boldLabel);
			cardInfo.Description = EditorGUILayout.TextArea(cardInfo.Description, GUILayout.MinHeight(100));

			GUILayout.Space(10);

			// GUILayout.Label("Get Asset Path from Asset", EditorStyles.boldLabel);
			// Object selectedObject = null;
			// selectedObject = EditorGUILayout.ObjectField(selectedObject, typeof(Object), false);
			// if (selectedObject != null) {
			// 	string assetPath = AssetDatabase.GetAssetPath(selectedObject);
			// 	cardInfo.ImagePath = assetPath;
			// }
			//
			// GUILayout.Label(cardInfo.ImagePath);

			SerializedProperty imageReference = serializedObject.FindProperty("ImageReference");
			EditorGUILayout.PropertyField(imageReference);
			
			SerializedProperty behaviours = serializedObject.FindProperty("CardBehaviourReferences");
			EditorGUILayout.PropertyField(behaviours);
			serializedObject.ApplyModifiedProperties();

			GUILayout.Space(10);
			GUILayout.Label("Base Information", EditorStyles.boldLabel);


			SerializedProperty attackProperty = serializedObject.FindProperty("Attack");
			SerializedProperty healthProperty = serializedObject.FindProperty("Health");
			
			switch (cardInfo.Type)
			{
				case CardType.Minion:
					EditorGUILayout.PropertyField(attackProperty, new GUIContent("Minion Attack"));
					EditorGUILayout.PropertyField(healthProperty, new GUIContent("Minion Health"));
					
					GUILayout.Space(10);

					SerializedProperty minionAttributes = serializedObject.FindProperty("MinionAttributes");
					EditorGUILayout.PropertyField(minionAttributes);
					break;
				case CardType.Spell:
					EditorGUILayout.PropertyField(attackProperty, new GUIContent("Spell Damage"));
					break;
				case CardType.Weapon:
					EditorGUILayout.PropertyField(attackProperty, new GUIContent("Weapon Damage"));
					EditorGUILayout.PropertyField(healthProperty, new GUIContent("Durability"));
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			
			serializedObject.ApplyModifiedProperties();			
		}

		private void OnEnable()
		{
			cardInfo = (CardInfo) target;
		}
	}
}