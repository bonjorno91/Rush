using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UITowerBuildPanel : MonoBehaviour
{
    [SerializeField] private TowerFactory _towerFactory;
    private Tile _targetTile;
    private Camera _camera;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _camera = Camera.main;
        gameObject.SetActive(false);
    }

    public void OnBuild(Tile tile)
    {
        _targetTile = tile;
        transform.position = Input.mousePosition;
        gameObject.SetActive(true);
        _audioSource.Play();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _targetTile = null;
            gameObject.SetActive(false);
        }
    }

    public void BuildTower(UITowerBuildAction action)
    {
        if (_towerFactory.TryGet(action.TowerType, _targetTile.transform.position, Quaternion.identity, out var towerValue))
        {
            _targetTile.PlaceTower(towerValue);
        }
        
        gameObject.SetActive(false);
    }
}