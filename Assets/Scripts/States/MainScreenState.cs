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
        _background.SetActive(true);
        _logo.SetActive(true);
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
        _background.SetActive(true);
        _logo.SetActive(true);
    }

    public void OnMainMenuAnimationComplete()
    {
        _fsm.Transition<HeroScreenState>();
    }
}
