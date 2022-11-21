using UnityEngine;

[CreateAssetMenu(menuName = "Game/Factory/Enemy Factory", fileName = "EnemyFactory", order = 0)]
public class EnemyFactory : Factory<EnemyType,Enemy>
{
    public override void Initialize()
    {
        
    }

    protected override void OnRelease(EnemyType key, Enemy value)
    {
        base.OnRelease(key, value);
        value.gameObject.SetActive(false);
    }

    protected override void OnGet(EnemyType key, Enemy value)
    {
        base.OnGet(key, value);
        value.gameObject.SetActive(true);
    }
}