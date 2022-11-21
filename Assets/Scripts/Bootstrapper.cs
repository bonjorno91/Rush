using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private InitializeOnStartSO[] _initializeSO;

    private void Awake()
    {
        foreach (var initializeOnStartSo in _initializeSO)
        {
            initializeOnStartSo.Initialize();
        }
    }
}
