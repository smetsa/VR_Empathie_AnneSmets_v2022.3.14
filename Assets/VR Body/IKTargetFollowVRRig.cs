using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public void Map()
    {
        ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class IKTargetFollowVRRig : MonoBehaviour
{
    [Range(0,1)]
    public float turnSmoothness = 0.1f;
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public Vector3 headBodyPositionOffset;
    public float headBodyYawOffset;
    public float minHeightFromGround = 1.0f; // Mindesthöhe des Kopfes vom Boden
    public float maxHeightFromGround = 1.4f; // Maximalhöhe des Kopfes vom Boden

    void Update()
    {
        // Begrenzung der Höhe des Kopfes zum Boden
        Vector3 headPos = head.ikTarget.position;
        RaycastHit hit;
        if (Physics.Raycast(headPos, Vector3.down, out hit, maxHeightFromGround))
        {
            float height = Mathf.Clamp(hit.distance, minHeightFromGround, maxHeightFromGround);
            headPos.y = height;
            head.ikTarget.position = headPos;
        }

        // Körperrotation separat von Kopfrotation behandeln
        transform.position = head.ikTarget.position + headBodyPositionOffset;
        float headYaw = head.vrTarget.eulerAngles.y - headBodyYawOffset;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, headYaw, 0f), turnSmoothness);

        // Kopf-, Hand- und Körper-IK-Ziele aktualisieren
        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}