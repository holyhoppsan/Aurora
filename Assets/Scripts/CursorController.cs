using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField]
    private Vector2 _velocity;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x > 2.0f)
        {
            _velocity.x *= -1.0f;
        }

        if (gameObject.transform.position.x < -2.0f)
        {
            _velocity.x *= -1.0f;
        }

        gameObject.transform.position += new Vector3(_velocity.x * Time.deltaTime, 0.0f, 0.0f);
    }
}
