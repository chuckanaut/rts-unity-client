using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopItem
{
    public string id;
    public string name;
    public string category;
    public string rarity;
    public float price;
}

[System.Serializable]
public class ShopResponse { public bool success; public ActiveShop activeShop; }
[System.Serializable]
public class ActiveShop { public string start; public string end; public ShopItem[] items; }

public class ShopPanel : MonoBehaviour
{
    public Transform itemContainer;
    public GameObject itemPrefab; // simple UI prefab with name, price, buy button

    void Start()
    {
        StartCoroutine(ApiClient.Get<ShopResponse>("/api/shop/active", OnShopLoaded, (err) => Debug.LogError(err)));
    }

    void OnShopLoaded(ShopResponse resp)
    {
        if (resp == null || !resp.success || resp.activeShop == null) return;
        foreach (Transform t in itemContainer) Destroy(t.gameObject);
        foreach (var it in resp.activeShop.items)
        {
            var go = Instantiate(itemPrefab, itemContainer);
            var ui = go.GetComponent<ShopItemUI>();
            if (ui != null) ui.Setup(it);
        }
    }
}
