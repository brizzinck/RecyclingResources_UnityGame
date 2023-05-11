using Extension;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class CollecterItem : MonoBehaviour
{
    public UnityAction<bool> OnItem;
    [SerializeField] private Transform _collectionStartPosition;
    [SerializeField] private TextMeshPro _textMax;
    [SerializeField] private int _maxTakeItem = 10;
    private Stack<BaseItem> _currentItems = new Stack<BaseItem>();
    private static CollecterItem instance;
    public Stack<BaseItem> CurrentItems => _currentItems;
    public static CollecterItem Instance => instance;
    public BaseItem OutItem()
    {
        if (_currentItems.Count > 0)
        {
            if (_currentItems.Count == 1)
                OnItem?.Invoke(false);
            DisplayTextMax(false);
            return _currentItems.Pop();
        }
        OnItem?.Invoke(false);
        return null;
    }

    private void DisplayTextMax(bool display = true)
    {
        if (_currentItems.Count > 0)
            _textMax.transform.position = _textMax.transform.position.SetY(_currentItems.Peek().transform.position.y - .5f);
        if (display)
            _textMax.text = "MAX";
        else
            _textMax.text = string.Empty;
    }

    public bool AllOutItem()
    {
        while (_currentItems.Count > 0)
        {
            BaseItem item = OutItem();
            item.gameObject.SetActive(false);
            item.transform.parent = null;
            if (_currentItems.Count == 0)
                return true;
        }
        return false;
    }
    public BaseItem PeekItem()
    {
        if (_currentItems.Count > 0)
            return _currentItems.Peek();
        return null;
    }
    public void TakeItem(BaseItem baseItem)
    {
        if (baseItem == null || CheckFullItem())
            return;
        if (_currentItems.Count > 0)
        {
            if (_currentItems.Peek().GetType() == baseItem.GetType())
                AddItem(baseItem);
        }
        else
            AddItem(baseItem);
    }
    public bool CheckFullItem()
    {
        return _currentItems.Count >= _maxTakeItem;
    }
    public bool CheckItem()
    {
        return PeekItem() != null;
    }
    private void Awake()
    {
        instance = this;
    }
    private void AddItem(BaseItem baseItem)
    {
        _currentItems.Push(baseItem);
        int index = _currentItems.Count - 1;
        Vector3 localPosition = Vector3.up * index * 0.2f;
        baseItem.SetTransform(localPosition, _collectionStartPosition, true);
        if (_currentItems.Count >= _maxTakeItem)
            DisplayTextMax();
        OnItem?.Invoke(true);
    }
}