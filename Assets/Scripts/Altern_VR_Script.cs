using UnityEngine;

[System.Serializable]
public class VRMap2
{
    public Transform vrTarget;
    public Transform ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public Vector3 headToBodyOffset;
    public void Map()
    {
        ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class Altern_IKTargetFollowVRRig : MonoBehaviour
{
    [Range(0, 1)]
    public float turnSmoothness = 0.1f;
    public VRMap2 head;
    public VRMap2 leftHand;
    public VRMap2 rightHand;

    public Vector3 headBodyPositionOffset;
    public float headBodyYawOffset;

    // Update is called once per frame
    void LateUpdate()
    {
        head.Map();
        leftHand.Map();
        rightHand.Map();

        // Hier wird auf headToBodyOffset über die Instanz head von VRMap2 zugegriffen
        Vector3 bodyPosition = head.ikTarget.position - head.headToBodyOffset;
        transform.position = bodyPosition + headBodyPositionOffset;

        float yaw = head.vrTarget.eulerAngles.y + headBodyYawOffset;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, yaw, transform.eulerAngles.z), turnSmoothness);
    }
}
