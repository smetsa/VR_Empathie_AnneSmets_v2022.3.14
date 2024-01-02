using System.Collections;
using UnityEngine;

public class AblaufenUndSitzen : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform finalDestination;
    public float movementSpeed = 2.0f;
    public float rotationSpeed = 5.0f;
    public GameObject check;
    public AudioSource audioSource;
    public GameObject neueAudioSource;
    public GameObject neueAnimation;

    private int currentWaypointIndex = 0;
    private Transform targetWaypoint;
    private bool reachedDestination = false;
    private bool reachedFinalDestination = false;

    private Animator animator;

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
        else
        {
            if (currentWaypointIndex < waypoints.Length - 1)
            {
                GoToNextWaypoint();
            }
            else if (currentWaypointIndex == waypoints.Length - 1 && !reachedFinalDestination)
            {
                reachedFinalDestination = true;
                StartCoroutine(WaitAtLastWaypoint());
            }
        }
    }

    private void MoveToWaypoint()
    {
        if (targetWaypoint != null)
        {
            Vector3 targetDirection = targetWaypoint.position - transform.position;
            targetDirection.y = 0;
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                reachedDestination = true;
                currentWaypointIndex++;
            }
        }
    }

    private void GoToNextWaypoint()
    {
        reachedDestination = false;

        if (currentWaypointIndex < waypoints.Length)
        {
            targetWaypoint = waypoints[currentWaypointIndex];
        }
    }

    private IEnumerator WaitAtLastWaypoint()
    {
        bool hasChecked = false;

        if (finalDestination != null && neueAnimation != null)
        {
            audioSource.Pause();
            neueAudioSource.SetActive(true);
            animator.SetBool("warten", true);

            while (!hasChecked)
            {
                if (check.activeSelf)
                {
                    neueAnimation.SetActive(true);
                    neueAudioSource.SetActive(true);
                    gameObject.SetActive(false);
                    hasChecked = true;
                }
                yield return null;
            }
        }
    }

}
