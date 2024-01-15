using CardManagement.CardComposition;
using CardManagement.Physical;
using Game;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;
using Utils;

public class TestButton : MonoBehaviour
{
    [SerializeField] private CardInfo testCard;
    [SerializeField] private  HorizontalLayoutGroup horizontalLayoutGroup;
    [SerializeField] private AssetLabelReference cardPrefabLabel;

    [SerializeField] private Player player;
    

    private Button button;
    
    // Start is called before the first frame update
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(async () =>
        {
            GameObject cardObject = await ResourceUtils.InstantiateFromLabel(cardPrefabLabel,
                new InstantiationParameters(horizontalLayoutGroup.transform, false));

            if (cardObject.TryGetComponent(out PhysicalCard physicalCard))
            {
                physicalCard.Initialize(new PhysicalCardConfiguration()
                {
                    CardInfo = testCard,
                    Player = player,
                });
                physicalCard.Flip();
            }});
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
