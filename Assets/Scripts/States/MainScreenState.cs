using FSM;
using UnityEngine;
using UnityEngine.Playables;

public class MainScreenState : FSMState
{
    [SerializeField]
    private PlayableDirector _director;

    [SerializeField]
    private GameObject _background;

    [SerializeField]
    private GameObject _logo;

    public override void Enter()
    {
        if (_director != null)
        {
            _director.Stop();
            _director.time = 0;
            _director.Evaluate();
            _director.Play();
        }

        _background.SetActive(true);
        _logo.SetActive(true);
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
        _background.SetActive(true);
        _logo.SetActive(true);
    }

    public void OnMainMenuAnimationComplete()
    {
        _fsm.Transition<HeroScreenState>();
    }
}
