using UnityEngine;

public class Lisa_NPC : MonoBehaviour
{
    public Transform[] kontrolleure; // Eine Liste der Transforme für die Trigger
    public float distanceThreshold = 1f; // Der Abstandsschwellenwert
    public GameObject attachPoint; // Das GameObject des Attach Points
    public float yOffset = 0.03f; // Der Betrag für die y-Position
    public float zOffset = 0.04f; // Der Betrag für die z-Position
    private AudioSource audioSource;
    private Animator animator;
    private bool triggerPlayed = false;
    private Vector3 originalAttachPointPosition; // Die ursprüngliche Position des Attach Points
    private bool isMoving = false; // Gibt an, ob die Position gerade geändert wird
    private float moveSpeed = 0.5f; // Die Geschwindigkeit der schrittweisen Änderung
    private float pauseDuration = 0.5f; // Die Dauer des Verweilens in der neuen Position

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (attachPoint != null)
        {
            originalAttachPointPosition = attachPoint.transform.position;
        }
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

    private void ReturnToOriginalPosition()
    {
        StartCoroutine(MoveBackToOriginalPosition());
    }

    private void TriggerAction()
    {
        
        audioSource.Play();
    }

    private System.Collections.IEnumerator MoveAttachPoint()
    {
        if (!triggerPlayed)
        {
            animator.SetTrigger("Kontrolleur"); // Setze den Animator-Bool
            isMoving = true;
            Vector3 targetPosition = originalAttachPointPosition + new Vector3(0f, yOffset, zOffset);

            while (Vector3.Distance(attachPoint.transform.position, targetPosition) > 0.01f)
            {
                attachPoint.transform.position = Vector3.MoveTowards(attachPoint.transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            attachPoint.transform.position = targetPosition; // Stelle sicher, dass die Zielposition erreicht wird
            isMoving = false;

            TriggerAction(); // Starte die Aktion nachdem die neue Position erreicht wurde
            yield return new WaitForSeconds(pauseDuration); // Warte in der neuen Position
            ReturnToOriginalPosition(); // Beginne die Rückkehr zur ursprünglichen Position
            triggerPlayed = true;
        }
    }

        private System.Collections.IEnumerator MoveBackToOriginalPosition()
    {
        isMoving = true;

        while (Vector3.Distance(attachPoint.transform.position, originalAttachPointPosition) > 0.01f)
        {
            attachPoint.transform.position = Vector3.MoveTowards(attachPoint.transform.position, originalAttachPointPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        attachPoint.transform.position = originalAttachPointPosition; // Stelle sicher, dass die ursprüngliche Position erreicht wird
        isMoving = false;
    }
}
