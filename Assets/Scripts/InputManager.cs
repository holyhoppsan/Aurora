using UnityEngine;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
using com.rfilkov.kinect;
#endif

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Transform _leftCursor;

    [SerializeField]
    private Transform _rightCursor;

    [SerializeField]
    private bool _mouseInputEnabled = false;

    [SerializeField]
    private float _configurableFarPlane = 1.0f;

    [SerializeField]
    private float _configurableNearPlane;

    public Vector2 LeftCursorScreenPosition
    {
        get
        {
            return Camera.main.WorldToScreenPoint(_leftCursor.transform.position);
        }
    }

    public Vector2 RightCursorScreenPosition
    {
        get
        {
            return Camera.main.WorldToScreenPoint(_rightCursor.transform.position);
        }
    }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    [Tooltip("Scene object that will be used to represent the sensor's position and rotation in the scene.")]
    public Transform sensorTransform;

    [Tooltip("Body scale factors in X,Y,Z directions.")]
    public Vector3 scaleFactors = Vector3.one;

    [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
    public int playerIndex = 0;
#endif

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_mouseInputEnabled)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = _configurableFarPlane; // Distance from the camera
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

            if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
            {
                Cursor.visible = false;
                _leftCursor.transform.position = worldPosition;
            }
            else if (Input.GetMouseButton(1) && !Input.GetMouseButton(0))
            {
                Cursor.visible = false;
                _rightCursor.transform.position = worldPosition;
            }
            else
            {
                Cursor.visible = true;
            }
        }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        ProcessKinectInput();
#endif
    }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    private void ProcessKinectInput()
    {
        KinectManager kinectManager = KinectManager.Instance;

        if (kinectManager && kinectManager.IsInitialized())
        {
            if (kinectManager.IsUserDetected(playerIndex))
            {

                ulong userId = kinectManager.GetUserIdByIndex(playerIndex);
                Vector3 clavicleLeftJoint = GetPointPos(kinectManager, userId, KinectInterop.JointType.ClavicleLeft);
                Vector3 clavicleRightJoint = GetPointPos(kinectManager, userId, KinectInterop.JointType.ClavicleRight);

                float leftClavicleLength = CalculateClavicleLength(kinectManager, userId, KinectInterop.JointType.ClavicleLeft);
                float rightClavicleLength = CalculateClavicleLength(kinectManager, userId, KinectInterop.JointType.ClavicleRight);
                float leftArmLength = CalculateArmLength(kinectManager, userId, KinectInterop.JointType.ShoulderLeft);
                float rightArmLength = CalculateArmLength(kinectManager, userId, KinectInterop.JointType.ShoulderRight);

                var leftShoulderMaxPos = clavicleLeftJoint + (new Vector3(1.0f, 0.0f, 0.0f) * (leftClavicleLength + leftArmLength));

                //Debug.DrawLine(clavicleLeftJoint, leftShoulderMaxPos, Color.red);

                var rightShoulderMaxPos = clavicleRightJoint + (new Vector3(-1.0f, 0.0f, 0.0f) * (rightClavicleLength + rightArmLength));

                //Debug.DrawLine(clavicleLeftJoint, rightShoulderMaxPos, Color.green);

                Vector3 shoulderLeftJoint = GetPointPos(kinectManager, userId, KinectInterop.JointType.ShoulderLeft);
                //Debug.DrawLine(shoulderLeftJoint, shoulderLeftJoint + new Vector3(0.0f, 1.0f, 0.0f) * leftArmLength, Color.black);
                //Debug.DrawLine(shoulderLeftJoint, shoulderLeftJoint + new Vector3(0.0f, 0.0f, 1.0f) * leftArmLength, Color.blue);

                var xHalf = (rightClavicleLength + rightArmLength + leftClavicleLength + leftArmLength) / 2.0f;
                var yHalf = (leftArmLength + rightArmLength) / 2.0f;
                var zHalf = (leftArmLength + rightArmLength) / 2.0f;

                var boxLocation = new Vector3(0, 0, 0);
                var minBox = new Vector3(-xHalf, -yHalf, 0.0f);
                var maxBox = new Vector3(xHalf, yHalf, zHalf);

                // DebugRendering.DrawBoundingBox(minBox, maxBox, Color.yellow);

                _rightCursor.position = MapPointToWorld(GetPointPos(kinectManager, userId, KinectInterop.JointType.HandRight), clavicleRightJoint, minBox, maxBox);
                _leftCursor.position = MapPointToWorld(GetPointPos(kinectManager, userId, KinectInterop.JointType.HandLeft), clavicleLeftJoint, minBox, maxBox);
            }
        }
    }

    private Vector3 GetPointPos(KinectManager kinectManager, ulong userId, KinectInterop.JointType joint)
    {
        Vector3 jointPos = !sensorTransform ? kinectManager.GetJointPosition(userId, joint) : kinectManager.GetJointKinectPosition(userId, joint, true);
        var scaledPos = new Vector3(jointPos.x * scaleFactors.x, jointPos.y * scaleFactors.y, jointPos.z * scaleFactors.z);

        if (sensorTransform)
        {
            scaledPos = sensorTransform.transform.TransformPoint(scaledPos);
        }

        return scaledPos;
    }

    private float CalculateClavicleLength(KinectManager kinectManager, ulong userId, KinectInterop.JointType clavicleJointIndex)
    {
        var clavicleJoint = GetPointPos(kinectManager, userId, clavicleJointIndex);
        var shoulderJoint = GetPointPos(kinectManager, userId, clavicleJointIndex + 1);

        return Vector3.Distance(clavicleJoint, shoulderJoint);
    }

    private float CalculateArmLength(KinectManager kinectManager, ulong userId, KinectInterop.JointType shoulderJoingIndex)
    {
        var shoulderJoint = GetPointPos(kinectManager, userId, shoulderJoingIndex);
        var elbowJoint = GetPointPos(kinectManager, userId, shoulderJoingIndex + 1);
        var wristJoint = GetPointPos(kinectManager, userId, shoulderJoingIndex + 2);
        var handJoint = GetPointPos(kinectManager, userId, shoulderJoingIndex + 3);

        var totalDistance = Vector3.Distance(shoulderJoint, elbowJoint)
                            + Vector3.Distance(elbowJoint, wristJoint)
                            + Vector3.Distance(wristJoint, handJoint);

        return totalDistance;
    }

