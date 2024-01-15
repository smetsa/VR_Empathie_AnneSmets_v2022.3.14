using UnityEngine;

public class Kopfhörer : MonoBehaviour
{
    public Transform kopfTransform;
    private Vector3 offsetPosition; // Ausgangsposition Kopfhörer relativ zum Kopf
    private Quaternion offsetRotation; // Ausgangsrotation relativ zum Kopf

    void Awake()
    {
        // Ausgangsposition und -rotation relativ zum Kopf
        offsetPosition = kopfTransform.InverseTransformPoint(transform.position);
        offsetRotation = Quaternion.Inverse(kopfTransform.rotation) * transform.rotation;
    }

    void Update()
    {
        // Aktualisiere die Position und Rotation
        transform.position = kopfTransform.TransformPoint(offsetPosition);
        transform.rotation = kopfTransform.rotation * offsetRotation;
    }
}
