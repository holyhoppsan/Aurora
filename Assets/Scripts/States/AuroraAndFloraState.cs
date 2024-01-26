using FSM;
using UnityEngine;
using UnityEngine.Playables;

public class AuroraAndFloraState : FSMState
{
    [SerializeField]
    private PlayableDirector _director;

    [SerializeField]
    private CameraController _cameraController;

    [SerializeField]
    private float _stateDuration = 3.0f;

    private float _currentTimer;


    public override void Enter()
    {
        _cameraController.SwitchCamera("FlowerZoomInView");
        _currentTimer = 0.0f;
    }

    public override void Tick()
    {
        _currentTimer += Time.deltaTime;

        if (_currentTimer >= _stateDuration || Input.GetKeyDown(KeyCode.Space))
        {
            _fsm.Transition<EndTransitionState>();
        }
    }

    public override void Exit()
    {

    }
}
