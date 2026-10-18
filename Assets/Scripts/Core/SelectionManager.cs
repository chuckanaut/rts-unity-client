using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public List<GameObject> selectedObjects = new List<GameObject>();
    public Color highlightColor = Color.cyan;

    private Material lastMaterial;

    public void SelectObject(GameObject go)
    {
        ClearSelection();
        if (go == null) return;
        selectedObjects.Add(go);
        Highlight(go, true);
    }

    public void ClearSelection()
    {
        foreach (var o in selectedObjects)
        {
            Highlight(o, false);
        }
        selectedObjects.Clear();
    }

    void Highlight(GameObject go, bool on)
    {
        var rend = go.GetComponentInChildren<Renderer>();
        if (!rend) return;
        if (on)
        {
            // store original
            // NOTE: simplistic approach: replace color if material supports it
            if (rend.material.HasProperty("_Color"))
            {
                rend.material.color = highlightColor;
            }
        }
        else
        {
            // In a production-ready approach we would restore the material; here we just tint back to white
            if (rend.material.HasProperty("_Color"))
            {
                rend.material.color = Color.white;
            }
        }
    }
}