#endif

    public Vector3 GetNormalizedCoodinatesUsingBoundingBox(Vector3 point, Vector3 boundingBoxOrigin, Vector3 minBox, Vector3 maxBox)
    {
        var relativeHandPosition = point - boundingBoxOrigin;

        relativeHandPosition.z = -relativeHandPosition.z;

        var normalizedPoint = new Vector3(
                (relativeHandPosition.x - minBox.x) / (maxBox.x - minBox.x),
                (relativeHandPosition.y - minBox.y) / (maxBox.y - minBox.y),
                relativeHandPosition.z / (maxBox.z - minBox.z));

        normalizedPoint.x = Mathf.Clamp(normalizedPoint.x, 0.0f, 1.0f);
        normalizedPoint.y = Mathf.Clamp(normalizedPoint.y, 0.0f, 1.0f);
        normalizedPoint.z = Mathf.Clamp(normalizedPoint.z, 0.0f, 1.0f);

        return normalizedPoint;
    }

    public Vector3 MapPointToWorld(Vector3 point, Vector3 boundingBoxOrigin, Vector3 minBox, Vector3 maxBox)
    {
        var normalizedPoint = GetNormalizedCoodinatesUsingBoundingBox(point, boundingBoxOrigin, minBox, maxBox);

        float depthRange = _configurableFarPlane - _configurableNearPlane;
        normalizedPoint.z = _configurableNearPlane + normalizedPoint.z * depthRange;

        // Convert viewport coordinates to world space
        Vector3 worldPoint = Camera.main.ViewportToWorldPoint(normalizedPoint);

        return worldPoint;
    }
}
