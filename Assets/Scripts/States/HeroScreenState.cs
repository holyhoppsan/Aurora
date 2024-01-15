using FSM;
using UnityEngine;
using UnityEngine.Playables;

public class HeroScreenState : FSMState
{
    [SerializeField]
    private PlayableDirector _director;
    public override void Enter()
    {
        _director.Play();
    }

    public override void Tick()
    {

    }

    public override void Exit()
    {

    }

    public void OnHeroScreenComplete()
    {
        _fsm.Transition<AuroraScreenState>();
    }
}
