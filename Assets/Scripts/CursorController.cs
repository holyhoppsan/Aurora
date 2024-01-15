using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField]
    private Vector2 _velocity;

    [SerializeField]
    public TrailRenderer _trailRenderer;

    [SerializeField]
    public TrailRenderer _smallTrailRenderer;

    private float _initialTraillMultiplier;

    private float _initialSmallTraillMultiplier;

    private float _initialTraillTime;

    private float _initialSmallTraillTime;

    void Awake()
    {
        // hack to update trail width when the project is awakened
        _initialTraillMultiplier = _trailRenderer.widthMultiplier;
        _initialSmallTraillMultiplier = _smallTrailRenderer.widthMultiplier;
        _initialTraillTime = _trailRenderer.time;
        _initialSmallTraillTime = _smallTrailRenderer.time;

        _trailRenderer.widthMultiplier = transform.localScale.y * _initialTraillMultiplier;
        _smallTrailRenderer.widthMultiplier = transform.localScale.y * _initialSmallTraillMultiplier;
        //_trailRenderer.time = transform.localScale.x * _initialTraillTime;
        //_smallTrailRenderer.time = transform.localScale.x * _initialSmallTraillTime;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (gameObject.transform.position.x > 10.0f)
        {
            _velocity.x *= -1.0f;
        }

        if (gameObject.transform.position.x < -10.0f)
        {
            _velocity.x *= -1.0f;
        }

        gameObject.transform.position += new Vector3(_velocity.x * Time.deltaTime, 0.0f, 0.0f);
    }
}
