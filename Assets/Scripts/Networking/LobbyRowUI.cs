using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LobbyRowUI : MonoBehaviour
{
    public Text idText;
    public Text infoText;
    public Button joinButton;
    private Lobby lobby;
    private string userId;

    public void Setup(Lobby l, string userId)
    {
        this.lobby = l;
        this.userId = userId;
        idText.text = l.id;
        infoText.text = $"Mode: {l.mode} Map: {l.mapId} Players: {l.players.Length}";
        joinButton.onClick.AddListener(OnJoinClicked);
    }

    void OnJoinClicked()
    {
        StartCoroutine(JoinCoroutine());
    }

    IEnumerator JoinCoroutine()
    {
        var json = JsonUtility.ToJson(new { lobbyId = lobby.id, userId = userId });
        yield return ApiClient.Post<JoinLobbyResponse>("/api/lobby/join", json, (resp) => {
            if (resp != null && resp.success) {
                Debug.Log("Joined lobby: " + resp.lobby.id);
            }
        }, (err) => Debug.LogError(err));
    }
}

[System.Serializable]
public class JoinLobbyResponse { public bool success; public Lobby lobby; }
