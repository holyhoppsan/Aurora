using FSM;
using UnityEngine;
using UnityEngine.Playables;

public class AuroraInteractionState : FSMState
{
    [SerializeField]
    private PlayableDirector _director;

    [SerializeField]
    private CameraController _cameraController;

    [SerializeField]
    private GameObject _auroras;

    public override void Enter()
    {
        if (_director != null)
        {
            _director.Stop();
            _director.time = 0;
            _director.Evaluate();
        }

        _cameraController.SwitchCamera("AuroraView");
        _auroras.SetActive(true);
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

    public void OnAuroraAndFloraTransitionComplete()
    {
        _fsm.Transition<AuroraAndFloraState>();
    }
}
