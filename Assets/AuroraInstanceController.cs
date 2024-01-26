using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class AuroraInstanceController : MonoBehaviour
{

    [SerializeField]
    private Transform _leftCursorTransform;

    [SerializeField]
    private Transform _rightCursorTransform;

    [SerializeField]
    private InputManager _inputManager;

    [SerializeField]
    private float _radius;

    [SerializeField]
    public float fadeIntensity;

    [SerializeField]
    public bool _debugMode = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 leftCursorPosition = new Vector3();
        Vector3 rightCursorPosition = new Vector3();

        if (_debugMode)
        {
            leftCursorPosition = _leftCursorTransform.position;
            rightCursorPosition = _rightCursorTransform.position;
        }
        else
        {
            leftCursorPosition = GetHitPosition(_inputManager.LeftCursorScreenPosition);
            rightCursorPosition = GetHitPosition(_inputManager.RightCursorScreenPosition);
        }

        Renderer auroraRenderer = GetComponent<Renderer>();
        auroraRenderer.material.SetFloat("_Radius", _radius);
        auroraRenderer.material.SetVector("_LeftCursorPoint", leftCursorPosition);
        auroraRenderer.material.SetVector("_RightCursorPoint", rightCursorPosition);
        auroraRenderer.material.SetFloat("_FadeIntensity", fadeIntensity);

    }

    private Vector3 GetHitPosition(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        Vector3 leftCursorPosition = new Vector3();

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == GetComponent<BoxCollider>())
            {
                leftCursorPosition = hit.point;
            }
        }

        return leftCursorPosition;
    }
}
