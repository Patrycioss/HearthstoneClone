using System;
using CardManagement;
using CardManagement.CardComposition;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CustomEditor
{
	[UnityEditor.CustomEditor(typeof(CardInfo))]
	public class CardInfoEditor : Editor
	{
		private CardInfo cardInfo;
		
		public override void OnInspectorGUI()
		{
			GUILayout.Label("Base Information", EditorStyles.boldLabel);
			cardInfo.CardName = EditorGUILayout.TextField("Card Name", cardInfo.CardName);
			cardInfo.Cost = EditorGUILayout.IntField("Mana Cost", cardInfo.Cost);
			cardInfo.Type = (CardType) EditorGUILayout.EnumPopup("CardType", cardInfo.Type);

			GUILayout.Space(10);

			GUILayout.Label("Description", EditorStyles.boldLabel);
			cardInfo.Description = EditorGUILayout.TextArea(cardInfo.Description, GUILayout.MinHeight(100));

			GUILayout.Space(10);

			GUILayout.Label("Get Asset Path from Asset", EditorStyles.boldLabel);
			Object selectedObject = null;
			selectedObject = EditorGUILayout.ObjectField(selectedObject, typeof(Object), false);
			if (selectedObject != null) {
				string assetPath = AssetDatabase.GetAssetPath(selectedObject);
				cardInfo.ImagePath = assetPath;
			}
			
			GUILayout.Label(cardInfo.ImagePath);


			SerializedProperty behaviours = serializedObject.FindProperty("Behaviours");
			EditorGUILayout.PropertyField(behaviours);
			serializedObject.ApplyModifiedProperties();

			GUILayout.Space(10);
			GUILayout.Label("Base Information", EditorStyles.boldLabel);
			

			switch (cardInfo.Type)
			{
				case CardType.Minion:
					cardInfo.Attack = EditorGUILayout.IntField("Attack Damage", cardInfo.Attack);
					cardInfo.Health = EditorGUILayout.IntField("Health", cardInfo.Health);

					GUILayout.Space(10);

					SerializedProperty minionAttributes = serializedObject.FindProperty("MinionAttributes");
					EditorGUILayout.PropertyField(minionAttributes);
					serializedObject.ApplyModifiedProperties();
					// for (int i = 0; i < minionAttributes.arraySize; i++)
					// {
					// 	EditorGUILayout.PropertyField(minionAttributes.GetArrayElementAtIndex(i));
					// }
					break;
				case CardType.Spell:
					cardInfo.Attack = EditorGUILayout.IntField("Spell Damage", cardInfo.Attack);
					break;
				case CardType.Weapon:
					cardInfo.Attack = EditorGUILayout.IntField("Weapon Damage", cardInfo.Attack);
					cardInfo.Health = EditorGUILayout.IntField("Durability", cardInfo.Health);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void OnEnable()
		{
			cardInfo = (CardInfo) target;
		}
	}
}