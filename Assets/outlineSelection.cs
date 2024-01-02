using System.Collections;
using UnityEngine;

public class OutlineSelection : MonoBehaviour
{
    public GameObject objectToHighlight; // Das GameObject, das hervorgehoben werden soll
    public GameObject triggerForHighlight;

    private Outline outline;

    void Start()
    {
        outline = objectToHighlight.GetComponent<Outline>();
        if (outline == null)
        {
            outline = objectToHighlight.AddComponent<Outline>();
            outline.OutlineColor = Color.magenta;
            outline.OutlineWidth = 10.0f;
        }
        outline.enabled = false; // Initial deaktiviert
    }

    void Update()
    {
        // Aktiviere das Hervorheben, wenn das andere GameObject aktiviert ist
        if (triggerForHighlight.activeSelf)
        {
            outline.enabled = true;
        }
        else
        {
            outline.enabled = false;
        }
    }
}
