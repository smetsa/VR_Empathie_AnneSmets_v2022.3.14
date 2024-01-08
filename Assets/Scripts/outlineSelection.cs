using UnityEngine;

public class OutlineSelection : MonoBehaviour
{
    public GameObject objectToHighlight; // Das GameObject, das hervorgehoben werden soll
    public GameObject triggerForHighlight;
    public GameObject check;

    private Outline outline;
    private bool wasTriggerActive = false;

    void Start()
    {
        outline = objectToHighlight.GetComponent<Outline>();
        if (outline == null)
        {
            outline = objectToHighlight.AddComponent<Outline>();
            outline.OutlineColor = Color.cyan;
            outline.OutlineWidth = 20.0f;
            outline.enabled = false; // Initial deaktiviert
        }
    }

    void Update()
    {
        bool isTriggerActive = triggerForHighlight.activeSelf;
        bool isCheckActive = check == null || check.activeSelf;


        if (isTriggerActive != wasTriggerActive)
        {
            outline.enabled = isTriggerActive;
            wasTriggerActive = isTriggerActive;
        }
        if (isCheckActive)
        {
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
    }

    void OnDisable()
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
}