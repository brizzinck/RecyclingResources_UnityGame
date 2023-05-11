using UnityEngine;
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private static Animator _animator;
    public static void SetRun(bool run = true)
    {
        if (!run)
            SetStay();
        _animator.SetBool("IsRunning", run);
    }
    public static void SetBoxStay(bool isBox = true)
    {
        SetStay();
        _animator.SetBool("IsHandBox", isBox);
    }
    public static void SetStay()
    {
        _animator.SetTrigger("Stay");
    }
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        CollecterItem.Instance.OnItem += SetBoxStay;
    }
    private void OnDestroy()
    {
        CollecterItem.Instance.OnItem -= SetBoxStay;
    }
}
