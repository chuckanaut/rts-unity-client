using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string userId;
    public string currentMode = "1v1";
    public string currentMap = "Desert Outpost";

    public int resources = 1000;
    public int power = 100;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetUserId(string id)
    {
        userId = id;
    }

    public void AddResources(int amount)
    {
        resources += amount;
    }

    public void SpendResources(int amount)
    {
        resources = Mathf.Max(0, resources - amount);
    }
}
