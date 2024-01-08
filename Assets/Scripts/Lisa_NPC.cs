using UnityEngine;

public class Lisa_NPC : MonoBehaviour
{
    public Transform[] kontrolleure; // Eine Liste der Transforme für die Trigger
    public float distanceThreshold = 1f; // Der Abstandsschwellenwert
    private AudioSource audioSource;
    private Animator animator;
    private bool triggerPlayed = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (kontrolleure != null)
        {
            for (int i = 0; i < kontrolleure.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, kontrolleure[i].position);

                if (distance <= distanceThreshold && !triggerPlayed)
                {
                    Invoke("TriggerAction", 2f); // Verzögere die Aktion um 2 Sekunden
                    triggerPlayed = true;
                }
            }
        }
    }

    private void TriggerAction()
    {
        animator.SetTrigger("Kontrolleur"); // Setze den Animator-Bool
        audioSource.Play();
    }
    }