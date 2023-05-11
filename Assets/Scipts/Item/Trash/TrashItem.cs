using DG.Tweening;
using UnityEngine;
public class TrashItem : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out CollecterItem item))
        {
            if (item.AllOutItem())
                transform.DOScale(transform.localScale * 1.2f, 0.2f).
                    OnComplete(() => transform.DOScale(transform.localScale / 1.2f, 0.2f));
        }
    }
}
