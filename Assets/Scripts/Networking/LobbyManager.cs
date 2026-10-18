using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public InputField modeInput; // e.g., "1v1"
    public Dropdown mapDropdown;
    public Button createButton;
    public Button searchButton;
    public Transform resultsContainer;
    public GameObject lobbyRowPrefab;
    public string userId;

    void Start()
    {
        createButton.onClick.AddListener(OnCreateClicked);
        searchButton.onClick.AddListener(OnSearchClicked);
    }

    void OnCreateClicked()
    {
        StartCoroutine(CreateLobby());
    }

    IEnumerator CreateLobby()
    {
        var json = JsonUtility.ToJson(new { hostUserId = userId, mode = modeInput.text, mapId = mapDropdown.options[mapDropdown.value].text });
        yield return ApiClient.Post<CreateLobbyResponse>("/api/lobby/create", json, (resp) => {
            if (resp != null && resp.success) {
                Debug.Log("Lobby created: " + resp.lobby.id);
            }
        }, (err) => Debug.LogError(err));
    }

    void OnSearchClicked()
    {
        StartCoroutine(SearchLobbies());
    }

    IEnumerator SearchLobbies()
    {
        string mode = modeInput.text;
        string path = $"/api/lobby/search?mode={mode}";
        yield return ApiClient.Get<SearchLobbiesResponse>(path, (resp) => {
            if (resp != null && resp.success) {
                foreach (Transform t in resultsContainer) Destroy(t.gameObject);
                foreach (var l in resp.matches) {
                    var go = Instantiate(lobbyRowPrefab, resultsContainer);
                    var ui = go.GetComponent<LobbyRowUI>();
                    ui.Setup(l, userId);
                }
            }
        }, (err) => Debug.LogError(err));
    }
}

[System.Serializable]
public class CreateLobbyResponse { public bool success; public Lobby lobby; }
[System.Serializable]
public class Lobby { public string id; public string hostUserId; public string mode; public string mapId; public string[] players; }
[System.Serializable]
public class SearchLobbiesResponse { public bool success; public Lobby[] matches; }
