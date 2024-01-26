using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class AuroraInstanceController : MonoBehaviour
{

    [SerializeField]
    private Transform _leftCursorTransform;

    [SerializeField]
    private float _radius;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Renderer auroraRenderer = GetComponent<Renderer>();
        auroraRenderer.material.SetFloat("_Radius", _radius);
        auroraRenderer.material.SetVector("_CursorPoint", _leftCursorTransform.position);

    }
}
