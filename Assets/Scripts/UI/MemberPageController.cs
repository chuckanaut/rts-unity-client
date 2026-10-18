using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MemberPageController : MonoBehaviour
{
    public Text usernameText;
    public Text unlockStatusText;
    public Button startTrialButton;
    public string userId;

    void Start()
    {
        startTrialButton.onClick.AddListener(OnStartTrialClicked);
        RefreshProfile();
    }

    public void RefreshProfile()
    {
        StartCoroutine(ApiClient.Get<ProfileResponse>($"/api/profile/{userId}", OnProfileSuccess, (err) => Debug.LogError(err)));
    }

    void OnProfileSuccess(ProfileResponse resp)
    {
        if (resp == null || !resp.success) return;
        var u = resp.user;
        usernameText.text = u.username;
        bool trialActive = u.trial_active;
        if (u.is_full_unlocked) unlockStatusText.text = "Fully Unlocked";
        else if (trialActive) unlockStatusText.text = $"Trial active until {u.trial_expires_at}";
        else unlockStatusText.text = "Locked";
    }

    void OnStartTrialClicked()
    {
        StartCoroutine(StartTrialCoroutine());
    }

    IEnumerator StartTrialCoroutine()
    {
        var json = JsonUtility.ToJson(new { userId = userId });
        yield return ApiClient.Post<TrialResponse>("/api/trial", json, (resp) => {
            if (resp != null && resp.success) {
                RefreshProfile();
            }
        }, (err) => Debug.LogError(err));
    }
}

[System.Serializable]
public class ProfileResponse { public bool success; public ProfileUser user; }
[System.Serializable]
public class ProfileUser { public string id; public string username; public bool is_full_unlocked; public bool trial_active; public string trial_expires_at; }
[System.Serializable]
public class TrialResponse { public bool success; public string trial_expires_at; }
