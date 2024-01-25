using FSM;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class AuroraSummonState : FSMState
{
    [SerializeField]
    private InputManager _inputManger;

    [SerializeField]
    private GameObject _runeCorners;

    [SerializeField]
    private RawImage _topLeftCorner;

    [SerializeField]
    private RawImage _topRightCorner;
    [SerializeField]
    private RawImage _bottomRightCorner;
    [SerializeField]
    private RawImage _bottomLeftCorner;

    [SerializeField]
    private PlayableDirector _director;

    [SerializeField]
    private CameraController _cameraController;

    private int _currentCornerIndex;
    private bool _inSequence;

    public override void Enter()
    {
        if (_director != null)
        {
            _director.Stop();
            _director.time = 0;
            _director.Evaluate();
        }

        Color color = Color.white;
        color.a = 0.0f;
        _topLeftCorner.color = color;
        _topRightCorner.color = color;
        _bottomRightCorner.color = color;
        _bottomLeftCorner.color = color;

        _currentCornerIndex = 0;
        _inSequence = true;

        _cameraController.SwitchCamera("AuroraView");

        _runeCorners.SetActive(true);
    }

    public override void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _director.Play();
        }

        if (_inSequence)
        {
            switch (_currentCornerIndex)
            {
                case 0:
                    {
                        Color color = _topLeftCorner.color;
                        color.a = 1.0f;
                        _topLeftCorner.color = color;

                        if (IsCircleCollidingWithRectTransform(_inputManger.LeftCursorScreenPosition, 32.0f, _topLeftCorner.rectTransform))
                        {
                            _topLeftCorner.color = Color.green;
                            _currentCornerIndex++;
                        }
                    }
                    break;
                case 1:
                    {
                        Color color = _topRightCorner.color;
                        color.a = 1.0f;
                        _topRightCorner.color = color;

                        if (IsCircleCollidingWithRectTransform(_inputManger.LeftCursorScreenPosition, 32.0f, _topRightCorner.rectTransform))
                        {
                            _topRightCorner.color = Color.green;
                            _currentCornerIndex++;
                        }
                    }
                    break;
                case 2:
                    {
                        Color color = _bottomRightCorner.color;
                        color.a = 1.0f;
                        _bottomRightCorner.color = color;

                        if (IsCircleCollidingWithRectTransform(_inputManger.LeftCursorScreenPosition, 32.0f, _bottomRightCorner.rectTransform))
                        {
                            _bottomRightCorner.color = Color.green;
                            _currentCornerIndex++;
                        }
                    }
                    break;
                case 3:
                    {
                        Color color = _bottomLeftCorner.color;
                        color.a = 1.0f;
                        _bottomLeftCorner.color = color;

                        if (IsCircleCollidingWithRectTransform(_inputManger.LeftCursorScreenPosition, 32.0f, _bottomLeftCorner.rectTransform))
                        {
                            _bottomLeftCorner.color = Color.green;
                            _currentCornerIndex++;
                        }
                    }
                    break;
                case 4:
                    {
                        _director.Play();
                        _inSequence = false;
                        _runeCorners.SetActive(false);
                    }
                    break;
                default:
                    {
                        throw new System.Exception("Invalid cornerIndex state");
                    }
            }
        }
    }

    public override void Exit()
    {

    }

    public void OnAuroraSummonComplete()
    {
        _fsm.Transition<AuroraInteractionState>();
    }
    bool IsCircleCollidingWithRectTransform(Vector2 circleCenter, float radius, RectTransform rectTransform)
    {
        // Convert the corners of the RectTransform to screen space
        Rect rect = rectTransform.rect;
        Vector2 rectCenter = (Vector2)rectTransform.position + new Vector2(rect.x, rect.y);
        Vector2 halfSize = new Vector2(rect.width / 2, rect.height / 2);

        // Find the closest point on the rectangle to the center of the circle
        Vector2 closestPoint = new Vector2(
            Mathf.Clamp(circleCenter.x, rectCenter.x - halfSize.x, rectCenter.x + halfSize.x),
            Mathf.Clamp(circleCenter.y, rectCenter.y - halfSize.y, rectCenter.y + halfSize.y)
        );

        // Calculate the distance between this point and the center of the circle
        float distanceSquared = (closestPoint - circleCenter).sqrMagnitude;

        // Check if the distance is less than or equal to the radius squared
        return distanceSquared <= (radius * radius);
    }
}
