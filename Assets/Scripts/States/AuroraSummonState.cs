using FSM;
using UnityEngine;
using UnityEngine.Playables;

public class AuroraSummonState : FSMState
{
    [SerializeField]
    private PlayableDirector _director;

    [SerializeField]
    private CameraController _cameraController;

    public override void Enter()
    {
        _cameraController.SwitchCamera("AuroraView");
    }

    public override void Tick()
    {

    }

    public override void Exit()
    {

    }

    public void OnAuroraSummonComplete()
    {
        _fsm.Transition<HeroBoostState>();
    }
}
