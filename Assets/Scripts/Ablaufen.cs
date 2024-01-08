using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ablaufen : MonoBehaviour
{
    public Transform[] waypoints;
    public float movementSpeed = 2.0f;
    public float rotationSpeed = 5.0f;
    public float delayAtWaypoint = 10.0f;
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
            animator.SetTrigger("Warten_Kontrolle");
            audioSource.Pause();
            isDelaying = true;

            separateAudioObject.SetActive(true);
            SocketInteractor.SetActive(true);

            if (check.activeSelf)
            {
                animator.SetTrigger("Weitergehen");
                audioSource.Play();
                AudioDanke.SetActive(true);
                alreadyTriggered = true; // Markiere den Trigger als bereits aktiviert
                isDelaying = false;
                GoToNextWaypoint();
                yield break; // Beende die Coroutine, da der Check aktiv wurde
            }

            yield return null; // Warte auf den nächsten Frame, bevor die Überprüfung erneut durchgeführt wird

        }

        Debug.Log("Waiting at waypoint " + currentWaypointIndex);
        animator.SetTrigger("Warten_Kontrolle");
        separateAudioObject.SetActive(true);
        audioSource.Pause();
        isDelaying = true;
        yield return new WaitForSeconds(delayAtWaypoint);
        separateAudioObject.SetActive(false);
        isDelaying = false;

        GoToNextWaypoint();
    }

    private void GoToNextWaypoint()
    {
        reachedDestination = false;
        Debug.Log("weitergehn");
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