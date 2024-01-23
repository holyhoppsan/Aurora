using FSM;
using UnityEngine;
using UnityEngine.Playables;

public class EndTransitionState : FSMState
{
    [SerializeField]
    private PlayableDirector _director;

    [SerializeField]
    private GameObject _auroras;

    public override void Enter()
    {
        if (_director != null)
        {
            _director.Stop();
            _director.time = 0;
            _director.Evaluate();
            _director.Play();
        }
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
        _auroras.SetActive(false);
    }

    public void OnEndTransitionComplete()
    {
        _fsm.Transition<IdleScreenState>();
    }
}
