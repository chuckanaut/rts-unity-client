using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    public float panSpeed = 0.5f;
    public float zoomSpeed = 0.1f;
    public float minY = 10f;
    public float maxY = 100f;

    private Camera cam;
    private Vector3 lastPanPosition;
    private int panFingerId; // touch mode
    private bool wasZoomingLastFrame; // touch mode
    private Vector3[] lastZoomPositions; // touch mode

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
#if UNITY_EDITOR
        UpdateEditor();
#else
        UpdateMobile();
#endif
    }

    void UpdateEditor()
    {
        // Pan with middle mouse
        if (Input.GetMouseButtonDown(2))
        {
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(2))
        {
            PanCamera(Input.mousePosition);
        }

        // Zoom with scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, zoomSpeed * 10f);
    }

    void UpdateMobile()
    {
        if (Input.touchCount == 1)
        {
            wasZoomingLastFrame = false;
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                lastPanPosition = touch.position;
                panFingerId = touch.fingerId;
            }
            else if (touch.phase == TouchPhase.Moved && touch.fingerId == panFingerId)
            {
                PanCamera(touch.position);
            }
        }
        else if (Input.touchCount == 2)
        {
            Vector3[] newPositions = new Vector3[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
            if (!wasZoomingLastFrame)
            {
                wasZoomingLastFrame = true;
                lastZoomPositions = newPositions;
            }
            else
            {
                // Zoom based on distance between touches
                float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                float delta = newDistance - oldDistance;
                ZoomCamera(delta * 0.01f, zoomSpeed);
                lastZoomPositions = newPositions;
            }
        }
        else
        {
            wasZoomingLastFrame = false;
        }
    }

    void PanCamera(Vector3 newPanPosition)
    {
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * panSpeed * cam.orthographicSize, 0, offset.y * panSpeed * cam.orthographicSize);
        transform.Translate(move, Space.World);
        lastPanPosition = newPanPosition;
    }

    void ZoomCamera(float deltaMagnitude, float speed)
    {
        float size = cam.orthographic ? cam.orthographicSize : cam.transform.position.y;
        size -= deltaMagnitude * speed;
        size = Mathf.Clamp(size, minY, maxY);
        if (cam.orthographic)
            cam.orthographicSize = size;
        else
        {
            Vector3 pos = cam.transform.position;
            pos.y = size;
            cam.transform.position = pos;
        }
    }
}
