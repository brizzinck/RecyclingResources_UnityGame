using DG.Tweening;
using Extension;
using UnityEngine;
public class BaseItem : MonoBehaviour
{
    [SerializeField] private Vector3 _positionOffset;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private Vector3 _scale;
    [SerializeField] private float _offsetYScale;
    private Vector3 _baseRotation;
    private Vector3 _baseScale;

    public void SetTransform(Vector3 position, Transform parent, bool offsetYScale = false)
    {
        transform.SetParent(parent);
        AnimationActivete(position, offsetYScale);
        transform.localEulerAngles = _rotation;
    }
    public void AnimationActivete(Vector3 position, bool offsetYScale = false)
    {
        transform.localScale = _scale.Increas(1.2f);
        if (offsetYScale)
            transform.localScale = transform.localScale.SetY(_scale.y / _offsetYScale);
        transform.localPosition = position + _positionOffset + Vector3Int.up;
        Vector3 scale = offsetYScale ? _scale.SetY(_scale.y / _offsetYScale) : _scale;
        transform.DOScale(scale, .2f);
        transform.DOLocalMove(position + _positionOffset, .2f);
    }
    private void OnEnable()
    {
        transform.eulerAngles = _baseRotation;
        transform.localScale = _scale;
    }
    private void Awake()
    {
        _baseRotation = transform.eulerAngles;
    }
}
