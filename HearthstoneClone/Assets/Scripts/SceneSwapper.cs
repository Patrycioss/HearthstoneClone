using External;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapper : MonoBehaviour
{
	public static SceneSwapper Instance;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogWarning($"There can't be more than one {nameof(SceneSwapper)} in a scene!");
			Destroy(Instance);
		}

		Instance = this;
	}

	public void SetScene(SceneField sceneField)
	{
		SceneManager.LoadSceneAsync(sceneField);
	}
}