using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.rfilkov.kinect;

public class HandController : MonoBehaviour
{
    [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
    public int playerIndex = 0;

    [SerializeField]
    private Transform _leftTransform;

    [SerializeField]
    private Transform _rightTransform;

    private Vector3 leftHandPos = Vector3.zero;

    private Vector3 rightHandPos = Vector3.zero;

    private ulong playerUserID = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        KinectManager kinectManager = KinectManager.Instance;

        // update Kinect interaction
        if (kinectManager && kinectManager.IsInitialized())
        {
            playerUserID = kinectManager.GetUserIdByIndex(playerIndex);

            if (playerUserID != 0)
            {
                leftHandPos = kinectManager.GetJointPosition(playerUserID, (int)KinectInterop.JointType.HandLeft);
                rightHandPos = kinectManager.GetJointPosition(playerUserID, (int)KinectInterop.JointType.HandRight);

                //leftHandPos = kinectManager.GetSensorJointKinectPosition(0, playerIndex, KinectInterop.JointType.HandLeft, true);
                Debug.Log(leftHandPos);
                _leftTransform.position = leftHandPos;
                _rightTransform.position = rightHandPos;
            }
        }
    }
}
