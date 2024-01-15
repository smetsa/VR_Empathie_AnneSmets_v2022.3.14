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

    void Update()
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

        float headYaw = head.vrTarget.eulerAngles.y - headBodyYawOffset;
        float headPitch = head.vrTarget.eulerAngles.x;
        float headRoll = head.vrTarget.eulerAngles.z;

        // Oberkörperdrehung
        float upperBodyYaw = headYaw * 0.9f;

        // Rotation nur für den Oberkörper
        Quaternion upperBodyRotation = Quaternion.Euler(0.8f, upperBodyYaw, 0.9f);

        // Spine-Bones oben vom Pelvis anpassen
        Quaternion spineRotation = Quaternion.Euler(headPitch, 0.7f, headRoll);

        // Kombiniere die Rotationen
        Quaternion finalRotation = Quaternion.Slerp(spine3Bone.rotation, spineRotation * upperBodyRotation, turnSmoothness);

        // Wende die Rotation auf den Oberkörper an
        spine3Bone.rotation = finalRotation;

    }
}