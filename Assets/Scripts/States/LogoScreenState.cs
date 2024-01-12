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
        _fsm.Transition<LogoScreenState>();
    }
}
