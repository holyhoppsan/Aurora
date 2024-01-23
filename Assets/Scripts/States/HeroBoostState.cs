using FSM;
using UnityEngine;
using UnityEngine.Playables;

public class HeroBoostState : FSMState
{
    [SerializeField]
    private PlayableDirector _director;

    [SerializeField]
    private CameraController _cameraController;

    public override void Enter()
    {
        if (_director != null)
        {
            _director.Stop();
            _director.time = 0;
            _director.Evaluate();
        }

        _cameraController.SwitchCamera("FlowerZoomInView");
    }

    public override void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _director.Play();
        }
    }

    public override void Exit()
    {

    }

    public void OnHeroBoostToAuroraSummonAnimationComplete()
    {
        _fsm.Transition<AuroraSummonState>();
    }
}