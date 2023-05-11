using UnityEngine;
public class CollecterTextInfo : MonoBehaviour
{
    private Camera _camera;
    protected virtual void Awake()
    {
        _camera = MainCamera.Camera;
    }
    private void LateUpdate()
    {
        transform.LookAt(new Vector3(transform.position.x, _camera.transform.position.y, _camera.transform.position.z));
        transform.Rotate(0, 180, 0);
    }
}
