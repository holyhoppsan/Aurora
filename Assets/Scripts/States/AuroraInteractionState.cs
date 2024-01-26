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

    [SerializeField]
    private float _stateDuration = 3.0f;

    private float _currentTimer;

    public override void Enter()
    {
        if (_director != null)
        {
            _director.Stop();
            _director.time = 0;
            _director.Evaluate();
        }

        _cameraController.SwitchCamera("AuroraView");
        _auroras.GetComponent<AuroraController>()._AuroraFade = 1.0f;
        _auroras.SetActive(true);

        _currentTimer = 0.0f;
    }

    public override void Tick()
    {
        _currentTimer += Time.deltaTime;

        if (_currentTimer >= _stateDuration || Input.GetKeyDown(KeyCode.Space))
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
