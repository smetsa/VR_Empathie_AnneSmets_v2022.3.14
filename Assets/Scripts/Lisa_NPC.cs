using UnityEngine;

public class Lisa_NPC : MonoBehaviour
{
    public Transform[] kontrolleure; // Eine Liste der Transforme für die Trigger
    public float distanceThreshold = 0.5f; // Der Abstandsschwellenwert
    private AudioSource audioSource; 

    private Animator animator;

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

                if (distance <= distanceThreshold)
                {
                    animator.SetBool("Kontrolleur", true); // Setze den Animator-Bool

                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                }
                else
                {
                    animator.SetBool("Kontrolleur", false);
                }
            }
        }
    }
}