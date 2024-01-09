using FSM;
using UnityEngine;
using Cinemachine;

public class LogoScreenState : FSMState
{
    [SerializeField]
    private CinemachineVirtualCameraBase _flowerCam;

    [SerializeField]
    private CinemachineVirtualCameraBase _mainCam;
    public override void Enter()
    {
        _flowerCam.Priority = 1;
        _mainCam.Priority = 0;
    }

    public override void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Replace with your condition
        {
            TriggerBlend();
        }
    }

    public override void Exit()
    {

    }

    private void TriggerBlend()
    {
        // Set the priority of camera2 higher than camera1 to make it active
        _flowerCam.Priority = 0;
        _mainCam.Priority = 1; // You can use higher numbers to ensure it takes precedence
    }
}
