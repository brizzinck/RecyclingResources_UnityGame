using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private Transform _skin;
    private Rigidbody _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        TouchInputer.Instance.OnTouch += MovePLayer;
        TouchInputer.Instance.OnEndTouch += EndMove;
    }
    private void MovePLayer(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            transform.LookAt(direction);
            direction.Normalize();
            _rigidbody.velocity = transform.forward * _movementSpeed;
            PlayerAnimation.SetRun();
        }
        else
            _rigidbody.velocity = Vector3.zero; 
    }
    private void EndMove()
    {
        PlayerAnimation.SetRun(false);
    }
    private void OnDestroy()
    {
        TouchInputer.Instance.OnTouch -= MovePLayer;
        TouchInputer.Instance.OnEndTouch -= EndMove;
    }
}
