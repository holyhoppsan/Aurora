using FSM;
using UnityEngine;
using UnityEngine.Playables;

public class AuroraAndFloraState : FSMState
{
    [SerializeField]
    private PlayableDirector _director;

    [SerializeField]
    private CameraController _cameraController;

    [SerializeField]
    private GameObject _auroras;

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
        _auroras.SetActive(false);
    }

    public void OnAuroraInteractionComplete()
    {
        _fsm.Transition<AuroraScreenState>();
    }
}
