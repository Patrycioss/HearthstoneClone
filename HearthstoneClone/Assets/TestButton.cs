using CardManagement.CardComposition;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    [SerializeField] private CardInfo testCard;
    [SerializeField] private PlayerHand hand;
    
    
    
    private Button button;
    
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            hand.AddCard(testCard);
        });
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
