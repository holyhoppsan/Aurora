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
