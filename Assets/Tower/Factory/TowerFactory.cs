using UnityEngine;

[CreateAssetMenu(menuName = "Game/Factory/Tower Factory", fileName = "TowerFactory", order = 0)]
public class TowerFactory : Factory<TowerType, Tower>
{
    [SerializeField] private BankAccount _account;
    public override void Initialize()
    {
        
    }

    public override bool TryGet(TowerType key, Vector3 position, Quaternion rotation, out IValueEntry<Tower> result)
    {
        result = null;
        if (!Config.Entries.ContainsKey(key)) return false;

        if (_account.CurrentBalance >= Config.Entries[key].GoldCost)
        {
            if (base.TryGet(key, position, rotation, out result))
            {
                _account.Withdraw(result.Value.GoldCost);
                return true;
            }
        }

        Debug.Log("Not enough gold.");
        return false;
    }

    protected override void OnCreate(TowerType key, Tower value)
    {
        
    }

    protected override void OnGet(TowerType key, Tower value)
    {
        
    }

    protected override void OnRelease(TowerType key, Tower value)
    {
        
    }

    protected override void OnRemove(TowerType key, Tower value)
    {
        
    }
}