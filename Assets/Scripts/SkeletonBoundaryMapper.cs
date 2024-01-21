using UnityEngine;
using com.rfilkov.kinect;

namespace Aurora
{
    public class SkeletonBoundaryMapper : MonoBehaviour
    {
        [SerializeField]
        public bool _debugSkeletonEnabled = false;

        #region DebugSkeletonMemberVariables 

        [Tooltip("Game object used to represent the body joints.")]
        public GameObject jointPrefab;

        [Tooltip("Line object used to represent the bones between joints.")]
        //public LineRenderer linePrefab;
        public GameObject linePrefab;

        private GameObject[] joints = null;
        private GameObject[] lines = null;
        #endregion

        [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
        public int playerIndex = 0;

        [Tooltip("Scene object that will be used to represent the sensor's position and rotation in the scene.")]
        public Transform sensorTransform;

        [Tooltip("Body scale factors in X,Y,Z directions.")]
        public Vector3 scaleFactors = Vector3.one;

        [SerializeField]
        private Transform _leftTransform;

        [SerializeField]
        private Transform _rightTransform;

        [SerializeField]
        private Transform _leftCameraWorldTransform;

        [SerializeField]
        private Transform _rightCameraWorldTransform;

        [SerializeField]
        private Camera _mainCamera;

        [SerializeField]
        private float _configurableFarPlane;

        [SerializeField]
        private float _configurableNearPlane;

        private void CalculateHandMovementBox()
        {
            KinectManager kinectManager = KinectManager.Instance;

            ulong userId = kinectManager.GetUserIdByIndex(playerIndex);
            Vector3 clavicleLeftJoint = GetPointPos(kinectManager, userId, KinectInterop.JointType.ClavicleLeft);
            Vector3 clavicleRightJoint = GetPointPos(kinectManager, userId, KinectInterop.JointType.ClavicleRight);

            float leftClavicleLength = CalculateClavicleLength(kinectManager, userId, KinectInterop.JointType.ClavicleLeft);
            float rightClavicleLength = CalculateClavicleLength(kinectManager, userId, KinectInterop.JointType.ClavicleRight);
            float leftArmLength = CalculateArmLength(kinectManager, userId, KinectInterop.JointType.ShoulderLeft);
            float rightArmLength = CalculateArmLength(kinectManager, userId, KinectInterop.JointType.ShoulderRight);

            var leftShoulderMaxPos = clavicleLeftJoint + (new Vector3(1.0f, 0.0f, 0.0f) * (leftClavicleLength + leftArmLength));

            Debug.DrawLine(clavicleLeftJoint, leftShoulderMaxPos, Color.red);

            var rightShoulderMaxPos = clavicleRightJoint + (new Vector3(-1.0f, 0.0f, 0.0f) * (rightClavicleLength + rightArmLength));

            Debug.DrawLine(clavicleLeftJoint, rightShoulderMaxPos, Color.green);

            Vector3 shoulderLeftJoint = GetPointPos(kinectManager, userId, KinectInterop.JointType.ShoulderLeft);
            Debug.DrawLine(shoulderLeftJoint, shoulderLeftJoint + new Vector3(0.0f, 1.0f, 0.0f) * leftArmLength, Color.black);
            Debug.DrawLine(shoulderLeftJoint, shoulderLeftJoint + new Vector3(0.0f, 0.0f, 1.0f) * leftArmLength, Color.blue);

            var xHalf = (rightClavicleLength + rightArmLength + leftClavicleLength + leftArmLength) / 2.0f;
            var yHalf = (leftArmLength + rightArmLength) / 2.0f;
            var zHalf = (leftArmLength + rightArmLength) / 2.0f;

            var boxLocation = new Vector3(0, 0, 0);
            var minBox = new Vector3(-xHalf, -yHalf, 0.0f);
            var maxBox = new Vector3(xHalf, yHalf, zHalf);

            DrawBoundingBox(minBox, maxBox, Color.yellow);

            // _rightTransform.position = boxLocation + (GetPointPos(kinectManager, userId, KinectInterop.JointType.HandRight) - clavicleRightJoint);
            // _leftTransform.position = boxLocation + (GetPointPos(kinectManager, userId, KinectInterop.JointType.HandLeft) - clavicleLeftJoint);

            _rightCameraWorldTransform.position = MapPointToCameraFrustum(GetPointPos(kinectManager, userId, KinectInterop.JointType.HandRight), clavicleRightJoint, minBox, maxBox);
            _leftCameraWorldTransform.position = MapPointToCameraFrustum(GetPointPos(kinectManager, userId, KinectInterop.JointType.HandLeft), clavicleLeftJoint, minBox, maxBox);
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

        private void DrawBoundingBox(Vector3 minCorner, Vector3 maxCorner, Color color)
        {
            Vector3[] corners = new Vector3[8];

            // Calculate the eight corners of the bounding box
            corners[0] = new Vector3(minCorner.x, minCorner.y, minCorner.z);
            corners[1] = new Vector3(minCorner.x, minCorner.y, maxCorner.z);
            corners[2] = new Vector3(maxCorner.x, minCorner.y, maxCorner.z);
            corners[3] = new Vector3(maxCorner.x, minCorner.y, minCorner.z);
            corners[4] = new Vector3(minCorner.x, maxCorner.y, minCorner.z);
            corners[5] = new Vector3(minCorner.x, maxCorner.y, maxCorner.z);
            corners[6] = new Vector3(maxCorner.x, maxCorner.y, maxCorner.z);
            corners[7] = new Vector3(maxCorner.x, maxCorner.y, minCorner.z);

            // Draw lines between the corners to create the bounding box
            for (int i = 0; i < 4; i++)
            {
                int nextIndex = (i + 1) % 4;
                Debug.DrawLine(corners[i], corners[nextIndex], color);
                Debug.DrawLine(corners[i + 4], corners[nextIndex + 4], color);
                Debug.DrawLine(corners[i], corners[i + 4], color);
            }

            // Connect the top and bottom corners
            for (int i = 0; i < 4; i++)
            {
                Debug.DrawLine(corners[i], corners[i + 4], color);
            }
        }

        public Vector3 MapPointToCameraFrustum(Vector3 point, Vector3 boundingBoxOrigin, Vector3 minBox, Vector3 maxBox)
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

            float depthRange = _configurableFarPlane - _configurableNearPlane;
            normalizedPoint.z = _configurableNearPlane + normalizedPoint.z * depthRange;

            // Convert viewport coordinates to world space
            Vector3 worldPoint = _mainCamera.ViewportToWorldPoint(normalizedPoint);

            return worldPoint;
        }

        void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 300, 50), $"x; {_rightCameraWorldTransform.position.x} y; {_rightCameraWorldTransform.position.y} z; {_rightCameraWorldTransform.position.z}");
        }

        void Start()
        {
            KinectManager kinectManager = KinectManager.Instance;
            if (_debugSkeletonEnabled)
            {
                #region StartDebugSkeleton
                if (kinectManager && kinectManager.IsInitialized())
                {
                    int jointsCount = kinectManager.GetJointCount();

                    if (jointPrefab)
                    {
                        joints = new GameObject[jointsCount];

                        for (int i = 0; i < joints.Length; i++)
                        {
                            joints[i] = Instantiate(jointPrefab) as GameObject;
                            joints[i].transform.parent = transform;
                            joints[i].name = ((KinectInterop.JointType)i).ToString();
                            joints[i].SetActive(false);
                        }
                    }

                    lines = new GameObject[jointsCount];
                }
                #endregion
            }
            else
            {
                DisableDebugSkeleton(kinectManager);
            }

        }

        void Update()
        {
            KinectManager kinectManager = KinectManager.Instance;

            if (kinectManager && kinectManager.IsInitialized())
            {
                if (_debugSkeletonEnabled)
                {
                    if (joints != null && lines != null)
                    {
                        #region UpdateDebugSkeleton
                        // overlay all joints in the skeleton
                        if (kinectManager.IsUserDetected(playerIndex))
                        {
                            ulong userId = kinectManager.GetUserIdByIndex(playerIndex);
                            int jointsCount = kinectManager.GetJointCount();

                            for (int i = 0; i < jointsCount; i++)
                            {
                                int joint = i;

                                if (kinectManager.IsJointTracked(userId, joint))
                                {
                                    Vector3 posJoint = !sensorTransform ? kinectManager.GetJointPosition(userId, joint) : kinectManager.GetJointKinectPosition(userId, joint, true);
                                    posJoint = new Vector3(posJoint.x * scaleFactors.x, posJoint.y * scaleFactors.y, posJoint.z * scaleFactors.z);

                                    if (sensorTransform)
                                    {
                                        posJoint = sensorTransform.transform.TransformPoint(posJoint);
                                    }

                                    if (joints != null)
                                    {
                                        // overlay the joint
                                        if (posJoint != Vector3.zero)
                                        {
                                            joints[i].SetActive(true);
                                            joints[i].transform.position = posJoint;
                                        }
                                        else
                                        {
                                            joints[i].SetActive(false);
                                        }
                                    }

                                    if (lines[i] == null && linePrefab != null)
                                    {
                                        lines[i] = Instantiate(linePrefab);  // as LineRenderer;
                                        lines[i].transform.parent = transform;
                                        lines[i].name = ((KinectInterop.JointType)i).ToString() + "_Line";
                                        lines[i].SetActive(false);
                                    }

                                    if (lines[i] != null)
                                    {
                                        // overlay the line to the parent joint
                                        int jointParent = (int)kinectManager.GetParentJoint((KinectInterop.JointType)joint);
                                        Vector3 posParent = Vector3.zero;

                                        if (kinectManager.IsJointTracked(userId, jointParent))
                                        {
                                            posParent = !sensorTransform ? kinectManager.GetJointPosition(userId, jointParent) : kinectManager.GetJointKinectPosition(userId, jointParent, true);
                                            posJoint = new Vector3(posJoint.x * scaleFactors.x, posJoint.y * scaleFactors.y, posJoint.z * scaleFactors.z);

                                            if (sensorTransform)
                                            {
                                                posParent = sensorTransform.transform.TransformPoint(posParent);
                                            }
                                        }

                                        if (posJoint != Vector3.zero && posParent != Vector3.zero)
                                        {
                                            lines[i].SetActive(true);

                                            Vector3 dirFromParent = posJoint - posParent;

                                            lines[i].transform.position = posParent + dirFromParent / 2f;
                                            lines[i].transform.up = transform.rotation * dirFromParent.normalized;

                                            Vector3 lineScale = lines[i].transform.localScale;
                                            lines[i].transform.localScale = new Vector3(lineScale.x, dirFromParent.magnitude / 2f, lineScale.z);
                                        }
                                        else
                                        {
                                            lines[i].SetActive(false);
                                        }
                                    }

                                }
                                else
                                {
                                    if (joints[i] != null)
                                    {
                                        joints[i].SetActive(false);
                                    }

                                    if (lines[i] != null)
                                    {
                                        lines[i].SetActive(false);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        DisableDebugSkeleton(kinectManager);
                    }
                }

                if (kinectManager.IsUserDetected(playerIndex))
                {
                    CalculateHandMovementBox();
                }
            }
        }

        private void DisableDebugSkeleton(KinectManager kinectManager)
        {
            int jointsCount = kinectManager.GetJointCount();

            for (int i = 0; i < jointsCount; i++)
            {
                if (joints[i] != null && joints[i].activeSelf)
                {
                    joints[i].SetActive(false);
                }

                if (lines[i] != null && lines[i].activeSelf)
                {
                    lines[i].SetActive(false);
                }
            }
        }
    }
}

