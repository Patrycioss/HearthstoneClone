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
		
		private void Start()
		{
			turnLength = GameManager.Instance.TurnLength;
		}

		public void Begin()
		{
			background.color = Color.green;

			OnStartCallback?.Invoke();
			StartCoroutine(TurnCountDown());
		}

		public void End()
		{
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