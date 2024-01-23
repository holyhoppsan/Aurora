using FSM;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Playables;

public class IdleScreenState : FSMState
{
    [SerializeField]
    private VideoPlayer _videoPlayer;

    [SerializeField]
    private GameObject _videoImage;

    [SerializeField]
    private PlayableDirector _director;

    [SerializeField]
    private CameraController _cameraController;

    public override void Enter()
    {
        _videoImage.SetActive(true);
        _videoPlayer.Play();
        _cameraController.SwitchCamera("FlowerZoomInView");
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
        _videoImage.SetActive(false);
    }

    public void OnIdleFadeComplete()
    {
        _fsm.Transition<MainScreenState>();
    }
}