using FSM;
using UnityEngine;
using UnityEngine.Video;

public class IdleScreenState : FSMState
{
    [SerializeField]
    private VideoPlayer _videoPlayer;

    [SerializeField]
    private GameObject _videoImage;


    public override void Enter()
    {
        _videoImage.SetActive(true);
        _videoPlayer.Play();
    }

    public override void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _fsm.Transition<MainScreenState>();
        }
    }

    public override void Exit()
    {
        _videoImage.SetActive(false);
    }

    public void OnMainMenuAnimationComplete()
    {
        //TODO: add screen fade
    }
}
