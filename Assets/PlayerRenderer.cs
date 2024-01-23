#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
using com.rfilkov.kinect;
#endif

using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    [SerializeField]
    private KinectManager _kinectManager;
#endif

    [SerializeField]
    private Material _playerMaterial;
    // Start is called before the first frame update
    void Awake()
    {

    }

    void Update()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        var texture = _kinectManager.GetUsersImageTex(0);

        if(texture)
        {
            _playerMaterial.SetTexture("_BaseMap", texture);
            var renderer = GetComponent<Renderer>();
            renderer.material = _playerMaterial;
        }
#endif
    }
}
