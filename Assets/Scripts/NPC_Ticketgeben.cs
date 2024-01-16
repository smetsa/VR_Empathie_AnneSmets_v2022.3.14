using UnityEngine;

public class NPC_Ticketgeben : MonoBehaviour
{
    public Transform[] kontrolleure; // Eine Liste der Transforme für die Trigger
    public float distanceThreshold = 1f; // Der Abstandsschwellenwert
    public float yOffset = 0.03f; // Der Betrag für die y-Position
    public float zOffset = 0.04f; // Der Betrag für die z-Position
    private Animator animator;
    private bool triggerPlayed = false;
    private bool isMoving = false; // Gibt an, ob die Position gerade geändert wird
    private float moveSpeed = 0.5f; // Die Geschwindigkeit der schrittweisen Änderung
    private float pauseDuration = 1f; // Die Dauer des Verweilens in der neuen Position

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isMoving && kontrolleure != null)
        {
            for (int i = 0; i < kontrolleure.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, kontrolleure[i].position);

                if (distance <= distanceThreshold && !triggerPlayed)
                {

                    StartCoroutine(MoveAttachPoint());
                    Invoke("ReturnToOriginalPosition", pauseDuration + 2f); // Verzögere die Rückkehr um die Verweilzeit plus 2 Sekunden
                    break; // Beende die Schleife, nachdem der Trigger gefunden wurde
                }
            }
        }
    }


    private System.Collections.IEnumerator MoveAttachPoint()
    {
        if (!triggerPlayed)
        {
            animator.SetTrigger("Kontrolleur"); // Setze den Animator-Bool
            yield return new WaitForSeconds(pauseDuration); // Warte in der neuen Position
            triggerPlayed = true;
        }
    }
}