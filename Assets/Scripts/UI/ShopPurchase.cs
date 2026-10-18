using UnityEngine;
using UnityEngine.Purchasing;

// Placeholder: in production implement Unity IAP / Play Billing integration
public class ShopPurchase : MonoBehaviour
{
    public void PurchaseItem(string itemId, string userId)
    {
        // Launch platform purchase flow and on success call server /api/shop/purchase with token
        Debug.Log($"Purchase requested: {itemId} for user {userId}");
    }
}
