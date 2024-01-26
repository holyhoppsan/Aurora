using FSM;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Video;

public class MainScreenState : FSMState
{
    [SerializeField]
    private VideoPlayer _videoPlayer;

    [SerializeField]
    private GameObject _videoImage;

    [SerializeField]
    private GameObject _fader;

    public override void Enter()
    {
        _fader.SetActive(false);

        _videoImage.SetActive(true);
        _videoPlayer.Prepare();
        _videoPlayer.loopPointReached += OnMainMenuAnimationComplete;
        _videoPlayer.Play();
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
        _videoPlayer.Stop();
        _videoImage.SetActive(false);
        _fader.SetActive(true);
    }

    public void OnMainMenuAnimationComplete(UnityEngine.Video.VideoPlayer vp)
    {
        _fsm.Transition<HeroScreenState>();
    }
}
