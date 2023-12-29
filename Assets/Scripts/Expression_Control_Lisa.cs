using UnityEngine;

public class Expression_Control_Lisa : MonoBehaviour
{
    public GameObject triggerObject; // Das GameObject, das als Trigger dient, bei Lisa das Smartphone
    private Animator anim;
    private bool inSpecialExpression = false;
    private string currentExpression = "Default";
    private float extraSadDuration = 20f; // Dauer des "Extra Sad"-Ausdrucks in Sekunden

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (triggerObject.activeSelf && !inSpecialExpression)
        {
            inSpecialExpression = true;
            SetExpression("Extra Sad");
            Invoke("SetDefaultExpression", extraSadDuration);
        }
    }

    private void SetDefaultExpression()
    {
        inSpecialExpression = false;
        SetExpression("Default");
    }

    private void SetExpression(string expressionName)
    {
        Debug.Log("Animation " + expressionName);
        anim.Play("Facial Expression_" + expressionName);
        currentExpression = expressionName;
    }
}