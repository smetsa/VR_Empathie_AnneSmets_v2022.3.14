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
    public Transform leftArmBone, rightArmBone;

    public Vector3 headBodyPositionOffset;
    public float headBodyYawOffset;
    public float minHeightFromGround = 1.0f;
    public float maxHeightFromGround = 1.99f;
    private bool armExtended = false;

    float maxArmLength = 0.5f; // Länge des charakter Unterarms
    float currentArmLength;


    void LateUpdate()
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
        // Quaternion spineRotation = Quaternion.Euler(headPitch, 0.1f, headRoll);

        // Kombiniere die Rotationen für den Kopf
        Quaternion finalHeadRotation = headRotation; //* Quaternion.Inverse(spineRotation);

        // Wende die Rotation auf den Kopf an
        head.ikTarget.rotation = finalHeadRotation;

        // Kopiere die Rotation der Spine für den Oberkörper
        //Quaternion finalUpperBodyRotation = spineRotation;

        // Spine-Bones oben vom Pelvis anpassen
        //spine3Bone.rotation = finalUpperBodyRotation;

        if (leftArmBone != null && leftHand != null)
        {
            currentArmLength = Vector3.Distance(leftHand.vrTarget.position, leftArmBone.position);

            if (currentArmLength > maxArmLength)
            {
                // Berechne das Verhältnis von maxArmLength zu currentArmLength
                float scaleRatio =  currentArmLength/maxArmLength;
                float requiredExtension = currentArmLength - maxArmLength;

                // Skaliere den Unterarm entsprechend
                Vector3 newScale = leftArmBone.localScale;
                newScale.y += requiredExtension; // Skaliere den Unterarm direkt um die benötigte Verlängerung
                leftArmBone.localScale = newScale;

                // Verschiebe die Hand entsprechend der Verlängerung des Arms
                Vector3 handPositionOffset = new Vector3(0f, requiredExtension, 0f);
                leftHand.ikTarget.position += handPositionOffset;
                maxArmLength = currentArmLength;
            }
            else
            {
                // Stellen Sie sicher, dass die Skalierung auf den Standardwert zurückgesetzt wird, wenn der Arm nicht verlängert wird
                leftArmBone.localScale = Vector3.one;
            }
        }

        // Setze die Handpositionen nach der Spine-Aktualisierung
        leftHand.ikTarget.position = leftHand.vrTarget.position;
        rightHand.ikTarget.position = rightHand.vrTarget.position;
    }
}