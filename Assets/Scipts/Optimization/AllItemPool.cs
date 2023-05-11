using Optimization;
using UnityEngine;
public class AllItemPool : MonoBehaviour
{
    [SerializeField] private AmmoItem _ammoItem;
    [SerializeField] private RocketItem _rocketItem;
    private static PoolMono<AmmoItem> _ammoItemPool;
    private static PoolMono<RocketItem> _rocketItemPool;

    public static BaseItem GetItemsPool<T>(T t) where T : BaseItem
    {
        if (t is AmmoItem)
            return _ammoItemPool.GetFreeElement(true);
        if (t is RocketItem)
            return _rocketItemPool.GetFreeElement(true);
        else
            return null;
    }
    private void Awake()
    {
        _ammoItemPool = new PoolMono<AmmoItem>(_ammoItem, 25, true);
        _rocketItemPool = new PoolMono<RocketItem>(_rocketItem, 25, true);
    }
}
