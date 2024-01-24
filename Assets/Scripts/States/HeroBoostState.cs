using FSM;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;

public class HeroBoostState : FSMState
{
    [SerializeField]
    private PlayableDirector _director;

    [SerializeField]
    private CameraController _cameraController;

    [SerializeField]
    private Material _avenMaterial;

    [SerializeField]
    private float _emissiveBoostSpeed;

    [SerializeField]
    private float _emissiveBoostCooldown;

    private float _currentEmissive;

    public override void Enter()
    {
        if (_director != null)
        {
            _director.Stop();
            _director.time = 0;
            _director.Evaluate();
        }

        _cameraController.SwitchCamera("FlowerZoomInView");

        _currentEmissive = 0.0f;

        _avenMaterial.SetFloat("_EmissiveIntensity", _currentEmissive);
    }

    public override void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _director.Play();
        }

        if (Input.GetKey(KeyCode.A))
        {
            _currentEmissive += Time.deltaTime * _emissiveBoostSpeed;
            _currentEmissive = Mathf.Min(1.0f, _currentEmissive);
        }

        if (_currentEmissive < 1.0f)
        {
            _currentEmissive = Mathf.Max(0.0f, _currentEmissive - (Time.deltaTime * _emissiveBoostCooldown));
        }
        else
        {
            _director.Play();
        }

        _avenMaterial.SetFloat("_EmissiveIntensity", _currentEmissive);
    }

    public override void Exit()
    {

    }

    public void OnHeroBoostToAuroraSummonAnimationComplete()
    {
        _fsm.Transition<AuroraSummonState>();
    }
}