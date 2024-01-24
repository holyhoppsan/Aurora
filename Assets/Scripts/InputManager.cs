using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Transform _leftCursor;

    [SerializeField]
    private Transform _rightCursor;

    [SerializeField]
    private bool _mouseInputEnabled = false;

    public Vector2 LeftCursorScreenPosition
    {
        get
        {
            return Camera.main.WorldToScreenPoint(_leftCursor.transform.position);
        }
    }

    public Vector2 RightCursorScreenPosition
    {
        get
        {
            return Camera.main.WorldToScreenPoint(_rightCursor.transform.position);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_mouseInputEnabled)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 1.0f; // Distance from the camera
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

            if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
            {
                Cursor.visible = false;
                _leftCursor.transform.position = worldPosition;
            }
            else if (Input.GetMouseButton(1) && !Input.GetMouseButton(0))
            {
                Cursor.visible = false;
                _rightCursor.transform.position = worldPosition;
            }
            else
            {
                Cursor.visible = true;
            }
        }
    }
}
