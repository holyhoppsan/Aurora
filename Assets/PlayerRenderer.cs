using System.Collections;
using System.Collections.Generic;
using com.rfilkov.kinect;
using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{
    [SerializeField]
    private KinectManager _kinectManager;

    [SerializeField]
    private Material _playerMaterial;
    // Start is called before the first frame update
    void Awake()
    {

    }

    void Update()
    {
        var texture = _kinectManager.GetUsersImageTex(0);

        if(texture)
        {
            _playerMaterial.SetTexture("_BaseMap", texture);
            var renderer = GetComponent<Renderer>();
            renderer.material = _playerMaterial;
        }
    }
}
