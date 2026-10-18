using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AvatarUploadPanel : MonoBehaviour
{
    public InputField fileNameInput;
    public Button browseButton; // if implementing native file picker
    public Button uploadButton;
    public RawImage previewImage;
    public string userId;

    private byte[] imageData;

    void Start()
    {
        uploadButton.onClick.AddListener(OnUploadClicked);
    }

    void OnUploadClicked()
    {
        if (string.IsNullOrEmpty(fileNameInput.text)) return;
        if (imageData == null) {
            Debug.Log("No image selected (placeholder). In editor, drag a texture to previewImage).");
            StartCoroutine(UploadPlaceholder());
            return;
        }
        StartCoroutine(UploadImage(fileNameInput.text));
    }

    IEnumerator UploadPlaceholder()
    {
        // In editor/demo we simulate an upload by calling request-upload then confirm
        var json = JsonUtility.ToJson(new { userId = userId, fileName = "avatar_demo.png" });
        yield return ApiClient.Post<UploadUrlResponse>("/api/avatar/request-upload", json, (resp) => {
            if (resp != null && resp.success) {
                // In production, you'd PUT the bytes to resp.uploadUrl. Here we immediately confirm
                StartCoroutine(ConfirmAvatar(resp.accessUrl));
            }
        }, (err) => Debug.LogError(err));
    }

    IEnumerator UploadImage(string fileName)
    {
        // Upload flow for real devices: get signed URL then PUT bytes to storage, then confirm
        var json = JsonUtility.ToJson(new { userId = userId, fileName = fileName });
        yield return ApiClient.Post<UploadUrlResponse>("/api/avatar/request-upload", json, (resp) => {
            if (resp != null && resp.success) {
                // TODO: perform PUT to resp.uploadUrl with imageData
                StartCoroutine(ConfirmAvatar(resp.accessUrl));
            }
        }, (err) => Debug.LogError(err));
    }

    IEnumerator ConfirmAvatar(string accessUrl)
    {
        var json = JsonUtility.ToJson(new { userId = userId, accessUrl = accessUrl });
        yield return ApiClient.Post<RedeemResponse>("/api/avatar/confirm", json, (resp) => {
            if (resp != null && resp.success) {
                Debug.Log("Avatar updated");
            }
        }, (err) => Debug.LogError(err));
    }
}

[System.Serializable]
public class UploadUrlResponse { public bool success; public string uploadUrl; public string accessUrl; }
