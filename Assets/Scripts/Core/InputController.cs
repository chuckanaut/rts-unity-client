using UnityEngine;

public class InputController : MonoBehaviour
{
    public SelectionManager selectionManager;

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                selectionManager.SelectObject(hit.collider.gameObject);
            }
        }
#else
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    selectionManager.SelectObject(hit.collider.gameObject);
                }
            }
        }
#endif
    }
}
