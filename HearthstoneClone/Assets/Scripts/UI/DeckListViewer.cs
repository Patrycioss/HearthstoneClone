using System.Collections.Generic;
using System.Linq;
using Deck;
using ErrorHandling;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class DeckListViewer : MonoBehaviour
	{
		[SerializeField] private GameObject deckCardPrefab;
		[SerializeField] private GridLayoutGroup contentTransform;

		private List<DeckInfo> decks = new();

		private async void Awake()
		{
			Debug.LogError($"{gameObject.name}");

			Result a = await GameManager.Instance.DeckManager.LoadAllDecks();
			Debug.Log(a.Message);

			decks = GameManager.Instance.DeckManager.GetAllDecks().ToList();

			foreach (DeckInfo deckInfo in decks)
			{
				GameObject cardObject = Instantiate(deckCardPrefab, contentTransform.transform);

				if (cardObject.TryGetComponent(out DeckCard deckCard))
				{
					deckCard.Instantiate(deckInfo);
				}
				else Debug.LogError($"Spawned object in {nameof(DeckListViewer)} doesn't have a {nameof(DeckCard)}!");
			}


			Vector2 cardSize = contentTransform.cellSize;
			Vector2 spacing = contentTransform.spacing;

			float height = decks.Count * cardSize.y + decks.Count * spacing.y;
			
			RectTransform rect = contentTransform.GetComponent<RectTransform>();
			rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
		}

		private void OnEnable()
		{
			

		}
	}
}