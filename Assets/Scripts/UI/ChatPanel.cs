using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChatPanel : MonoBehaviour
{
    public InputField messageInput;
    public Button sendButton;
    public Transform messagesContainer;
    public GameObject messagePrefab;
    public string channel = "world";
    public string userId;

    void Start()
    {
        sendButton.onClick.AddListener(OnSendClicked);
        StartCoroutine(LoadHistory());
    }

    void OnSendClicked()
    {
        var msg = messageInput.text.Trim();
        if (string.IsNullOrEmpty(msg)) return;
        StartCoroutine(SendMessage(msg));
    }

    IEnumerator SendMessage(string msg)
    {
        var json = JsonUtility.ToJson(new { channel = channel, userId = userId, message = msg });
        yield return ApiClient.Post<SendChatResponse>("/api/chat/send", json, (resp) => {
            if (resp != null && resp.success) {
                AddMessage(resp.message.username, resp.message.message);
                messageInput.text = string.Empty;
            }
        }, (err) => Debug.LogError(err));
    }

    IEnumerator LoadHistory()
    {
        yield return ApiClient.Get<ChatHistoryResponse>($"/api/chat/history/{channel}", (resp) => {
            if (resp != null && resp.success) {
                foreach (var m in resp.messages) AddMessage(m.username, m.message);
            }
        }, (err) => Debug.LogError(err));
    }

    void AddMessage(string user, string text)
    {
        var go = Instantiate(messagePrefab, messagesContainer);
        var t = go.GetComponentInChildren<Text>();
        t.text = $"{user}: {text}";
    }
}

[System.Serializable]
public class SendChatResponse { public bool success; public ChatMessage message; }
[System.Serializable]
public class ChatMessage { public string id; public string channel; public string userId; public string username; public string message; public string timestamp; }
[System.Serializable]
public class ChatHistoryResponse { public bool success; public ChatMessage[] messages; }
