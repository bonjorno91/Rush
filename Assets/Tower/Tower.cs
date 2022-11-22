using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [field: SerializeField] public int GoldCost { get; private set; }
    [SerializeField] private float _partBuildDelay = 1;

    private IEnumerator Start()
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
            foreach (Transform grandChild in child.transform)
            {
                grandChild.gameObject.SetActive(false);
            }
        }
        
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(_partBuildDelay);
            foreach (Transform grandChild in child.transform)
            {
                grandChild.gameObject.SetActive(true);
            }
        }
    }
}