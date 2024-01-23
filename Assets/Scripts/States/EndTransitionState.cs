using FSM;
using UnityEngine;
using UnityEngine.Playables;

public class EndTransitionState : FSMState
{
    [SerializeField]
    private PlayableDirector _director;

    public override void Enter()
    {
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

    public void OnEndTransitionComplete()
    {
        _fsm.Transition<IdleScreenState>();
    }
}
