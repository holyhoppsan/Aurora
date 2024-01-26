using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class AuroraInstanceController : MonoBehaviour
{

    [SerializeField]
    private Transform _leftCursorTransform;

    [SerializeField]
    private InputManager _inputManager;

    [SerializeField]
    private float _radius;

    [SerializeField]
    public float fadeIntensity;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(_inputManager.LeftCursorScreenPosition);
        RaycastHit hit;

        //Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100.0f);

        Vector4 leftCursorPosition = new Vector3();

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == GetComponent<BoxCollider>())
            {
                leftCursorPosition = hit.point;
            }
        }

        Renderer auroraRenderer = GetComponent<Renderer>();
        auroraRenderer.material.SetFloat("_Radius", _radius);
        auroraRenderer.material.SetVector("_CursorPoint", leftCursorPosition);
        auroraRenderer.material.SetFloat("_FadeIntensity", fadeIntensity);

    }
}
