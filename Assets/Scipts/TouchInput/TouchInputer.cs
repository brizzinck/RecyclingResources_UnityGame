using Extension;
using UnityEngine;
using UnityEngine.Events;
public class TouchInputer : MonoBehaviour
{
    public UnityAction<Vector3> OnTouch;
    public UnityAction OnEndTouch;
    private static TouchInputer instance;
    public static TouchInputer Instance => instance;
    private Vector3 startPosition;
    private Vector3 currentPosition;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (Input.touchSupported)
            TouchInput();
        else
            MouseInput();
    }
    private void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
                startPosition = touch.position;
            if (touch.phase == TouchPhase.Moved)
            {
                currentPosition = touch.position;
                SetDirection();
            }
            if (touch.phase == TouchPhase.Ended)
            {
                OnTouch?.Invoke(Vector2.zero);
                OnEndTouch?.Invoke();
            }
        }
    }
    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
            startPosition = Input.mousePosition;       
        if (Input.GetMouseButton(0))
        {
            currentPosition = Input.mousePosition;
            SetDirection();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnTouch?.Invoke(Vector2.zero);
            OnEndTouch?.Invoke();
        }
    }
    private void SetDirection()
    {
        Vector3 direction = currentPosition - startPosition;
        OnTouch?.Invoke(direction.SetYinZ());
    }
}
