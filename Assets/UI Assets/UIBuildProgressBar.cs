using UnityEngine;
using UnityEngine.UI;

public class UIBuildProgressBar : MonoBehaviour
{
    [SerializeField] private Image _fillBar;
    [SerializeField] private Color _colorStart = Color.red;
    [SerializeField] private Color _colorEnd = Color.green;

    private void Awake()
    {
        _fillBar.transform.localScale = new Vector3(0, 1, 1);
    }

    public void ModifyProgress(float percent)
    {
        _fillBar.transform.localScale = new Vector3(_fillBar.transform.localScale.x + percent, 1, 1);
        _fillBar.color = Color.Lerp(_colorStart,_colorEnd,_fillBar.transform.localScale.x);
    }

    public void Show(Transform targetTransform)
    {
        gameObject.transform.position = Camera.main.WorldToScreenPoint(targetTransform.position);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        _fillBar.transform.localScale = new Vector3(0, 1, 1);
    }
}