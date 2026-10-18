using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class RedeemResponse
{
    public bool success;
    public UserData user;
}

[System.Serializable]
public class UserData
{
    public string id;
    public string username;
    public bool is_full_unlocked;
    public string trial_expires_at;
}

public class PromoRedeemPanel : MonoBehaviour
{
    public InputField codeInput;
    public Button redeemButton;
    public Text statusText;
    public string userId; // wire this from sign-in

    void Start()
    {
        redeemButton.onClick.AddListener(OnRedeemClicked);
    }

    void OnRedeemClicked()
    {
        string code = codeInput.text.Trim();
        if (string.IsNullOrEmpty(code))
        {
            statusText.text = "Enter a code.";
            return;
        }
        StartCoroutine(RedeemCoroutine(code));
    }

    IEnumerator RedeemCoroutine(string code)
    {
        var json = JsonUtility.ToJson(new { userId = userId, code = code });
        yield return ApiClient.Post<RedeemResponse>("/api/redeem", json, (resp) => {
            if (resp != null && resp.success)
            {
                statusText.text = "Code accepted. Account unlocked.";
            }
            else
            {
                statusText.text = "Invalid code or server error.";
            }
        }, (err) => {
            statusText.text = "Error: " + err;
        });
    }
}
