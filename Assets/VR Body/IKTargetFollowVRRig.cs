using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    private Vector3 lastIKPosition;
    private Quaternion lastIKRotation;

    public void Map(float blendFactor)
    {
        ikTarget.position = Vector3.Lerp(lastIKPosition, vrTarget.TransformPoint(trackingPositionOffset), blendFactor);
        ikTarget.rotation = Quaternion.Slerp(lastIKRotation, vrTarget.rotation * Quaternion.Euler(trackingRotationOffset), blendFactor);

        lastIKPosition = ikTarget.position;
        lastIKRotation = ikTarget.rotation;
    }
}

public class IKTargetFollowVRRig : MonoBehaviour
{
    [Range(0, 1)]
    public float turnSmoothness = 0.99f;

    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public Transform pelvisBone;
    public Transform spine1Bone;
    public Transform spine2Bone;
    public Transform spine3Bone;

    public Vector3 headBodyPositionOffset;
    public float headBodyYawOffset;
    public float minHeightFromGround = 1.0f;
    public float maxHeightFromGround = 1.99f;

    void FixedUpdate()
    {
        // Kopf-, Hand- und Körper-IK-Ziele aktualisieren
        head.Map(turnSmoothness);
        leftHand.Map(turnSmoothness);
        rightHand.Map(turnSmoothness);

        // Begrenzung der Höhe des Kopfes zum Boden
        Vector3 headPos = head.ikTarget.position;
        RaycastHit hit;
        if (Physics.Raycast(headPos, Vector3.down, out hit, maxHeightFromGround))
        {
            float height = Mathf.Clamp(hit.distance, minHeightFromGround, maxHeightFromGround);
            headPos.y = height;
            head.ikTarget.position = headPos;
        }

        // Kopfrotation
        float headYaw = head.vrTarget.eulerAngles.y - headBodyYawOffset;
        float headPitch = head.vrTarget.eulerAngles.x;
        float headRoll = head.vrTarget.eulerAngles.z;
        Quaternion headRotation = Quaternion.Euler(headPitch, headYaw, headRoll);

        // Spine-Bones oben vom Pelvis anpassen
        Quaternion spineRotation = Quaternion.Euler(headPitch, 0.1f, headRoll);

        // Kombiniere die Rotationen für den Kopf
        Quaternion finalHeadRotation = headRotation * Quaternion.Inverse(spineRotation);

        // Wende die Rotation auf den Kopf an
        head.ikTarget.rotation = finalHeadRotation;

        // Kopiere die Rotation der Spine für den Oberkörper
        Quaternion finalUpperBodyRotation = spineRotation;

        // Spine-Bones oben vom Pelvis anpassen
        spine3Bone.rotation = finalUpperBodyRotation;

        // Setze die Handpositionen nach der Spine-Aktualisierung
        leftHand.ikTarget.position = leftHand.vrTarget.position;
        rightHand.ikTarget.position = rightHand.vrTarget.position;
    }
}