using FSM;
using UnityEngine;
using Cinemachine;

public class AuroraScreenState : FSMState
{
    [SerializeField]
    private CinemachineVirtualCameraBase _auroraCam;

    [SerializeField]
    private CinemachineVirtualCameraBase _flowerCam;

    [SerializeField]
    private CinemachineVirtualCameraBase _mainCam;

    public override void Enter()
    {
        _auroraCam.Priority = 1;
        _flowerCam.Priority = 0;
        _mainCam.Priority = 0;
    }

    public override void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
    }

    public override void Exit()
    {

    }
}
