using FSM;
using UnityEngine;
using UnityEngine.Playables;

public class MainScreenState : FSMState
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

    public void OnMainMenuAnimationComplete()
    {
        _fsm.Transition<LogoScreenState>();
    }
}
