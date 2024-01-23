using FSM;
using UnityEngine;
using UnityEngine.Playables;

public class AuroraAndFloraState : FSMState
{
    [SerializeField]
    private PlayableDirector _director;

    [SerializeField]
    private CameraController _cameraController;


    public override void Enter()
    {
        _cameraController.SwitchCamera("FlowerZoomInView");
    }

    public override void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _fsm.Transition<EndTransitionState>();
        }
    }

    public override void Exit()
    {

    }
}
