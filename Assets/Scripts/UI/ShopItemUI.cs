using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public Text nameText;
    public Text priceText;
    public Button buyButton;

    private ShopItem item;

    public void Setup(ShopItem i)
    {
        item = i;
        nameText.text = i.name;
        priceText.text = "$" + i.price.ToString("0.00");
        buyButton.onClick.AddListener(OnBuyClicked);
    }

    void OnBuyClicked()
    {
        // In production: call Play Billing flow. For now show placeholder
        Debug.Log($"Buy clicked for {item.id}");
    }
}
