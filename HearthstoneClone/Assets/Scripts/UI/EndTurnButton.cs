using Game;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EndTurnButton : MonoBehaviour
    {
        [SerializeField] private PlayManager playManager;

        [SerializeField] private Button button;
    
        private void Start()
        {
            button.onClick.AddListener(() =>
            {
                playManager.EndActiveTurn();
            });
        }

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }
    }
}
