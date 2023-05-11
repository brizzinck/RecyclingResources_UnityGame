using DG.Tweening;
using System.Collections;
using UnityEngine;
public class RecylItemController : MonoBehaviour
{
    [SerializeField] private BaseWarehouseBlock _stockMustRecylItem;
    [SerializeField] private BaseWarehouseBlock _stockRecylItem;
    [SerializeField] private BaseItem _mustRecylItem;
    [SerializeField] private BaseItem _recylItem;
    [SerializeField] private Transform _recylObjetc;
    private bool _isRecyl;
    private void Recyling(BaseItem mustRecylItem)
    {
        if (mustRecylItem == null)
            return;
        if (mustRecylItem.GetType() == _mustRecylItem.GetType())
            StartCoroutine(Recyling());
    }
    private IEnumerator Recyling()
    {
        if (!_isRecyl)
        {
            if (_stockRecylItem.GridItem.CheckFreeSlote())
            {
                _isRecyl = true;
                yield return new WaitForSeconds(0.2f);
                Sequence sequence = DOTween.Sequence();
                if (_stockMustRecylItem.GridItem.PeekItemInCell() != null)
                {
                    Transform itemTransform = _stockMustRecylItem.GridItem.PeekItemInCell().transform;
                    sequence.Append(itemTransform.DOLocalMove(new Vector3(2.5f, .32f, .4f), 1))
                        .Append(_recylObjetc.transform.DOLocalMoveY(.5f, 1))
                        .Append(_recylObjetc.transform.DOLocalMoveY(1f, 1))
                        .OnComplete(() =>
                        {
                            _stockMustRecylItem.OutStockItem().gameObject.SetActive(false);
                            BaseItem item = AllItemPool.GetItemsPool(_recylItem);
                            item.transform.parent = transform;
                            item.transform.transform.localPosition = new Vector3(-.7f, 1, .24f);
                            item.transform.DOLocalMove(new Vector3(.5f, 1, .24f), 1).OnComplete(() =>
                            {
                                _stockRecylItem.InStockItem(item);
                                _isRecyl = false;
                                StartCoroutine(Recyling());
                            });
                        });
                    sequence.Play();
                }
                else
                    _isRecyl = false;
            }
        }
    }
    private void Start()
    {
        _stockMustRecylItem.OnAddItemOfPlayer += Recyling;
        _stockRecylItem.OnOutItemOfPlayer += () => StartCoroutine(Recyling());

    }
    private void OnDestroy()
    {
        _stockMustRecylItem.OnAddItemOfPlayer -= Recyling;
        _stockRecylItem.OnOutItemOfPlayer -= () => StartCoroutine(Recyling());
    }
}
