using FSM;
using UnityEngine;
using UnityEngine.Playables;

public class HeroScreenState : FSMState
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

        _director.Play();
        _cameraController.SwitchCamera("FlowerZoomInView");
    }

    public override void Tick()
    {

    }

    public override void Exit()
    {

    }

    public void OnHeroScreenComplete()
    {
        _fsm.Transition<HeroBoostState>();
    }
}
