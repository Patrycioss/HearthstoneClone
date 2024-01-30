
namespace CustomEditor
{
	using UnityEngine;
	using UnityEditor;

	/// <summary>
	/// Used to make serialized fields for scenes in the editor.
	/// Based on this answer from the Unity forums: https://discussions.unity.com/t/inspector-field-for-scene-asset/40763/5
	/// </summary>
	[System.Serializable]
	public class SceneField
	{
		/// <summary>
		/// Name of the scene.
		/// </summary>
		public string SceneName => sceneName;

		[SerializeField] private Object sceneAsset;
		[SerializeField] private string sceneName = "";

		/// <summary>
		/// Returns the scene selected in the field's name so it works with Unity's methods.
		/// </summary>
		/// <param name="sceneField">Editor field for a scene.</param>
		/// <returns>The name of the scene.</returns>
		public static implicit operator string(SceneField sceneField)
		{
			return sceneField.SceneName;
		}
	}

	/// <summary>
	/// Drawer to draw the property for <see cref="SceneField"/> in the editor.
	/// </summary>
	[CustomPropertyDrawer(typeof(SceneField))]
	public class SceneFieldPropertyDrawer : PropertyDrawer 
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, GUIContent.none, property);
			
			SerializedProperty sceneAsset = property.FindPropertyRelative("sceneAsset");
			SerializedProperty sceneName = property.FindPropertyRelative("sceneName");
			
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			
			if (sceneAsset != null)
			{
				sceneAsset.objectReferenceValue = EditorGUI.ObjectField(position, sceneAsset.objectReferenceValue, typeof(SceneAsset), false); 

				if( sceneAsset.objectReferenceValue != null )
				{
					sceneName.stringValue = (sceneAsset.objectReferenceValue as SceneAsset)?.name;
				}
			}
			
			EditorGUI.EndProperty( );
		}
	}
}