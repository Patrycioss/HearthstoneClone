using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Utility class to make it easier to swap to different scenes.
/// </summary>
public class SceneSwapper
{
	private static Dictionary<Scene, string> sceneToName = new Dictionary<Scene, string>()
	{
		{Scene.MainMenu, "MainMenu"},
		{Scene.Game, "Game"},
		{Scene.DeckEditor, "DeckEditor"},
		{Scene.DeckView, "DeckView"}
	};

	/// <summary>
	/// Enum that helps prevent issues with typos in scene name strings.
	/// </summary>
	public enum Scene
	{
		MainMenu,
		Game,
		DeckEditor,
		DeckView
	}

	/// <summary>
	/// Previous active scene.
	/// </summary>
	public Scene PreviousScene { get; private set; }

	/// <summary>
	/// Currently active scene.
	/// </summary>
	public Scene ActiveScene { get; private set; }

	/// <summary>
	/// Initializes the scene swapper.
	/// </summary>
	public void Initialize()
	{
		string sceneName = SceneManager.GetActiveScene().name;

		foreach (var a in sceneToName.Where(a => a.Value.Equals(sceneName)))
		{
			ActiveScene = a.Key;
			return;
		}
	}
	
	/// <summary>
	/// Sets the scene to the specified scene name.
	/// </summary>
	/// <param name="scene"><see cref="Scene"/> to swap to.</param>
	public void SetScene(Scene scene, Action<AsyncOperation> onCompleteCallback = null)
	{
		PreviousScene = ActiveScene;
		
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToName[scene]);

		operation.completed += doneOperation =>
		{
			ActiveScene = scene;
			onCompleteCallback?.Invoke(doneOperation);
		};
	}

	/// <summary>
	/// Exits the application.
	/// </summary>
	public void ExitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;			
#else
			Application.Quit();
#endif
	}

	
}