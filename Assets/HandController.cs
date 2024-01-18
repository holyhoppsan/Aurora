using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.rfilkov.kinect;
using System.Net.WebSockets;

public class HandController : MonoBehaviour
{
    [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
    public int playerIndex = 0;

    [SerializeField]
    private Transform _leftTransform;

    [SerializeField]
    private Transform _rightTransform;

    private Vector3 _leftHandPos = Vector3.zero;

    private Vector3 _rightHandPos = Vector3.zero;

    private Vector3 _leftIboxLeftBotBack = Vector3.zero;
    private Vector3 _leftIboxRightTopFront = Vector3.zero;
    private bool _isleftIboxValid = false;

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
                _isleftIboxValid = kinectManager.GetLeftHandInteractionBox(playerUserID, ref _leftIboxLeftBotBack, ref _leftIboxRightTopFront, _isleftIboxValid);

                // Debug.DrawLine(_leftIboxLeftBotBack, new Vector3(_leftIboxLeftBotBack.x, _leftIboxRightTopFront.y, _leftIboxLeftBotBack.z), Color.red); // 1
                // Debug.DrawLine(_leftIboxLeftBotBack, new Vector3(_leftIboxLeftBotBack.x, _leftIboxLeftBotBack.y, _leftIboxRightTopFront.z), Color.red); // 2
                // Debug.DrawLine(_leftIboxLeftBotBack, new Vector3(_leftIboxRightTopFront.x, _leftIboxLeftBotBack.y, _leftIboxLeftBotBack.z), Color.red); // 3
                // Debug.DrawLine(new Vector3(_leftIboxLeftBotBack.x, _leftIboxLeftBotBack.y, _leftIboxRightTopFront.z), new Vector3(_leftIboxLeftBotBack.x, ) , Color.red); // 4

                _leftHandPos = kinectManager.GetJointPosition(playerUserID, (int)KinectInterop.JointType.HandLeft);
                _rightHandPos = kinectManager.GetJointPosition(playerUserID, (int)KinectInterop.JointType.HandRight);

                //leftHandPos = kinectManager.GetSensorJointKinectPosition(0, playerIndex, KinectInterop.JointType.HandLeft, true);
                Debug.Log(_leftHandPos);
                _leftTransform.position = _leftHandPos;
                _rightTransform.position = _rightHandPos;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_leftHandPos, 0.1f);
    }
}
