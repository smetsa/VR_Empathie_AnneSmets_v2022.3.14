using UnityEngine;

public class DeactivateAfterDelay : MonoBehaviour
{
    public GameObject targetObject;
    public float delayInSeconds = 30f;

    private void Start()
    {
        Invoke("DeactivateTarget", delayInSeconds);
    }

    void DeactivateTarget()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }
}
