using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public void SwitchCamera(string cameraName)
    {
        foreach (Transform child in transform)
        {
            var vcam = child.GetComponent<CinemachineVirtualCamera>();
            if (vcam != null)
            {
                vcam.enabled = child.name == cameraName;
            } 
        }
    }
}
