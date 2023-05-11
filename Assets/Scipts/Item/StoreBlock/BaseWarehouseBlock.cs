using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[Serializable]
internal class GridItem
{
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Vector2Int _x;
    [SerializeField] private Vector2Int _y;
    [SerializeField] private Vector2Int _z;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 DistanceBetweenObj;
    [SerializeField] private Vector3 RotateObj;
    private List<CellItem> _cellItems = new List<CellItem>();
    public void InitGrid()
    {
        for (int y = _y.x; y <= _y.y; y++)
        {
            for (int z = _z.x; z <= _z.y; z++)
            {
                for (int x = _x.x; x <= _x.y; x++)
                {
                    _cellItems.Add(new CellItem(new Vector3(
                        x * DistanceBetweenObj.x + _offset.x,
                        y * DistanceBetweenObj.y + _offset.y,
                        z * DistanceBetweenObj.z + _offset.z)));
                }
            }
        }
    }
    public void SpawnGrid(BaseItem baseItem)
    {
        for (int y = _y.x; y <= _y.y; y++)
        {
            for (int z = _z.x; z <= _z.y; z++)
            {
                for (int x = _x.x; x <= _x.y; x++)
                {
                    AddItemInCell(AllItemPool.GetItemsPool(baseItem));
                }
            }
        }
    }
    public bool AddItemInCell(BaseItem baseItem)
    {
        if (baseItem == null) 
            return false;
        foreach (CellItem cellItem in _cellItems)
        {
            if (!cellItem.IsBusy)
            {
                baseItem.gameObject.SetActive(true);
                baseItem.SetTransform(cellItem.Coordinates, _startPosition);
                if (RotateObj == Vector3.zero)
                    baseItem.transform.localRotation = Quaternion.identity;
                else
                    baseItem.transform.eulerAngles = RotateObj;
                cellItem.IsBusy = true;
                cellItem.SetBaseItem(baseItem);
                return true;
            }
        }
        return false;
    }
    public bool CheckFreeSlote()
    {
        foreach (CellItem cellItem in _cellItems)
        {
            if (!cellItem.IsBusy)
                return true;
        }
        return false;
    }
    public BaseItem PopItemInCell()
    {
        for (int i = _cellItems.Count - 1; i >= 0; i--)
        {
            if (_cellItems[i].IsBusy)
            {
                _cellItems[i].IsBusy = false;
                _cellItems[i].GetBaseItem().transform.parent = null;
                return _cellItems[i].GetBaseItem();
            }
        }
        return null;
    }
    public BaseItem PeekItemInCell()
    {
        for (int i = _cellItems.Count - 1; i >= 0; i--)
        {
            if (_cellItems[i].IsBusy)
                return _cellItems[i].GetBaseItem();
        }
        return null;
    }
}
[Serializable]
internal class CellItem
{
    public bool IsBusy;
    private BaseItem _baseItem;
    private Vector3 _coordinates;
    public void SetBaseItem(BaseItem baseItem) => _baseItem = baseItem;
    public BaseItem GetBaseItem() => _baseItem;
    public Vector3 Coordinates => _coordinates;
    public CellItem(Vector3 coordinates)
    {
        IsBusy = false;
        _coordinates = coordinates;
    }
}
public class BaseWarehouseBlock : MonoBehaviour
{
    public UnityAction<BaseItem> OnAddItemOfPlayer;
    public UnityAction OnOutItemOfPlayer;
    [SerializeField] private bool _outStack;
    [SerializeField] private bool _spawn;
    [SerializeField] private GridItem _gridItem;
    [SerializeField] private BaseItem _baseItem;
    internal GridItem GridItem => _gridItem;
    public bool InStockItem(BaseItem item)
    {
        if (item != null)
        {
            if (!CheckType(_baseItem, item))
                return false;
            return GridItem.AddItemInCell(item);
        }
        return false;
    }
    public BaseItem OutStockItem()
    {
        return _gridItem.PopItemInCell();
    }
    private void Start()
    {
        GridItem.InitGrid();
        SpawnItem();
    }
    private void SpawnItem()
    {
        if (_spawn)
            GridItem.SpawnGrid(_baseItem);
    }
    private void OnCollisionStay(Collision collision)
    {
        OperationOnItem(collision.transform);
    }
    private void OnTriggerStay(Collider other)
    {
        OperationOnItem(other.transform);
    }
    private void OperationOnItem(Transform objCollison)
    {
        if (objCollison.TryGetComponent(out CollecterItem collecterItem))
        {
            if (!_outStack)
                InStockItem(collecterItem);
            else
                OutStockItem(collecterItem);
        }
    }
    private void InStockItem(CollecterItem collecterItem)
    {
        if (!CheckType(_baseItem, collecterItem.PeekItem()))
            return;
        BaseItem baseItem = collecterItem.OutItem();
        if (baseItem == null) return;
        GridItem.AddItemInCell(baseItem);
        OnAddItemOfPlayer?.Invoke(baseItem);
    }
    private void OutStockItem(CollecterItem collecterItem)
    {
        if (!collecterItem.CheckFullItem())
        {
            if (collecterItem.CheckItem())
            {
                if (!CheckType(GridItem.PeekItemInCell(), collecterItem.PeekItem())) 
                    return;
            }
            collecterItem.TakeItem(GridItem.PopItemInCell());
            OnOutItemOfPlayer?.Invoke();
        }
    }
    private bool CheckType(BaseItem baseItem1, BaseItem baseItem2)
    {
        if (baseItem1 != null && baseItem2 != null)
            if (baseItem1.GetType() == baseItem2.GetType())
                return true;
        return false;
    }
}