using FSM;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Playables;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
using com.rfilkov.kinect;
#endif

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

    [SerializeField]
    private Material _avenMaterial;

    [SerializeField]
    private float _bootWait = 5.0f;

    private float _currentBootTimer = 0.0f;

    private bool _isBooting = false;

    public override void Enter()
    {
        SetupDefaultValues();

        if (_director != null)
        {
            _director.Stop();
            _director.time = 0;
            _director.Evaluate();
        }

        _videoImage.SetActive(true);
        _videoPlayer.Play();
        _cameraController.SwitchCamera("FlowerZoomInView");

        _currentBootTimer = 0.0f;
        _isBooting = false;
    }

    public override void Tick()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        KinectManager kinectManager = KinectManager.Instance;

        if (kinectManager && kinectManager.IsInitialized())
        {
            if (kinectManager.IsUserDetected(0))
            {
                if (!_isBooting)
                {
                    _currentBootTimer += Time.deltaTime;
                    Debug.Log($"Counting {_currentBootTimer}");

                    if (_currentBootTimer > _bootWait)
                    {
                        Debug.Log($"booting");
                        _director.Play();
                        _isBooting = true;
                    }
                }
            }
        }

#endif

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _director.Play();
        }
    }

    public override void Exit()
    {
        _videoPlayer.Stop();
        _videoImage.SetActive(false);
    }

    public void OnIdleFadeComplete()
    {
        _fsm.Transition<MainScreenState>();
    }

    private void SetupDefaultValues()
    {
        _avenMaterial.SetFloat("_EmissiveIntensity", 0.0f);
    }
}
