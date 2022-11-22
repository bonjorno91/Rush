using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
[ExecuteAlways]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] private Color _occupiedColor = Color.grey;
    [SerializeField] private Color _freeColor = Color.white;
    private TMP_Text _labelText;
    private Tile _tile;
    private Map _map;

    private void Awake()
    {
        _labelText = GetComponent<TMP_Text>();
        _map = FindObjectOfType<Map>();
        _tile = gameObject.transform.parent.GetComponent<Tile>();
        _labelText.enabled = false;
        UpdateCoordinates();
    }

    private void Update()
    {
        if (!Application.IsPlaying(gameObject) && gameObject.transform.parent.hasChanged) UpdateCoordinates();
        ToggleLabels();
    }

    private void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _labelText.enabled = !_labelText.enabled;

            if (_labelText.enabled)
            {
                if (_tile)
                {
                    _tile.IsPlaceable.Subscribe(OnPlaceableChanged);
                    OnPlaceableChanged(_tile.IsPlaceable.Value);
                }
            }
            else
            {
                if (_tile) _tile.IsPlaceable.Unsubscribe(OnPlaceableChanged);
            }
        }
    }

    private void OnPlaceableChanged(bool value)
    {
        _labelText.color = value ? _freeColor : _occupiedColor;
    }

    private void UpdateCoordinates()
    {
        gameObject.transform.parent.hasChanged = false;

        var x = Mathf.RoundToInt(gameObject.transform.parent.position.x / _map.SnapSize).ToString();
        var y = Mathf.RoundToInt(gameObject.transform.parent.position.z / _map.SnapSize).ToString();

        _labelText.text = $"{x},{y}";
        UpdateTileName();
    }

    private void UpdateTileName()
    {
        gameObject.transform.parent.gameObject.name = $"[{_labelText.text}]";
    }
}