using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	public class Turn : MonoBehaviour
	{
		/// <summary>
		/// Called when the turn ends.
		/// </summary>
		public event Action OnEndCallback;
		
		/// <summary>
		/// Called when the turn starts.
		/// </summary>
		public event Action OnStartCallback;
		
		[SerializeField] private Image background;
		[SerializeField] private TMP_Text countdownText;

		private int turnLength = 10;
		private int currentTurnLength;
		private Coroutine activeCoroutine;
		
		private void Start()
		{
			turnLength = GameManager.Instance.TurnLength;
		}

		public void Begin()
		{
			background.color = Color.green;

			OnStartCallback?.Invoke();
			activeCoroutine = StartCoroutine(TurnCountDown());
		}

		public void End()
		{
			if (activeCoroutine != null)
			{
				StopCoroutine(activeCoroutine);
			}
			
			currentTurnLength = turnLength;

			background.color = Color.red;
			OnEndCallback?.Invoke();
		}

		private IEnumerator TurnCountDown()
		{
			currentTurnLength = turnLength;

			while (currentTurnLength > 0)
			{
				countdownText.text = currentTurnLength.ToString();
				yield return new WaitForSecondsRealtime(1);
				currentTurnLength--;
			}
			
			countdownText.text = currentTurnLength.ToString();
			End();
		}
	}
}