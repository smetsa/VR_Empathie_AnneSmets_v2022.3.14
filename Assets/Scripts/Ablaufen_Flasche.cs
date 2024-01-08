using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ablaufen_Flasche : MonoBehaviour
{
    public Transform[] waypoints;
    public float movementSpeed = 1.0f;
    public float rotationSpeed = 2.0f;
    public float delayAtWaypoint = 6.0f;
    public GameObject check;
    public GameObject player;
    public GameObject SocketInteractor;
    public bool alreadyTriggered = false;
    private int currentWaypointIndex = 0;
    private Transform targetWaypoint;
    private bool reachedDestination = false;
    public bool isDelaying = false;

    private Animator animator;
    public AudioSource audioSource;
    public GameObject separateAudioObject;
    public GameObject AudioDanke;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (waypoints.Length > 0)
        {
            targetWaypoint = waypoints[0];
        }
    }

    private void Update()
    {
        if (!reachedDestination)
        {
            MoveToWaypoint();
        }
        else if (currentWaypointIndex != waypoints.Length && !isDelaying)
        {
            StartCoroutine(DelayAtWaypoint());
        }
    }

  

    private void MoveToWaypoint()
    {
        if (targetWaypoint != null)
        {
            Vector3 targetDirection = targetWaypoint.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);

            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                reachedDestination = true;
                if (currentWaypointIndex < waypoints.Length - 1)
                {
                    currentWaypointIndex++;
                    targetWaypoint = waypoints[currentWaypointIndex];
                }
            }
        }
    }

    private IEnumerator DelayAtWaypoint()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        while (distanceToPlayer < 1f && !alreadyTriggered)
        {
            animator.SetTrigger("Warten_Flasche");
            audioSource.Pause();
            isDelaying = true;
            SocketInteractor.SetActive(true);

            separateAudioObject.SetActive(true);

            if (check.activeSelf)
            {
                //animator.SetTrigger("Weitergehen");
                AudioDanke.SetActive(true);
                animator.SetTrigger("Weitergehen");
                audioSource.Play();
                alreadyTriggered = true; // Markiere den Trigger als bereits aktiviert
                isDelaying = false;
                GoToNextWaypoint();
                yield break; // Beende die Coroutine, da der Check aktiv wurde
            }

            yield return null; // Warte auf den nächsten Frame, bevor die Überprüfung erneut durchgeführt wird

        }

        Debug.Log("Waiting at waypoint " + currentWaypointIndex);
        animator.SetTrigger("Warten_Flasche");
        audioSource.Pause();
        isDelaying = true;
        yield return new WaitForSeconds(delayAtWaypoint);
        isDelaying = false;
        GoToNextWaypoint();
    }

    private void GoToNextWaypoint()
    {
        reachedDestination = false;
        animator.SetTrigger("Weitergehen");
        audioSource.Play();

        //yield return new WaitForSeconds(delayAtWaypoint);

        if (currentWaypointIndex < waypoints.Length)
        {
            targetWaypoint = waypoints[currentWaypointIndex];
        }
        else
        {
            Debug.Log("NPC reached the end of waypoints.");
            // Weitere Aktionen, wenn der NPC das Ziel erreicht hat
        }
    }
}