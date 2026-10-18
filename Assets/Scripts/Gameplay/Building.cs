using UnityEngine;

public class Building : MonoBehaviour
{
    public string buildingId;
    public int buildCost = 200;
    public int health = 250;
    public bool isProductionBuilding;

    public void Build()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SpendResources(buildCost);
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
