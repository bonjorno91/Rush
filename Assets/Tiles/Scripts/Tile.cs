using Unity.Mathematics;
using UnityEngine;

namespace Tiles
{
    [SelectionBase]
    public class Tile : MonoBehaviour
    {
        [field: SerializeField] public ReactiveProperty<bool> IsPlaceable { get; private set; } = new(false);
        [SerializeField] private TowerFactory _towerFactory;
        [SerializeField] private TowerType _spawnTower;
        private IValueEntry<Tower> _content;

        private void OnMouseDown()
        {
            if (IsPlaceable.Value)
            {
                if (_towerFactory.TryGet(_spawnTower,gameObject.transform.position,quaternion.identity, out _content))
                {
                    IsPlaceable.Value = false;
                }
            }
        }
    }
}
