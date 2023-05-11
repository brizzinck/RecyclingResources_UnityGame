using UnityEngine;
[RequireComponent(typeof(Camera))]
public class MainCamera : MonoBehaviour
{
    private static Camera _camera;
    public static Camera Camera => _camera;
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.5f;
    [SerializeField] private Vector3 offset;
    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }
    private void Start()
    {
        offset = transform.position - target.position;
    }
    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
